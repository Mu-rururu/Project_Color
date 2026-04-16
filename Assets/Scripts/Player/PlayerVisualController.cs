using UnityEngine;

public class PlayerVisualController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite redSprite;

    private void Start()
    {
        // 시작할 때 기본 상태 적용
        ApplyState(PlayerStateController.Instance.CurrentState);

        // 상태 변화 구독
        PlayerStateController.Instance.OnStateChanged += ApplyState;
    }

    private void ApplyState(PlayerState state)
    {
        if (state == PlayerState.Red)
        {
            spriteRenderer.sprite = redSprite;
        }
        else
        {
            spriteRenderer.sprite = normalSprite;
        }
    }

    private void OnDestroy()
    {
        if (PlayerStateController.Instance != null)
        {
            PlayerStateController.Instance.OnStateChanged -= ApplyState;
        }
    }
}