using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    //info
    public int Id = -1;
    [SerializeField] private UnitData data;
    public UnitData Data => data;

    //runtime states
    [SerializeField] private int hp;
    [SerializeField] private float radius;
    [SerializeField] private float weight;
    [SerializeField] private float targetRange;
    [SerializeField] private Vector2 velocity;

    public int Hp => hp;
    public float Radius => radius;
    public float Weight => weight;
    public float TargetRange => targetRange;
    public Vector2 Velocity => velocity;

    //bool
    public bool IsAttacking { get; private set; }

    //target
    private Unit _targetUnit;
    private int _targetId;

    public Unit TargetUnit => _targetUnit;
    public int TargetId => _targetId;

    #region initialize & setter
    public void Initialize(int id, UnitData data, Vector2 position)
    {
        Id = id;
        this.data = data;

        hp = data.BaseHp;
        radius = data.Radius;
        //weight
        targetRange = data.TargetRange;
        velocity = Vector2.zero;

        IsAttacking = false;

        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = data.Visuals?.IdleSprite;
        }

        SetPosition(position);
    }
    public void ChangeHp(int value)
    {
        hp += value;
    }
    public void SetId(int id)
    {
        Id = id;
    }
    public void SetSprite(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
    }
    public void SetPosition(Vector2 position)
    {
        transform.position = position;
    }
    public void SetTarget(Unit target)
    {
        if (target == null)
        {
            _targetUnit = null;
            _targetId = -1;
            return;
        }

        _targetUnit = target;
        _targetId = target.Id;
    }
    #endregion

    #region ai
    public void Think()
    {
        if (IsAttacking) return;

        switch (Data.Team)
        {
            case Team.Ally: DecideAlly(); break;
            case Team.Enemy: DecideEnemy(); break;
            default: velocity = Vector2.zero; break;
        }
    }
    private void DecideAlly()
    {
        if (TargetUnit == null || TargetId == -1)
        {
            float distanceToTown = Vector2.SqrMagnitude(transform.position);
            velocity = Vector2.zero;
            if (distanceToTown > 12 * 12)
                velocity = -transform.position.normalized * Data.MoveSpeed;
            return;
        }

        float distance = Vector2.Distance(TargetUnit.transform.position, transform.position);

        if (distance < TargetUnit.Radius + Radius + Data.AtkRange)
        {
            Attack();
        }
        else
        {
            Vector2 dir = (TargetUnit.transform.position - transform.position).normalized;
            velocity = dir * Data.MoveSpeed;
        }
    }
    private void DecideEnemy()
    {
        velocity = (-transform.position).normalized * Data.MoveSpeed;
    }
    #endregion

    #region attack
    public void Attack()
    {
        AttackAsync().Forget();
    }
    private async UniTask AttackAsync()
    {
        IsAttacking = true;

        SetSprite(Data.Visuals.StartAttackSprite);

        await UniTask.Delay(500);
        SetSprite(Data.Visuals.AttackSprite);

        await UniTask.Delay(200);
        SetSprite(Data.Visuals.IdleSprite);

        IsAttacking = false;
    }
    #endregion
}
