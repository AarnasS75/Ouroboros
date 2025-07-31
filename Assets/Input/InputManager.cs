using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static event Action<Vector2> OnMoveDirectionChanged;

    private void Update()
    {
        var direction = Vector2.zero;

        if (Input.GetKeyDown(KeyCode.W)) direction = Vector2.up;
        else if (Input.GetKeyDown(KeyCode.S)) direction = Vector2.down;
        else if (Input.GetKeyDown(KeyCode.A)) direction = Vector2.left;
        else if (Input.GetKeyDown(KeyCode.D)) direction = Vector2.right;

        if (direction != Vector2.zero)
        {
            OnMoveDirectionChanged?.Invoke(direction);
        }
    }
}