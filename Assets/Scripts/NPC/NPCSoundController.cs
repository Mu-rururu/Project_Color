using UnityEngine;

public class NPCSoundController : MonoBehaviour
{
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioClip deathClip;

    private void Awake()
    {
        if (sfxSource == null)
            sfxSource = GetComponent<AudioSource>();
    }

    public void PlayDeathSound()
    {
        if (deathClip == null) return;

        sfxSource.PlayOneShot(deathClip, SoundManager.Instance.SFXVolume);
    }
}