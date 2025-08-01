using System;
using System.Collections;
using System.Linq;
using Static_Events;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private int _maxEnemyCount = 7;
    [SerializeField] private float _enemySpawnInterval = 5f;
    [SerializeField] private Ghost _ghostPrefab;

    public event Action<Ghost> OnEnemySpawned;
    
    private Ghost[] _ghosts;
    private WaitForSeconds _waitForNewEnemyToSpawn;

    private Vector2 _startPos;
    
    public void Initialize(PlayerMovement player)
    {
        _ghosts = new Ghost[_maxEnemyCount];
        _startPos = player.transform.position;
        
        for (var i = 0; i < _maxEnemyCount; i++)
        {
            _ghosts[i] = Instantiate(_ghostPrefab, _startPos, Quaternion.identity);
            _ghosts[i].gameObject.SetActive(false);
            _ghosts[i].Initialize(player);
        }

        _waitForNewEnemyToSpawn = new WaitForSeconds(_enemySpawnInterval);
    }

    public void StartSpawning()
    {
        StartCoroutine(nameof(EnemySpawnRoutine));
    }

    private void OnEnable()
    {
        StaticEventHandler.OnGameFinished += OnGameOver;
    }

    private void OnDisable()
    {
        StaticEventHandler.OnGameFinished -= OnGameOver;
    }

    private void OnGameOver(GameOverEventArgs args)
    {
        StopAllCoroutines();
    }

    private IEnumerator EnemySpawnRoutine()
    {
        while (true)
        {
            yield return _waitForNewEnemyToSpawn;

            var ghost = _ghosts.FirstOrDefault(x => !x.isActiveAndEnabled);
            if (!ghost)
            {
                yield break;
            }
            
            ghost.gameObject.SetActive(true);
            OnEnemySpawned?.Invoke(ghost);
        }
    }

    public void Reset()
    {
        for (var i = 0; i < _maxEnemyCount; i++)
        {
            _ghosts[i].gameObject.SetActive(false);
            _ghosts[i].transform.position= _startPos;
        }
    }
}