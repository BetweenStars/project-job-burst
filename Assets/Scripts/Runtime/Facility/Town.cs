using UnityEngine;
using UnityEngine.Events;

public class Town : MonoBehaviour, IDamageable
{
    public static Town Instance { get; private set; }

    [SerializeField] private int maxHp = 3000;
    [SerializeField] private int hp;
    [SerializeField] private int money;

    [Tooltip("(Hp,MaxHp)")] public UnityEvent<int, int> HpChanged;
    public UnityEvent<int> MoneyChanged;
    public UnityEvent TownDestroyed;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        Initialize();
    }

    private void Initialize()
    {
        hp = maxHp;
        HpChanged?.Invoke(hp, maxHp);
        MoneyChanged?.Invoke(money);
    }

    public void TakeDamage(int damage)
    {
        hp = Mathf.Clamp(hp - damage, 0, maxHp);

        if (hp <= 0)
        {
            Die();
        }

        HpChanged?.Invoke(hp, maxHp);
    }

    public void AddMoney(int value)
    {
        money += value;
        MoneyChanged?.Invoke(money);
    }

    public bool TrySpendMoney(int value)
    {
        int spended = money - value;

        if (spended < 0)
        {
            Debug.Log("[Town]Not Enough Money");
            return false;
        }

        money = Mathf.Clamp(spended, 0, int.MaxValue);
        MoneyChanged?.Invoke(money);
        return true;
    }

    private void Die()
    {
        TownDestroyed?.Invoke();
        Destroy(gameObject);
    }
}