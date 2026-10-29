using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

public class UnitSimulation
{ 
    private const int CAPACITY = 4096;
    private const int RESOLVE_COUNT = 3;
    private List<Unit> _units;

    private NativeList<float2> _positions;
    private NativeList<float2> _velocities;
    private NativeList<float> _radii;
    private NativeList<float> _weights;

    private NativeList<float2> _predictedPositions;

    private float cellSize = 3f;
    public NativeParallelMultiHashMap<int2, int> Grid;

    public UnitSimulation()
    {
        _positions = new(CAPACITY, Allocator.Persistent);
        _velocities = new(CAPACITY, Allocator.Persistent);
        _radii = new(CAPACITY, Allocator.Persistent);
        _weights = new(CAPACITY, Allocator.Persistent);
        _predictedPositions = new(CAPACITY, Allocator.Persistent);

        Grid = new(CAPACITY, Allocator.Persistent);
    }

    #region schedule
    private JobHandle ScheduleResolveUnitOverlap(int unitCount, JobHandle dependency = default)
    {
        float maxRadius = 0f;
        for (int i = 0; i < unitCount; i++)
        {
            maxRadius = math.max(maxRadius, _radii[i]);
        }

        var job = new ResolveUnitOverlapJob
        {
            Positions = _positions.AsArray(),
            PredictedPositions = _predictedPositions.AsArray(),
            Radii = _radii.AsArray(),
            Grid = Grid,
            CellSize = cellSize,
            MaxRadius = maxRadius
        };

        return job.ScheduleParallel(unitCount, 64, dependency);
    }
    private JobHandle ScheduleResolveObstacleCollision(int unitCount, JobHandle dependency = default)
    {
        var job = new ResolveObstacleCollisionJob { };

        return job.ScheduleParallel(unitCount, 64, dependency);
    }
    #endregion

    public void Dispose()
    {
        if (_positions.IsCreated) _positions.Dispose();
        if (_velocities.IsCreated) _velocities.Dispose();
        if (_radii.IsCreated) _radii.Dispose();
        if (_weights.IsCreated) _weights.Dispose();
        if (_predictedPositions.IsCreated) _predictedPositions.Dispose();

        if (Grid.IsCreated) Grid.Dispose();
    }

    public void SyncPositions(List<Unit> units)
    {
        _units = units;

        _positions.Clear();

        foreach (var unit in units)
        {
            _positions.Add(new float2(unit.transform.position.x, unit.transform.position.y));
        }

        BuildSpatialGrid();
    }

    public void Simulate()
    {
        //이동 예측에 필요한 데이터들 동기화
        _velocities.Clear();
        _radii.Clear();
        _weights.Clear();

        for (int i = 0; i < _units.Count; i++)
        {
            _velocities.Add(_units[i].Velocity);
            _radii.Add(_units[i].Radius);
            //임시 무게 1로 통일
            _weights.Add(1);

            _positions[i] += _velocities[i] * Time.deltaTime;
        }

        //보정 반복
        _predictedPositions.ResizeUninitialized(_positions.Length);

        for (int i = 0; i < RESOLVE_COUNT; i++)
        {
            //예측 그리드 생성
            BuildSpatialGrid();

            //유닛 겹침 보정
            JobHandle unitOverlapHandle = ScheduleResolveUnitOverlap(_units.Count);
            //장애물 겹침 보정
            JobHandle obstacleCollisionHandle = ScheduleResolveObstacleCollision(_units.Count, unitOverlapHandle);
            //완료 대기
            obstacleCollisionHandle.Complete();

            //예측 버퍼와 교환
            var temp = _positions;
            _positions = _predictedPositions;
            _predictedPositions = temp;
        }
    }

    /// <summary>
    /// 실제 유닛에 위치 적용
    /// </summary>
    public void ApplyMovementStates()
    {
        if (_units.Count != _positions.Length)
        {
            Debug.LogWarning("[Unit Simulation]Maybe Unit list has Modified.");
            return;
        }

        for (int i = 0; i < _units.Count; i++)
        {
            _units[i].SetPosition(_positions[i]);
        }
    }

