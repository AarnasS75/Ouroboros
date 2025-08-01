using System;
using Static_Events;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static event Action<Vector2> OnMoveDirectionChanged;
    public static event Action OnInteract;

    private PlayerControls _playerControls;

    private void Awake()
    {
        _playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        _playerControls.Enable();
        _playerControls.Player.Move.performed += OnDirectionChanged;
        _playerControls.Player.Interact.started += OnPressedInteract;
    }

    private void OnDisable()
    {
        _playerControls.Disable();
        _playerControls.Player.Move.performed -= OnDirectionChanged;
        _playerControls.Player.Interact.started -= OnPressedInteract;
    }

    private void OnPressedInteract(InputAction.CallbackContext ctx)
    {
        OnInteract?.Invoke();
    }

    private void OnDirectionChanged(InputAction.CallbackContext ctx)
    {
        var direction = ctx.ReadValue<Vector2>();
        OnMoveDirectionChanged?.Invoke(direction);
    }
}