using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public bool IsSessionRunning { get; private set; }
    public bool IsSessionEnded { get; private set; }

    [SerializeField] private NPCManager npcManager;
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private TimeManager timeManager;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private PlayerStateController playerStateController;

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

    private void Update()
    {
        if (!IsSessionRunning) return;

        timeManager.Tick(Time.deltaTime);

        if (timeManager.IsTimeOver())
        {
            EndSession();
        }
    }

    public void StartSession()
    {
        IsSessionRunning = true;
        IsSessionEnded = false;

        scoreManager.Initialize();
        timeManager.Initialize();
        npcManager.Initialize();
        uiManager.Initialize();
        playerStateController.Initialize();
    }

    public void EndSession()
    {
        if (IsSessionEnded) return;

        IsSessionRunning = false;
        IsSessionEnded = true;

        int finalScore = scoreManager.CurrentScore;
        GameResultData.FinalScore = finalScore;

        if (GameProgressData.HasSeenRedEnding && finalScore == 0)
        {
            SceneManager.LoadScene("WhiteEndingScene");
        }
        else if (finalScore >= 30)
        {
            GameProgressData.MarkRedEndingSeen();
            SceneManager.LoadScene("RedEndingScene");
        }
        else
        {
            SceneManager.LoadScene("BlackEndingScene");
        }
    }
}