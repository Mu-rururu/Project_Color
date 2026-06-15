using UnityEngine;
using UnityEngine.UI;

public class OptionUIController : MonoBehaviour
{
    [SerializeField] private GameObject optionPanel;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;

    private void Start()
    {
        SyncSlidersWithSettings();
    }

    public void OpenOptionPanel()
    {
        SyncSlidersWithSettings();
        optionPanel.SetActive(true);
    }

    public void CloseOptionPanel()
    {
        optionPanel.SetActive(false);
    }

    private void SyncSlidersWithSettings()
    {
        if (bgmSlider != null)
            bgmSlider.value = SoundSettings.BGMVolume;

        if (sfxSlider != null)
            sfxSlider.value = SoundSettings.SFXVolume;
    }
}