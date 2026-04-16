using UnityEngine;

public class NPCController : MonoBehaviour
{
    public bool IsPendingRemove { get; private set; }

    [SerializeField] private float moveSpeed;
    [SerializeField] private float despawnDistance;

    private Vector2 moveDirection;
    private Vector2 spawnPosition;
    private Rigidbody2D rb;
    private bool isInitialized;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Initialize(Vector2 direction)
    {
        IsPendingRemove = false;
        moveDirection = direction.normalized;
        spawnPosition = transform.position;
        isInitialized = true;
    }

    public void FixedUpdate()
    {
        if (IsPendingRemove) return;
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

        IsPendingRemove = true;
        NPCManager.Instance.RequestRemove(this);
    }
}