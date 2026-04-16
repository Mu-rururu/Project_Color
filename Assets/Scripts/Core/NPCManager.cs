using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    public static NPCManager Instance { get; private set; }

    [SerializeField] private NPCController npcPrefab;
    [SerializeField] private NPCSpawner npcSpawner;
    [SerializeField] private int targetNpcCount;
    [SerializeField] private float spawnInterval;
    
    private float spawnTimer = 0f;
    private readonly List<NPCController> activeNpcs = new List<NPCController>();
    private readonly List<NPCController> pendingRemoveNpcs = new List<NPCController>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void Initialize()
    {
        ClearAllNpcs();

        activeNpcs.Clear();
        pendingRemoveNpcs.Clear();
        spawnTimer = 0f;
    }

    private void Update()
    {
        if (!GameManager.Instance.IsSessionRunning) return;

        ProcessPendingRemovals();
        HandleSpawn();
    }

    private void HandleSpawn()
    {
        if (activeNpcs.Count >= targetNpcCount)
        {
            spawnTimer = 0f;
            return;
        }

        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnInterval)
        {
            spawnTimer = 0f;
            SpawnNpc();
        }
    }

    public IReadOnlyList<NPCController> GetActiveNpcs()
    {
        return activeNpcs;
    }

    public void RequestRemove(NPCController npc)
    {
        if (npc == null) return;
        if (!pendingRemoveNpcs.Contains(npc))
        {
            pendingRemoveNpcs.Add(npc);
        }
    }

    private void ProcessPendingRemovals()
    {
        for (int i = 0; i < pendingRemoveNpcs.Count; i++)
        {
            NPCController npc = pendingRemoveNpcs[i];
            if (npc == null) continue;

            activeNpcs.Remove(npc);
            Destroy(npc.gameObject);
        }

        pendingRemoveNpcs.Clear();
    }

    private void RefillNpcCount()
    {
        while (activeNpcs.Count < targetNpcCount)
        {
            SpawnNpc();
        }
    }

    private void SpawnNpc()
    {
        bool success = npcSpawner.TryGetSpawnData(out Vector2 spawnPosition, out Vector2 moveDirection);
        if (!success) return;

        NPCController npc = Instantiate(npcPrefab, spawnPosition, Quaternion.identity);
        npc.Initialize(moveDirection);
        activeNpcs.Add(npc);
    }

    private void ClearAllNpcs()
    {
        NPCController[] existingNpcs = Object.FindObjectsByType<NPCController>(FindObjectsSortMode.None);
        for (int i = 0; i < existingNpcs.Length; i++)
        {
            Destroy(existingNpcs[i].gameObject);
        }
    }
}