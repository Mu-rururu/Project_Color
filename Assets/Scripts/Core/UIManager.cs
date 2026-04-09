using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI timeText;

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

        RefreshScore(ScoreManager.Instance.CurrentScore);
        RefreshTime(TimeManager.Instance.RemainingTime);
    }

    public void RefreshScore(int score)
    {
        scoreText.text = "Score : " + score;
    }

    public void RefreshTime(float remainingTime)
    {
        timeText.text = "Time : " + Mathf.CeilToInt(remainingTime);
    }

    private void OnDestroy()
    {
        if (ScoreManager.Instance != null)
            ScoreManager.Instance.OnScoreChanged -= RefreshScore;

        if (TimeManager.Instance != null)
            TimeManager.Instance.OnTimeChanged -= RefreshTime;
    }
}