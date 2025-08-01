using Static_Events;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    [SerializeField] private WindowController _mainMenuWindow;
    [SerializeField] private WindowController _gameOverWindow;
    
    private void OnEnable()
    {
        LevelManager.OnLevelReset += OnLevelReset;
    }

    private void OnDisable()
    {
        LevelManager.OnLevelReset -= OnLevelReset;
    }

    private void Start()
    {
        _gameOverWindow.Hide();
        _mainMenuWindow.Show();
    }

    private void OnLevelReset()
    {
        _gameOverWindow.Show();
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}