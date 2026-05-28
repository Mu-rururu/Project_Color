using UnityEngine;

public class NPCAnimationController : MonoBehaviour
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

    public void UpdateMoveAnimation(Vector2 moveDirection)
    {
        bool isHorizontal = Mathf.Abs(moveDirection.x) > Mathf.Abs(moveDirection.y);

        animator.SetBool("IsHorizontal", isHorizontal);

        if (isHorizontal)
        {
            spriteRenderer.flipX = moveDirection.x < 0f;
        }
    }

    public void PlayDeathAnimation()
    {
        animator.SetTrigger("Die");
    }
}