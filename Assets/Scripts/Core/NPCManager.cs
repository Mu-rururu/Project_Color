using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    public static NPCManager Instance { get; private set; }

    [SerializeField] private NPCController npcPrefab;
    [SerializeField] private int targetNpcCount = 5;

    private List<NPCController> activeNpcs = new List<NPCController>();
    private List<NPCController> pendingRemoveNpcs = new List<NPCController>();

    private void Awake()
    {
        Instance = this;
    }

    public void Initialize()
    {
        activeNpcs.Clear();
        pendingRemoveNpcs.Clear();

        for (int i = 0; i < targetNpcCount; i++)
        {
            SpawnNpc();
        }
    }

    private void Update()
    {
        if (!GameManager.Instance.IsSessionRunning) return;

        TickAll();
        ProcessPendingRemovals();
        RefillNpcCount();
    }

    public void TickAll()
    {
        for (int i = 0; i < activeNpcs.Count; i++)
        {
            if (activeNpcs[i] != null)
            {
                activeNpcs[i].Tick();
            }
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
        Vector2 spawnPos = GetRandomSpawnPosition();
        NPCController npc = Instantiate(npcPrefab, spawnPos, Quaternion.identity);
        npc.Initialize();
        activeNpcs.Add(npc);
    }

    private Vector2 GetRandomSpawnPosition()
    {
        float x = Random.Range(-6f, 6f);
        float y = Random.Range(-3f, 3f);
        return new Vector2(x, y);
    }
}