using UnityEngine;

public class ViolenceUrgeController : MonoBehaviour
{
    public static ViolenceUrgeController Instance { get; private set; }

    [SerializeField] private Transform player;
    [SerializeField] private float urgeStrength = 0.35f;
    [SerializeField] private float autoAttackRange = 1.5f;
    [SerializeField] private bool enableAutoAttack = true;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public bool IsActive()
    {
        return GameProgressData.HasSeenRedEnding
            && GameManager.Instance.IsSessionRunning
            && PlayerStateController.Instance.CurrentState == PlayerState.Black;
    }

    public Vector2 ComposeMove(Vector2 inputMove)
    {
        if (!IsActive()) return inputMove;

        NPCController nearest = FindNearestNpc();
        if (nearest == null) return inputMove;

        Vector2 urge = ((Vector2)nearest.transform.position - (Vector2)player.position).normalized;
        Vector2 raw = inputMove + urge * urgeStrength;

        return raw.sqrMagnitude > 1f ? raw.normalized : raw;
    }

    public bool ShouldAutoAttack()
    {
        if (!IsActive()) return false;
        if (!enableAutoAttack) return false;

        NPCController nearest = FindNearestNpc();
        if (nearest == null) return false;
        if (!PlayerCombat.Instance.IsAttackReady()) return false;

        return Vector2.Distance(player.position, nearest.transform.position) <= autoAttackRange;
    }

    public NPCController FindNearestNpc()
    {
        var npcs = NPCManager.Instance.GetActiveNpcs();

        NPCController nearest = null;
        float minDistance = float.MaxValue;

        for (int i = 0; i < npcs.Count; i++)
        {
            NPCController npc = npcs[i];

            if (npc == null) continue;
            if (npc.IsPendingRemove) continue;

            float distance = Vector2.Distance(player.position, npc.transform.position);

            if (distance < minDistance)
            {
                minDistance = distance;
                nearest = npc;
            }
        }

        return nearest;
    }
}