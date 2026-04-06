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
        Vector2 input = inputReader.ReadMoveInput();
        controller.SetMoveVector(input);

        if (inputReader.ConsumeAttackPressed())
        {
            combat.TryAttack();
        }
    }
}