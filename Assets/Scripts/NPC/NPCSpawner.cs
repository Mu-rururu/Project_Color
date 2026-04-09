using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    [SerializeField] private Transform player;

    [Header("Spawn Ring")]
    [SerializeField] private float minSpawnRadius = 7f;
    [SerializeField] private float maxSpawnRadius = 9f;

    [Header("Target Ring")]
    [SerializeField] private float minTargetRadius = 4f;
    [SerializeField] private float maxTargetRadius = 5f;

    public bool TryGetSpawnData(out Vector2 spawnPosition, out Vector2 moveDirection)
    {
        spawnPosition = Vector2.zero;
        moveDirection = Vector2.zero;

        if (player == null)
        {
            Debug.LogError("NPCSpawner: player reference is missing.");
            return false;
        }

        Vector2 playerPos = player.position;

        spawnPosition = GetRandomPointInRing(playerPos, minSpawnRadius, maxSpawnRadius);
        Vector2 targetPosition = GetRandomPointInRing(playerPos, minTargetRadius, maxTargetRadius);

        moveDirection = (targetPosition - spawnPosition).normalized;

        return true;
    }

    private Vector2 GetRandomPointInRing(Vector2 center, float minRadius, float maxRadius)
    {
        float angle = Random.Range(0f, Mathf.PI * 2f);
        float distance = Random.Range(minRadius, maxRadius);

        Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        return center + direction * distance;
    }

    private void OnDrawGizmosSelected()
    {
        if (player == null) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(player.position, minSpawnRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(player.position, maxSpawnRadius);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(player.position, minTargetRadius);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(player.position, maxTargetRadius);
    }
}