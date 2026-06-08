using UnityEngine;

public class NPCController : MonoBehaviour
{
    public bool IsPendingRemove { get; private set; }

    [SerializeField] private float moveSpeed = 2.0f;
    [SerializeField] private float despawnDistance = 18f;
    [SerializeField] private float deathAnimationDuration = 0.5f;
    
    [Header("Stuck Check")]
    [SerializeField] private float stuckSpeedThreshold = 0.1f;
    [SerializeField] private float stuckRemoveDelay = 0.8f;

    private Vector2 lastPosition;
    private float stuckTimer;

    private Vector2 moveDirection;
    private Vector2 spawnPosition;
    private Rigidbody2D rb;
    private bool isInitialized;
    private bool isDead;
    private NPCSoundController soundController;

    private NPCAnimationController animationController;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animationController = GetComponent<NPCAnimationController>();
        soundController = GetComponent<NPCSoundController>();
    }

    public void Initialize(Vector2 direction)
    {
        IsPendingRemove = false;
        isDead = false;

        lastPosition = transform.position;
        stuckTimer = 0f;

        moveDirection = direction.normalized;
        spawnPosition = transform.position;
        isInitialized = true;

        if (animationController != null)
        {
            animationController.UpdateMoveAnimation(moveDirection);
        }
    }

    private void FixedUpdate()
    {
        if (IsPendingRemove) return;
        if (isDead) return;
        if (!isInitialized) return;
        if (!GameManager.Instance.IsSessionRunning) return;
        if (rb == null) return;

        Vector2 nextPosition = rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(nextPosition);

        CheckStuck();

        if (Vector2.Distance(spawnPosition, rb.position) >= despawnDistance)
        {
            NPCManager.Instance.RequestRemove(this);
        }
    }

    private void CheckStuck()
    {
        float movedDistance = Vector2.Distance(rb.position, lastPosition);
        float actualSpeed = movedDistance / Time.fixedDeltaTime;

        if (actualSpeed <= stuckSpeedThreshold)
        {
            stuckTimer += Time.fixedDeltaTime;

            if (stuckTimer >= stuckRemoveDelay)
            {
                NPCManager.Instance.RequestRemove(this);
            }
        }
        else
        {
            stuckTimer = 0f;
        }

        lastPosition = rb.position;
    }

    public void OnHit()
    {
        if (IsPendingRemove) return;
        if (isDead) return;

        StartDeath();
    }

    private void StartDeath()
    {
        isDead = true;
        IsPendingRemove = true;

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }

        if (animationController != null)
        {
            animationController.PlayDeathAnimation();
        }

        if (soundController != null)
        {
            soundController.PlayDeathSound();
        }

        Invoke(nameof(RequestRemoveAfterDeath), deathAnimationDuration);
    }

    private void RequestRemoveAfterDeath()
    {
        NPCManager.Instance.RequestRemove(this);
    }
}