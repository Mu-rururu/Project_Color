using UnityEngine;

public class AutoBehaviorController : MonoBehaviour
{
    public static AutoBehaviorController Instance { get; private set; }

    [SerializeField] private Transform player;
    [SerializeField] private float autoAttackRange = 1.5f;

    private NPCController currentTarget;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public Vector2 CalculateAutoMove()
    {
        currentTarget = FindNearestNpc();

        if (currentTarget == null)
            return Vector2.zero;

        Vector2 dir = ((Vector2)currentTarget.transform.position - (Vector2)player.position).normalized;
        return dir;
    }

    public bool ShouldAutoAttack()
    {
        if (!GameManager.Instance.IsSessionRunning) return false;
        if (!PlayerStateController.Instance.IsRedState()) return false;

        currentTarget = FindNearestNpc();

        if (currentTarget == null) return false;
        if (!PlayerCombat.Instance.IsAttackReady()) return false;

        return Vector2.Distance(player.position, currentTarget.transform.position) <= autoAttackRange;
    }

    public NPCController FindNearestNpc()
    {
        var npcs = NPCManager.Instance.GetActiveNpcs();
        NPCController nearest = null;
        float minDist = float.MaxValue;

        for (int i = 0; i < npcs.Count; i++)
        {
            NPCController npc = npcs[i];
            if (npc == null) continue;
            if (npc.IsPendingRemove) continue;

            float dist = Vector2.Distance(player.position, npc.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = npc;
            }
        }

        return nearest;
    }
}