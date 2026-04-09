using UnityEngine;

public class AttackResolutionSystem : MonoBehaviour
{
    public static AttackResolutionSystem Instance { get; private set; }

    [SerializeField] private LayerMask npcLayer;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public int ResolveAttack(Vector2 origin, float radius)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(origin, radius, npcLayer);

        int hitCount = 0;

        foreach (var hit in hits)
        {
            NPCController npc = hit.GetComponent<NPCController>();
            if (npc == null) continue;
            if (npc.IsPendingRemove) continue;

            npc.OnHit();
            hitCount++;
        }

        if (hitCount > 0)
        {
            ScoreManager.Instance.AddScore(hitCount);
        }

        return hitCount;
    }
}