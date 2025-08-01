using Static_Events;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    [SerializeField] private Transform _sprite;
    
    private PlayerMovement _player;
    private int _currentPathIndex = 0;
    
    public void Initialize(PlayerMovement player)
    {
        _player = player;
        _player.OnMove += FollowMovement;
    }

    private void OnDisable()
    {
        if (!_player)
        {
            return;
        }

        _currentPathIndex = 0;
    }

    private void FollowMovement(Vector2 obj)
    {
        if (!gameObject.activeSelf)
        {
            return;
        }

        var path = PathTracker.GetPlayerPath();

        if (_currentPathIndex >= path.Count)
        {
            return;
        }
        
        transform.position = path[_currentPathIndex];
        //RotateSprite(transform.position);
        _currentPathIndex++;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent(out PlayerMovement player))
        {
            return;
        }
        
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
