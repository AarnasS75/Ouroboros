using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static event Action<Vector2> OnMoveDirectionChanged;

    private PlayerControls _playerControls;

    private void Awake()
    {
        _playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        _playerControls.Enable();
        _playerControls.Player.Move.performed += OnDirectionChanged;
    }

    private void OnDisable()
    {
        _playerControls.Disable();
        _playerControls.Player.Move.performed -= OnDirectionChanged;
    }

    private void OnDirectionChanged(InputAction.CallbackContext ctx)
    {
        var direction = ctx.ReadValue<Vector2>();
        OnMoveDirectionChanged?.Invoke(direction);
    }
}