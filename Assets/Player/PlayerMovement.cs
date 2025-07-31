using System;
using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _moveDelay = 0.5f;

    private bool _isMoving;
    private Vector2 _currentDirection = Vector2.right;
    private WaitForSeconds _waitBetweenMove;
    
    public event Action<Vector2> OnMove;
    
    private void Start()
    {
        _waitBetweenMove = new WaitForSeconds(_moveDelay);
        StartCoroutine(nameof(AutoMove));
    }

    private void OnEnable()
    {
        InputManager.OnMoveDirectionChanged += ChangeDirection;
    }

    private void OnDisable()
    {
        InputManager.OnMoveDirectionChanged -= ChangeDirection;
    }

    private void ChangeDirection(Vector2 newDirection)
    {
        if (newDirection + _currentDirection == Vector2.zero)
        {
            return;
        }

        _currentDirection = newDirection;
    }

    private IEnumerator AutoMove()
    {
        _isMoving = true;

        while (_isMoving)
        {
            var nextPos = transform.position + (Vector3)(_currentDirection * LevelManager.GridSize);
            transform.position = nextPos;
            
            PathTracker.Add(nextPos);
            OnMove?.Invoke(_currentDirection);

            yield return _waitBetweenMove;
        }
    }
}