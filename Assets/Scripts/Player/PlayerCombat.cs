using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public static PlayerCombat Instance { get; private set; }

    [Header("Attack")]
    [SerializeField] private float blackAttackCooldown = 1.0f;
    [SerializeField] private float redAttackCooldown = 0.2f;
    [SerializeField] private float attackRadius = 1.5f;

    private float currentAttackCooldown;
    private float lastAttackTime = -999f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        currentAttackCooldown = blackAttackCooldown;
    }

    public bool IsAttackReady()
    {
        return Time.time >= lastAttackTime + currentAttackCooldown;
    }

    public bool TryAttack()
    {
        if (!GameManager.Instance.IsSessionRunning) return false;
        if (!IsAttackReady()) return false;

        lastAttackTime = Time.time;
        AttackResolutionSystem.Instance.ResolveAttack(transform.position, attackRadius);
        return true;
    }

    public void SetRedState(bool isRed)
    {
        currentAttackCooldown = isRed ? redAttackCooldown : blackAttackCooldown;
    }
}