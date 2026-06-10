using UnityEngine;

public class OptionUIController : MonoBehaviour
{
    [SerializeField] private GameObject optionPanel;

    public void OpenOptionPanel()
    {
        optionPanel.SetActive(true);
    }

    public void CloseOptionPanel()
    {
        optionPanel.SetActive(false);
    }
}