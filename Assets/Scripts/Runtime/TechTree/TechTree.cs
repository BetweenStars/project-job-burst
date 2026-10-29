using System;
using System.Collections.Generic;
using UnityEngine;

public enum NodeType
{
    None = -1,
    UnlockUnit = 0,
    IncreaseAtk = 1,
    IncreaseMaxHp = 2
}

[Serializable]
public class TechEffect
{
    [SerializeField] private NodeType type;
    [SerializeField] private string targetId;
    [SerializeField] private int value;

    public NodeType Type => type;
    public string TargetId => targetId;
    public int Value => value;
}

[Serializable]
[CreateAssetMenu(fileName = "TechNode", menuName = "TechTree/Node")]
public class TechNode : ScriptableObject
{
    [SerializeField] private string id;
    [SerializeField] private string displayName;
    [SerializeField] private List<TechEffect> effects;
    [SerializeField] private int cost;
    [SerializeField] private string description;
    [SerializeField] private TechNode parent;

    public string Id => id;
    public string DisplayName => displayName;
    public string Description => description;
    public int Cost => cost;
}

public class TechTree : MonoBehaviour
{
    [SerializeField] private List<string> activeNodeIds = new();

    public void ActiveNode(TechNode node) { }
}