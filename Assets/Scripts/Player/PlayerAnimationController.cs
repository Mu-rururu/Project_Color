using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        if (animator == null)
            animator = GetComponent<Animator>();

        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void UpdateMoveAnimation(Vector2 move)
    {
        bool isMoving = move.sqrMagnitude > 0.01f;
        animator.SetBool("IsMoving", isMoving);

        if (!isMoving)
            return;

        bool isHorizontal = Mathf.Abs(move.x) > Mathf.Abs(move.y);
        animator.SetBool("IsHorizontal", isHorizontal);

        if (isHorizontal)
        {
            spriteRenderer.flipX = move.x < 0f;
        }
    }

    public void PlayAttackAnimation()
    {
        animator.SetTrigger("Attack");
    }
}