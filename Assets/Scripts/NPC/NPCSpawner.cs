using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    [SerializeField] private Transform player;

    [Header("Spawn Ring")]
    [SerializeField] private float minSpawnRadius;
    [SerializeField] private float maxSpawnRadius;

    [Header("Target Ring")]
    [SerializeField] private float minTargetRadius;
    [SerializeField] private float maxTargetRadius;

    public bool TryGetSpawnData(out Vector2 spawnPosition, out Vector2 moveDirection)
    {
        spawnPosition = Vector2.zero;
        moveDirection = Vector2.zero;

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
}