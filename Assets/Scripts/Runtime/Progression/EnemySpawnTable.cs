using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class EnemySpawnEntry
{
    public UnitData EnemyData;
    public int Weight;
}

[CreateAssetMenu(fileName = "SpawnTable", menuName = "SpawnTable")]
public class EnemySpawnTable : ScriptableObject
{
    public int SpawnInterval;
    public int SpawnCount;
    public int MaxAlive;
    public List<EnemySpawnEntry> Entries;
}
