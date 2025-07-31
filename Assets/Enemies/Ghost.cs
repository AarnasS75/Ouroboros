using System;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    private PlayerMovement _player;
    
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

    private void FollowMovement(Vector2 direction)
    { 
        if (!_player)
        {
            return;
        }
        
        var nextPos = transform.position + (Vector3)(direction * LevelManager.GridSize);
        transform.position = nextPos;
    }
}
