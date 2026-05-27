using UnityEngine;

public class PlayerMovementHandler : MonoBehaviour
{
    private PlayerInputReader inputReader;
    private PlayerController controller;
    private PlayerCombat combat;
    private PlayerAnimationController animationController;

    private void Awake()
    {
        inputReader = GetComponent<PlayerInputReader>();
        controller = GetComponent<PlayerController>();
        combat = GetComponent<PlayerCombat>();
        animationController = GetComponent<PlayerAnimationController>();
    }

    private void Update()
    {
        if (!GameManager.Instance.IsSessionRunning)
        {
            controller.StopMove();
            animationController.UpdateMoveAnimation(Vector2.zero);
            return;
        }

        if (PlayerStateController.Instance.IsRedState())
        {
            inputReader.SetInputEnabled(false);

            Vector2 autoMove = AutoBehaviorController.Instance.CalculateAutoMove();
            controller.SetMoveVector(autoMove);
            animationController.UpdateMoveAnimation(autoMove);

            if (AutoBehaviorController.Instance.ShouldAutoAttack())
            {
                combat.TryAttack();
            }
        }
        else
        {
            inputReader.SetInputEnabled(true);

            Vector2 input = inputReader.ReadMoveInput();
            Vector2 finalMove = input;

            if (ViolenceUrgeController.Instance != null &&
                ViolenceUrgeController.Instance.IsActive())
            {
                finalMove = ViolenceUrgeController.Instance.ComposeMove(input);
            }

            controller.SetMoveVector(finalMove);
            animationController.UpdateMoveAnimation(finalMove);

            if (inputReader.ConsumeAttackPressed())
            {
                if (combat.TryAttack())
                {
                    animationController.PlayAttackAnimation();
                }
            }

            if (ViolenceUrgeController.Instance != null &&
                ViolenceUrgeController.Instance.ShouldAutoAttack())
            {
                combat.TryAttack();
            }
        }
    }
}