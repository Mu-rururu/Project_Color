using UnityEngine;

public class PlayerMovementHandler : MonoBehaviour
{
    private PlayerInputReader inputReader;
    private PlayerController controller;
    private PlayerCombat combat;

    private void Awake()
    {
        inputReader = GetComponent<PlayerInputReader>();
        controller = GetComponent<PlayerController>();
        combat = GetComponent<PlayerCombat>();
    }

    private void Update()
    {
        if (!GameManager.Instance.IsSessionRunning)
        {
            controller.StopMove();
            return;
        }

        if (PlayerStateController.Instance.IsRedState())
        {
            inputReader.SetInputEnabled(false);

            Vector2 autoMove = AutoBehaviorController.Instance.CalculateAutoMove();
            controller.SetMoveVector(autoMove);

            if (AutoBehaviorController.Instance.ShouldAutoAttack())
            {
                combat.TryAttack();
            }
        }
        else
        {
            inputReader.SetInputEnabled(true);

            Vector2 input = inputReader.ReadMoveInput();
            controller.SetMoveVector(input);

            if (inputReader.ConsumeAttackPressed())
            {
                combat.TryAttack();
            }
        }
    }
}