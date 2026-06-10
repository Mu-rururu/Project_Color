using UnityEngine;

public class ExitButtonHandler : MonoBehaviour
{
    public void ExitGame()
    {
        Debug.Log("Exit Game");

        Application.Quit();
    }
}