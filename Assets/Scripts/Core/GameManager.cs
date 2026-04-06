using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public bool IsSessionRunning { get; private set; }
    public bool IsSessionEnded { get; private set; }

    [SerializeField] private NPCManager npcManager;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        StartSession();
    }

    public void StartSession()
    {
        IsSessionRunning = true;
        IsSessionEnded = false;

        npcManager.Initialize();
    }

    public void EndSession()
    {
        if (IsSessionEnded) return;

        IsSessionRunning = false;
        IsSessionEnded = true;
    }
}