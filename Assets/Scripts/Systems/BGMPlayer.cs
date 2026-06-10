using UnityEngine;

public class BGMPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip bgmClip;

    private void Awake()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        if (audioSource == null) return;
        if (bgmClip == null) return;

        audioSource.clip = bgmClip;
        audioSource.loop = true;
        audioSource.playOnAwake = false;
        audioSource.volume = SoundSettings.BGMVolume;
        audioSource.Play();
    }

    public void SetVolume(float volume)
    {
        SoundSettings.BGMVolume = volume;

        if (audioSource != null)
        {
            audioSource.volume = volume;
        }
    }
}