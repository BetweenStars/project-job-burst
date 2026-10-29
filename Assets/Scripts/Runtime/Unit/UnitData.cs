using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum Team
{
    None = -1,
    Ally = 0,
    Enemy = 1,
    Neutral = 2
}

public enum AttackType
{
    None = -1,
    Melee = 0,
    Ranged = 1
}

[Serializable]
public class UnitVisualData
{
    [SerializeField] private Sprite idleSprite;
    [SerializeField] private Sprite startAttackSprite;
    [SerializeField] private Sprite attackSprite;
    [SerializeField] private Sprite knockbackSprite;

    public Sprite IdleSprite => idleSprite;
    public Sprite StartAttackSprite => startAttackSprite;
    public Sprite AttackSprite => attackSprite;
    public Sprite KnockbackSprite => knockbackSprite;
}

[Serializable]
public class UnitAttackData
{
    [SerializeField] private AttackType type;
}

[CreateAssetMenu(fileName = "UnitData", menuName = "Unit/UnitData", order = 0)]
public class UnitData : ScriptableObject
{
    [Header("Unit Info")]
    [SerializeField] private string unitId;
    [SerializeField] private string displayName;
    [SerializeField] private Team team;
    [SerializeField] private AttackType attackType;

    [Header("Unit Stats")]
    [SerializeField] private int baseHp;
    [SerializeField] private float moveSpeed;
    [SerializeField] private int atkPower;
    [SerializeField] private int defPower;
    [SerializeField] private List<UnitAttackData> atkDatas;
    [SerializeField] private float atkRange;
    [SerializeField] private float atkSpeed;
    [SerializeField, Range(0f, 1f)] private float atkTiming;

    [Header("Target Settings")]
    [SerializeField] private float targetRange;

    [Header("Simulation Settings")]
    [SerializeField] private float radius = 1f;

    [Header("Visual Settings")]
    [SerializeField] private UnitVisualData visuals;

    public string UnitId => unitId;
    public string DisplayName => displayName;
    public Team Team => team;
    public int BaseHp => baseHp;
    public float MoveSpeed => moveSpeed;
    public int AtkPower => atkPower;
    public int DefPower => defPower;
    public List<UnitAttackData> AtkDatas => atkDatas;
    public float AtkRange => atkRange;
    public float AtkSpeed => atkSpeed;
    public float AtkTiming => atkTiming;
    public AttackType AttackType => attackType;

    public float Radius => radius;

    public float TargetRange => targetRange;

    public UnitVisualData Visuals => visuals;
}
