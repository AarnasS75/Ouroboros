using System;
using UnityEngine;
using System.Collections;
using Static_Events;

public class PlayerMovement : MonoBehaviour
{
    [Header("Sprites")]
    [SerializeField] private SpriteRenderer _sprite;
    
    [Header("Movement")]
    [SerializeField] private float _moveDelay = 0.5f;
    [SerializeField] private LayerMask _wallLayer;

    private Transform _spriteTransform;
    private bool _isMoving;
    private Vector2 _currentDirection = Vector2.right;
    private WaitForSeconds _waitBetweenMove;
    private float _currentDelay;
    
    public event Action<Vector2> OnMove;

    private void Awake()
    {
        _spriteTransform = _sprite.transform;
    }

    private void OnEnable()
    {
        StartCoroutine(nameof(FadeIn));
        
        InputManager.OnMoveDirectionChanged += ChangeDirection;
        StaticEventHandler.OnFoodConsumed += OnFoodConsumed;
    }

    private void OnDisable()
    {
        PathTracker.Reset();
        InputManager.OnMoveDirectionChanged -= ChangeDirection;
        StaticEventHandler.OnFoodConsumed -= OnFoodConsumed;
    }

    private void OnFoodConsumed(Food obj)
    {
        _currentDelay -= 0.015f;
        _waitBetweenMove = new WaitForSeconds(_currentDelay);
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
            _spriteTransform.rotation = Quaternion.Euler(0f, 0f, 0f);
            _spriteTransform.localScale = Vector3.one;
        }
        else if (dir == Vector2.left)
        {
            _spriteTransform.rotation = Quaternion.Euler(0f, 0f, 0f);
            _spriteTransform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if (dir == Vector2.up)
        {
            _spriteTransform.rotation = Quaternion.Euler(0f, 0f, 90f);
            _spriteTransform.localScale = Vector3.one;
        }
        else if (dir == Vector2.down)
        {
            _spriteTransform.rotation = Quaternion.Euler(0f, 0f, -90f);
            _spriteTransform.localScale = Vector3.one;
        }
    }
    
    const float duration = 2f;
    private IEnumerator FadeIn()
    {
        var elapsedTime = 0f;
        
        var c = _sprite.color;
        _sprite.color = new Color(c.r, c.g, c.b, 0f);

        while (elapsedTime < duration)
        {
            var alpha = elapsedTime / duration;
            _sprite.color = new Color(c.r, c.g, c.b, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        _sprite.color = new Color(c.r, c.g, c.b, 1f);
        
        _currentDelay = _moveDelay;
        _waitBetweenMove = new WaitForSeconds(_currentDelay);
        StartCoroutine(nameof(AutoMove));
    }
}