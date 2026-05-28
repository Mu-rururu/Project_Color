using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Animator Controllers")]
    [SerializeField] private RuntimeAnimatorController blackController;
    [SerializeField] private RuntimeAnimatorController redController;

    private void Awake()
    {
        if (animator == null)
            animator = GetComponent<Animator>();

        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        PlayerStateController.Instance.OnStateChanged += OnPlayerStateChanged;
        OnPlayerStateChanged(PlayerStateController.Instance.CurrentState);
    }

    private void OnPlayerStateChanged(PlayerState state)
    {
        if (state == PlayerState.Red)
        {
            animator.runtimeAnimatorController = redController;
        }
        else
        {
            animator.runtimeAnimatorController = blackController;
        }
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

    private void OnDestroy()
    {
        if (PlayerStateController.Instance != null)
        {
            PlayerStateController.Instance.OnStateChanged -= OnPlayerStateChanged;
        }
    }
}