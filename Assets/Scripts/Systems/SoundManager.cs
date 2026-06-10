using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    public float SFXVolume { get; private set; } = 1f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void SetSFXVolume(float volume)
    {
        SoundSettings.SFXVolume = volume;
    }
}