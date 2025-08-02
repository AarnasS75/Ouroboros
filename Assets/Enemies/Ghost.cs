using System.Collections;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _sprite;
    
    private PlayerMovement _player;
    private int _currentPathIndex = 0;
    private Transform _spriteTransform;
    private bool _isReady;
    
    private void Awake()
    {
        _spriteTransform = _sprite.transform;
    }

    public void Initialize(PlayerMovement player)
    {
        _player = player;
        _player.OnMove += FollowMovement;
    }

    private void OnEnable()
    {
        StartCoroutine(nameof(FadeIn));
    }

    private void OnDisable()
    {
        if (!_player)
        {
            return;
        }

        _isReady = false;
        _currentPathIndex = 0;
    }

    private void FollowMovement(Vector2 obj)
    {
        if (!gameObject.activeSelf || !_isReady)
        {
            return;
        }

        var path = PathTracker.GetPlayerPath();

        if (_currentPathIndex >= path.Count)
        {
            return;
        }

        var previousPosition = (Vector2)transform.position;
        var nextPosition = path[_currentPathIndex];

        transform.position = nextPosition;

        var dir = (nextPosition - previousPosition).normalized;
        if (dir != Vector2.zero)
        {
            RotateSprite(dir);
        }

        _currentPathIndex++;
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
        _isReady = true;
    }
}
