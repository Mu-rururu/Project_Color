using UnityEngine;

public class NPCController : MonoBehaviour
{
    public bool IsPendingRemove { get; private set; }

    [SerializeField] private float moveSpeed = 2.0f;
    [SerializeField] private float despawnDistance = 18f;
    [SerializeField] private float deathAnimationDuration = 0.5f;

    private Vector2 moveDirection;
    private Vector2 spawnPosition;
    private Rigidbody2D rb;
    private bool isInitialized;
    private bool isDead;

    private NPCAnimationController animationController;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animationController = GetComponent<NPCAnimationController>();
    }

    public void Initialize(Vector2 direction)
    {
        IsPendingRemove = false;
        isDead = false;

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

        if (Vector2.Distance(spawnPosition, rb.position) >= despawnDistance)
        {
            NPCManager.Instance.RequestRemove(this);
        }
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

        Invoke(nameof(RequestRemoveAfterDeath), deathAnimationDuration);
    }

    private void RequestRemoveAfterDeath()
    {
        NPCManager.Instance.RequestRemove(this);
    }
}