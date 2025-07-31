using UnityEngine;

public class Ghost : MonoBehaviour
{
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
        
        _player.OnMove -= FollowMovement;
    }
    private void FollowMovement(Vector2 obj)
    {
        var path = PathTracker.GetPlayerPath();

        if (_currentPathIndex >= path.Count)
        {
            return;
        }
        
        transform.position = path[_currentPathIndex];
        _currentPathIndex++;
    }
}
