using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Unity.Collections;
using Unity.Mathematics;
using MoreMountains.Tools;
using Unity.Profiling;

public class UnitManager : MonoBehaviour
{
    private static readonly ProfilerMarker SyncMarker =
    new("Units.SyncPositions");

    private static readonly ProfilerMarker ThinkMarker =
        new("Units.ThinkByGroup");

    private static readonly ProfilerMarker SimulateMarker =
        new("Units.Simulate");

    private static readonly ProfilerMarker ApplyMarker =
        new("Units.ApplyMovementStates");

    // 클래스 필드
    private static readonly ProfilerMarker FindTargetMarker =
        new("Units.FindNearestTarget");

    private static readonly ProfilerMarker UnitThinkMarker =
        new("Units.UnitThink");

    public static UnitManager Instance { get; private set; }

    [SerializeField] private UnitPool unitPool;
    private UnitSimulation _simulation;

    private readonly List<Unit> _units = new();
    private int _nextId;

    #region life-cycle
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (unitPool == null) unitPool = GetComponentInChildren<UnitPool>();
        _simulation = new();
    }

    private void Start()
    {
        StartCoroutine(TestSpawnLoop());
    }

    private void Update()
    {
        ProcessDespawn();
        ProcessSpawn();

        using (SyncMarker.Auto())
        {
            _simulation.SyncPositions(_units);
        }

        using (ThinkMarker.Auto())
        {
            ThinkByGroup();
        }

        using (SimulateMarker.Auto())
        {
            _simulation.Simulate();
        }

        using (ApplyMarker.Auto())
        {
            _simulation.ApplyMovementStates();
        }
    }

    private void OnDestroy()
    {
        _simulation?.Dispose();
    }
    #endregion

    #region spawn
    private struct SpawnRequest
    {
        public UnitData Data;
        public Vector2 Position;
    }
    private readonly Queue<SpawnRequest> _spawnRequests = new();
    private readonly Queue<Unit> _despawnRequests = new();
    public void RequestSpawn(UnitData data, Vector2 position)
    {
        _spawnRequests.Enqueue(new SpawnRequest
        {
            Data = data,
            Position = position
        });
    }
    public void RequestDespawn(Unit unit)
    {
        _despawnRequests.Enqueue(unit);
    }
    private void ProcessSpawn()
    {
        while (_spawnRequests.Count > 0)
        {
            SpawnRequest request = _spawnRequests.Dequeue();

            Unit unit = unitPool.Get();
            unit.Initialize(_nextId, request.Data, request.Position);
            unit.gameObject.SetActive(true);

            _units.Add(unit);

            Debug.Log($"[Unit Manager]Unit Spawned. ID : {_nextId}");
            _nextId++;
        }
    }
    private void ProcessDespawn()
    {
        while (_despawnRequests.Count > 0)
        {
            Unit unit = _despawnRequests.Dequeue();

            _units.Remove(unit);
        }
    }
    #endregion

    #region functions
    private int _thinkGroup;
    private const int ThinkGroupSize = 10;
    private void ThinkByGroup()
    {
        for (int i = _thinkGroup; i < _units.Count; i += ThinkGroupSize)
        {
            Unit unit = _units[i];
            Unit target = unit.TargetUnit;

            if (target == null ||
                target.Id != unit.TargetId ||
                !target.isActiveAndEnabled ||
                target.Hp <= 0)
            {
                using (FindTargetMarker.Auto())
                {
                    unit.SetTarget(_simulation.FindNearestTarget(unit));
                }
            }

            using (UnitThinkMarker.Auto())
            {
                unit.Think();
            }
        }
        _thinkGroup = (_thinkGroup + 1) % ThinkGroupSize;
    }
    #endregion

    #region debug
    [Header("Debug Settings")]
    [SerializeField] private UnitData testUnitData;
    [SerializeField] private float spawnDistance = 10f;
    [SerializeField] private float spawnInterval = 0.1f;

    [MMInspectorButton(nameof(SpawnEnemy))]
    [SerializeField] private bool testSpawnButton;
    [SerializeField] private UnitData testEnemyData;
    private IEnumerator TestSpawnLoop()
    {
        while (true)
        {
            float angle = UnityEngine.Random.Range(0f, 2f * Mathf.PI);
            Vector2 dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized;
            RequestSpawn(testUnitData, dir * spawnDistance);
            yield return new WaitForSeconds(spawnInterval);
        }
    }
    private void SpawnEnemy()
    {

        float angle = UnityEngine.Random.Range(0f, 2f * Mathf.PI);
        Vector2 dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized;
        for (int i = 0; i < 10; i++)
        {
            RequestSpawn(testEnemyData, dir * 40);
        }
    }
    #endregion
}