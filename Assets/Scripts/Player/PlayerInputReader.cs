using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputReader : MonoBehaviour
{
    private PlayerControls controls;
    private Vector2 moveInput;
    private bool attackPressedThisFrame;
    private bool inputEnabled = true;

    private void Awake()
    {
        controls = new PlayerControls();
    }

    private void OnEnable()
    {
        controls.Player.Enable();

        controls.Player.Move.performed += OnMovePerformed;
        controls.Player.Move.canceled += OnMoveCanceled;
        controls.Player.Attack.performed += OnAttackPerformed;
    }

    private void OnDisable()
    {
        controls.Player.Move.performed -= OnMovePerformed;
        controls.Player.Move.canceled -= OnMoveCanceled;
        controls.Player.Attack.performed -= OnAttackPerformed;

        controls.Player.Disable();
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        if (!inputEnabled)
        {
            moveInput = Vector2.zero;
            return;
        }

        moveInput = context.ReadValue<Vector2>();
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        moveInput = Vector2.zero;
    }

    private void OnAttackPerformed(InputAction.CallbackContext context)
    {
        if (!inputEnabled) return;
        attackPressedThisFrame = true;
    }

    public Vector2 ReadMoveInput()
    {
        return inputEnabled ? moveInput : Vector2.zero;
    }

    public bool ConsumeAttackPressed()
    {
        if (!inputEnabled) return false;

        bool result = attackPressedThisFrame;
        attackPressedThisFrame = false;
        return result;
    }

    public void SetInputEnabled(bool enabled)
    {
        inputEnabled = enabled;

        if (!enabled)
        {
            moveInput = Vector2.zero;
            attackPressedThisFrame = false;
        }
    }
}