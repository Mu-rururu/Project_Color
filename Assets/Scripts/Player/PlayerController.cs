using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Move Speed")]
    [SerializeField] private float blackMoveSpeed = 4f;
    [SerializeField] private float redMoveSpeed = 8f;

    private float currentMoveSpeed;

    private Rigidbody2D rb;
    private Vector2 pendingMoveVector;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        currentMoveSpeed = blackMoveSpeed;
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

    public void SetRedState(bool isRed)
    {
        currentMoveSpeed = isRed ? redMoveSpeed : blackMoveSpeed;
    }

    private void FixedUpdate()
    {
        if (!GameManager.Instance.IsSessionRunning)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        rb.linearVelocity = pendingMoveVector * currentMoveSpeed;
    }
}