using Static_Events;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private Vector2 _levelBounds = new (5, 3);
    [SerializeField] private Vector3 _noSpawnZoneCenter = new(0, 2, 0);
    [SerializeField] private Vector2 _noSpawnZoneSize = new(3, 2);
    [SerializeField] private Transform _playerSpawnPoint;

    [Header("Controllers")]
    [SerializeField] private EnemySpawner _enemySpawner;
    [SerializeField] private SnakeBodyController _snakeBodyController;
    
    [Header("Prefabs")]
    [SerializeField] private Food _foodPrefab;
    [SerializeField] private PlayerMovement _playerPrefab;
    
    public const float GridSize = 1f;
    
    private Food _activeFood;
    private Vector2 _playerSpawnPos;
    private PlayerMovement _player;
    
    private void Start()
    {
        _player = FindAnyObjectByType<PlayerMovement>();
        
        if (!_player)
        {
            _player = Instantiate(_playerPrefab, _playerSpawnPoint.position, Quaternion.identity);
        }
        
        SpawnFood();
    }

    private void OnEnable()
    {
        StaticEventHandler.OnFoodConsumed += SpawnNewFood;
        StaticEventHandler.OnFoodDestroyed += SpawnNewFood;
        _enemySpawner.OnEnemySpawned += PositionEnemy;
    }

    private void OnDisable()
    {
        StaticEventHandler.OnFoodConsumed -= SpawnNewFood;
        StaticEventHandler.OnFoodDestroyed -= SpawnNewFood;
        _enemySpawner.OnEnemySpawned -= PositionEnemy;
    }

    private void SpawnNewFood(Food usedFood)
    {
        if (_activeFood)
        {
            _activeFood.gameObject.SetActive(false);
        }

        _snakeBodyController.EnablePart();
        if (!_snakeBodyController.IsWaitingForHead)
        {
            SpawnFood();
        }
    }
    
    private void PositionEnemy(Ghost enemy)
    {
        enemy.transform.position = _playerSpawnPoint.position;
        enemy.Initialize(_player);
    }
    
    private void SpawnFood()
    {
        if (!_activeFood)
        {
            _activeFood = Instantiate(_foodPrefab);
        }

        var spawnPosition = GetRandomGridPosition();
        _activeFood.transform.position = spawnPosition;
        _activeFood.gameObject.SetActive(true);
    }

    private Vector3 GetRandomGridPosition()
    {
        Vector3 position;
        var attempts = 0;

        do
        {
            var x = Random.Range(-(int)_levelBounds.x, (int)_levelBounds.x + 1);
            var y = Random.Range(-(int)_levelBounds.y, (int)_levelBounds.y + 1);
            position = new Vector3(x * GridSize, y * GridSize, 0);
            attempts++;
        }
        while (IsInNoSpawnZone(position) && attempts < 100);

        return position;
    }

    private bool IsInNoSpawnZone(Vector3 position)
    {
        var min = _noSpawnZoneCenter - new Vector3(_noSpawnZoneSize.x / 2f, _noSpawnZoneSize.y / 2f, 0);
        var max = _noSpawnZoneCenter + new Vector3(_noSpawnZoneSize.x / 2f, _noSpawnZoneSize.y / 2f, 0);

        return position.x >= min.x && position.x <= max.x &&
               position.y >= min.y && position.y <= max.y;
    }

    private void OnDrawGizmos()
    {
        // Level bounds
        Gizmos.color = Color.green;
        var center = Vector3.zero;
        var size = new Vector3(_levelBounds.x * 2 + 1, _levelBounds.y * 2 + 1, 0.1f);
        Gizmos.DrawWireCube(center, size);

        // No-spawn zone
        Gizmos.color = Color.red;
        var noSpawnSize = new Vector3(_noSpawnZoneSize.x, _noSpawnZoneSize.y, 0.1f);
        Gizmos.DrawWireCube(_noSpawnZoneCenter, noSpawnSize);
    }
}