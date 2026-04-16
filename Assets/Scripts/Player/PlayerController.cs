using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;

    private Rigidbody2D rb;
    private Vector2 pendingMoveVector;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void SetMoveVector(Vector2 moveVector)
    {
        if (moveVector.sqrMagnitude > 1f)
            moveVector = moveVector.normalized;

        pendingMoveVector = moveVector;
    }

    public void StopMove()
    {
        pendingMoveVector = Vector2.zero;
    }

    private void FixedUpdate()
    {
        if (!GameManager.Instance.IsSessionRunning)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        rb.linearVelocity = pendingMoveVector * moveSpeed;
    }
}
