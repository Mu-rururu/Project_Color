using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private float attackCooldown = 1.0f;
    [SerializeField] private float attackRadius = 1.5f;

    private float lastAttackTime = -999f;

    public bool IsAttackReady()
    {
        return Time.time >= lastAttackTime + attackCooldown;
    }

    public bool TryAttack()
    {
        if (!GameManager.Instance.IsSessionRunning) return false;
        if (!IsAttackReady()) return false;

        lastAttackTime = Time.time;

        AttackResolutionSystem.Instance.ResolveAttack(
            transform.position,
            attackRadius
        );

        return true;
    }
}