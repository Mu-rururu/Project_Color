using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI timeText;

    [Header("Colors")]
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color redColor = Color.red;

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
        ScoreManager.Instance.OnScoreChanged += RefreshScore;
        TimeManager.Instance.OnTimeChanged += RefreshTime;

        // ⭐ 상태 변화 구독
        PlayerStateController.Instance.OnStateChanged += OnPlayerStateChanged;

        RefreshScore(ScoreManager.Instance.CurrentScore);
        RefreshTime(TimeManager.Instance.RemainingTime);

        // ⭐ 시작 시 색 초기화
        ApplyColor(PlayerStateController.Instance.CurrentState);
    }

    public void RefreshScore(int score)
    {
        scoreText.text = "Score : " + score;
    }

    public void RefreshTime(float remainingTime)
    {
        timeText.text = "Time : " + Mathf.CeilToInt(remainingTime);
    }

    private void OnPlayerStateChanged(PlayerState state)
    {
        ApplyColor(state);
    }

    private void ApplyColor(PlayerState state)
    {
        if (state == PlayerState.Red)
        {
            scoreText.color = redColor;
            timeText.color = redColor;
        }
        else
        {
            scoreText.color = normalColor;
            timeText.color = normalColor;
        }
    }

    private void OnDestroy()
    {
        if (ScoreManager.Instance != null)
            ScoreManager.Instance.OnScoreChanged -= RefreshScore;

        if (TimeManager.Instance != null)
            TimeManager.Instance.OnTimeChanged -= RefreshTime;

        if (PlayerStateController.Instance != null)
            PlayerStateController.Instance.OnStateChanged -= OnPlayerStateChanged;
    }
}