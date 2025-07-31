using Static_Events;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private Vector2 _levelBounds = new (5, 3);
    
    [SerializeField] private Food _foodPrefab;
    [SerializeField] private Ghost _ghostPrefab;
    
    public const float GridSize = 1f;
    
    private Food _activeFood;

    private void Start()
    {
        SpawnFood();
    }

    private void OnEnable()
    {
        StaticEventHandler.OnFoodConsumed += SpawnNewFood;
        StaticEventHandler.OnFoodDestroyed += SpawnNewFood;
    }

    private void OnDisable()
    {
        StaticEventHandler.OnFoodConsumed -= SpawnNewFood;
        StaticEventHandler.OnFoodDestroyed -= SpawnNewFood;
    }

    private void SpawnNewFood(Food usedFood)
    {
        if (_activeFood != null)
        {
            _activeFood.gameObject.SetActive(false);
        }

        SpawnFood();
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
        var x = Random.Range(-(int)_levelBounds.x, (int)_levelBounds.x + 1);
        var y = Random.Range(-(int)_levelBounds.y, (int)_levelBounds.y + 1);

        return new Vector3(x * GridSize, y * GridSize, 0);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        var center = Vector3.zero;
        var size = new Vector3(_levelBounds.x * 2 + 1, _levelBounds.y * 2 + 1, 0.1f);

        Gizmos.DrawWireCube(center, size);
    }
}