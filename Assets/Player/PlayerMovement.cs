using System;
using UnityEngine;
using System.Collections;
using Static_Events;

public class PlayerMovement : MonoBehaviour
{
    [Header("Sprites")]
    [SerializeField] private Transform _sprite;
    
    [Header("Movement")]
    [SerializeField] private float _moveDelay = 0.5f;
    [SerializeField] private LayerMask _wallLayer;

    private bool _isMoving;
    private Vector2 _currentDirection = Vector2.right;
    private WaitForSeconds _waitBetweenMove;
    
    public event Action<Vector2> OnMove;
    
    private void Start()
    {
        _waitBetweenMove = new WaitForSeconds(_moveDelay);
    }

    private void OnEnable()
    {
        StartCoroutine(nameof(AutoMove));
        InputManager.OnMoveDirectionChanged += ChangeDirection;
    }

    private void OnDisable()
    {
        PathTracker.Reset();
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
            RotateSprite(_currentDirection);
            transform.position = nextPos;
            
            PathTracker.Add(nextPos);
            OnMove?.Invoke(_currentDirection);

            yield return _waitBetweenMove;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (((1 << other.gameObject.layer) & _wallLayer) == 0)
        {
            return;
        }
        
        StopAllCoroutines();
        StaticEventHandler.CallGameFinishedEvent(new GameOverEventArgs
        {
            IsPlayerDead = true
        });
    }
    
    private void RotateSprite(Vector2 dir)
    {
        if (dir == Vector2.right)
        {
            _sprite.rotation = Quaternion.Euler(0f, 0f, 0f);
            _sprite.localScale = Vector3.one;
        }
        else if (dir == Vector2.left)
        {
            _sprite.rotation = Quaternion.Euler(0f, 0f, 0f);
            _sprite.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if (dir == Vector2.up)
        {
            _sprite.rotation = Quaternion.Euler(0f, 0f, 90f);
            _sprite.localScale = Vector3.one;
        }
        else if (dir == Vector2.down)
        {
            _sprite.rotation = Quaternion.Euler(0f, 0f, -90f);
            _sprite.localScale = Vector3.one;
        }
    }
}