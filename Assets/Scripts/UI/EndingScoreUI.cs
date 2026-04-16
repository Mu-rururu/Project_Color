using UnityEngine;
using TMPro;

public class EndingScoreUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;

    private void Start()
    {
        scoreText.text = "Score : " + GameResultData.FinalScore;
    }
}