    /// <summary>
    /// 유닛들 위치로 그리드 재 생성
    /// </summary>
    public void BuildSpatialGrid()
    {
        Grid.Clear();

        for (int id = 0; id < _units.Count; id++)
        {
            int2 cell = (int2)math.floor(_positions[id] / cellSize);

            Grid.Add(cell, id);
        }
    }

    /// <summary>
    /// 유닛 기준 가까운 유닛을 타겟으로 설정
    /// </summary>
    /// <param name="seeker">기준이 되는 유닛</param>
    /// <returns></returns>
    public Unit FindNearestTarget(Unit seeker)
    {
        float radius = seeker.TargetRange;

        if (seeker == null || radius <= 0f || !Grid.IsCreated)
            return null;

        if (seeker.Data == null)
            return null;

        Team targetTeam;

        switch (seeker.Data.Team)
        {
            case Team.Ally:
                targetTeam = Team.Enemy;
                break;
            case Team.Enemy:
                targetTeam = Team.Ally;
                break;
            default:
                return null;
        }

        float2 origin = new float2(
            seeker.transform.position.x,
            seeker.transform.position.y);

        int2 minCell = (int2)math.floor((origin - radius) / cellSize);
        int2 maxCell = (int2)math.floor((origin + radius) / cellSize);

        Unit nearest = null;
        float nearestDistanceSq = radius * radius;

        for (int y = minCell.y; y <= maxCell.y; y++)
        {
            for (int x = minCell.x; x <= maxCell.x; x++)
            {
                int2 cell = new int2(x, y);

                if (!Grid.TryGetFirstValue(cell, out int index, out var iterator))
                    continue;

                do
                {
                    Unit candidate = _units[index];

                    if (candidate == null ||
                        candidate == seeker ||
                        !candidate.isActiveAndEnabled ||
                        candidate.Hp <= 0 ||
                        candidate.Data == null ||
                        candidate.Data.Team != targetTeam)
                    {
                        continue;
                    }

                    float distanceSq =
                        math.distancesq(origin, _positions[index]);

                    if (distanceSq > nearestDistanceSq)
                        continue;

                    nearestDistanceSq = distanceSq;
                    nearest = candidate;
                }
                while (Grid.TryGetNextValue(out index, ref iterator));
            }
        }

        return nearest;
    }
}

[BurstCompile]
public struct ResolveUnitOverlapJob : IJobFor
{
    [ReadOnly] public NativeArray<float2> Positions;
    [ReadOnly] public NativeArray<float> Radii;
    [ReadOnly] public NativeParallelMultiHashMap<int2, int> Grid;
    [WriteOnly] public NativeArray<float2> PredictedPositions;

    public float CellSize;
    public float MaxRadius;

    public void Execute(int id)
    {
        float radius = Radii[id] + MaxRadius;
        float2 origin = Positions[id];

        float2 correction = float2.zero;
        int contactCount = 0;

        int2 minCell = (int2)math.floor((origin - radius) / CellSize);
        int2 maxCell = (int2)math.floor((origin + radius) / CellSize);

        for (int x = minCell.x; x <= maxCell.x; x++)
        {
            for (int y = minCell.y; y <= maxCell.y; y++)
            {
                int2 cell = new int2(x, y);
                if (!Grid.TryGetFirstValue(cell, out int otherId, out var iterator))
                    continue;

                do
                {
                    if (otherId == id) continue;

                    float2 difference = origin - Positions[otherId];
                    float distanceSq = math.lengthsq(difference);
                    float combinedRadius = Radii[id] + Radii[otherId];

                    if (distanceSq >= combinedRadius * combinedRadius) continue;

                    float distance = math.sqrt(distanceSq);
                    // 중심이 같아도 두 유닛이 서로 반대 방향으로 분리되도록 합니다.
                    float2 direction = distance > 0f
                        ? difference / distance
                        : new float2(id < otherId ? -1f : 1f, 0f);

                    correction += direction * ((combinedRadius - distance) * 0.5f);
                    contactCount++;
                }
                while (Grid.TryGetNextValue(out otherId, ref iterator));
            }
        }

        PredictedPositions[id] = origin + (contactCount > 0 ? correction / contactCount : float2.zero);
    }
}

[BurstCompile]
public struct ResolveObstacleCollisionJob : IJobFor
{
    public void Execute(int id) { }
}
