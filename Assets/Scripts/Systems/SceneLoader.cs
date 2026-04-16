using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadTitleScene()
    {
        SceneManager.LoadScene("TitleScene");
    }

    public void LoadGameScene()
    {
        GameResultData.FinalScore = 0;
        SceneManager.LoadScene("GameScene");
    }

    public void LoadBlackEndingScene()
    {
        SceneManager.LoadScene("BlackEndingScene");
    }

    public void LoadRedEndingScene()
    {
        SceneManager.LoadScene("RedEndingScene");
    }
}