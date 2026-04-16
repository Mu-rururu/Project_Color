using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public static PlayerCombat Instance { get; private set; }

    [SerializeField] private float attackCooldown = 1.0f;
    [SerializeField] private float attackRadius = 1.5f;

    private float lastAttackTime = -999f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public bool IsAttackReady()
    {
        return Time.time >= lastAttackTime + attackCooldown;
    }

    public bool TryAttack()
    {
        if (!GameManager.Instance.IsSessionRunning) return false;
        if (!IsAttackReady()) return false;

        lastAttackTime = Time.time;
        AttackResolutionSystem.Instance.ResolveAttack(transform.position, attackRadius);
        return true;
    }
}