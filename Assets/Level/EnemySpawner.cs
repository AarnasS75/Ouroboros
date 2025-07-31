using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private int _maxEnemyCount = 7;
    [SerializeField] private float _enemySpawnInterval = 5f;
    [SerializeField] private Ghost _ghostPrefab;

    public event Action<Ghost> OnEnemySpawned;
    
    private Ghost[] _ghosts;
    private WaitForSeconds _waitForNewEnemyToSpawn;
    
    private void Start()
    {
        _ghosts = new Ghost[_maxEnemyCount];
        for (var i = 0; i < _maxEnemyCount; i++)
        {
            _ghosts[i] = Instantiate(_ghostPrefab);
            _ghosts[i].gameObject.SetActive(false);
        }

        _waitForNewEnemyToSpawn = new WaitForSeconds(_enemySpawnInterval);

        StartCoroutine(nameof(EnemySpawnRoutine));
    }

    private IEnumerator EnemySpawnRoutine()
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