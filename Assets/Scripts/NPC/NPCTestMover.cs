using UnityEngine;

public class NPCTestMover : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;

    private Vector2 targetPosition;
    private bool isInitialized = false;

    public void Initialize(Vector2 targetPos)
    {
        targetPosition = targetPos;
        isInitialized = true;
    }

    private void Update()
    {
        if (!isInitialized) return;

        Vector2 currentPosition = transform.position;
        Vector2 nextPosition = Vector2.MoveTowards(
            currentPosition,
            targetPosition,
            moveSpeed * Time.deltaTime
        );

        transform.position = nextPosition;

        if (Vector2.Distance(currentPosition, targetPosition) <= 0.05f)
        {
            Destroy(gameObject);
        }
    }
}