using UnityEngine;

public class PlayerSoundController : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource walkSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Clips")]
    [SerializeField] private AudioClip walkingClip;
    [SerializeField] private AudioClip attackClip;

    public void UpdateWalkSound(Vector2 move)
    {
        bool isMoving = move.sqrMagnitude > 0.01f;

        if (isMoving)
        {
            if (!walkSource.isPlaying)
            {
                walkSource.clip = walkingClip;
                walkSource.loop = true;
                walkSource.Play();
            }
        }
        else
        {
            if (walkSource.isPlaying)
            {
                walkSource.Stop();
            }
        }
    }

    public void PlayAttackSound()
    {
        if (attackClip == null) return;

        sfxSource.PlayOneShot(attackClip);
    }
}