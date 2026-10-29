using System.Collections.Generic;
using UnityEngine;

public class UnitPool : MonoBehaviour
{
    private const int Initial_Pool_Count = 100;

    [SerializeField] private Unit unitPrefab;
    private readonly Queue<Unit> _pool = new();

    private void Awake()
    {
        InitializePool();
    }

    private void InitializePool()
    {
        for (int i = 0; i < Initial_Pool_Count; i++)
        {
            Unit unit = Instantiate(unitPrefab, Vector2.zero, Quaternion.identity, transform);
            unit.gameObject.SetActive(false);
            _pool.Enqueue(unit);
        }
    }
    public Unit Get()
    {
        return _pool.Count > 0
        ? _pool.Dequeue()
        : Instantiate(unitPrefab, Vector2.zero, Quaternion.identity, transform);
    }
    public void Release(Unit unit)
    {
        unit.gameObject.SetActive(false);
        _pool.Enqueue(unit);
    }
}