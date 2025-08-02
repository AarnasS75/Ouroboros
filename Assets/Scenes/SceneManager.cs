using Static_Events;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    [SerializeField] private MainMenuWindowController _mainMenuWindow;
    [SerializeField] private GameOverWindowController _gameOverWindow;
    
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

    private void OnLevelReset(bool isPlayerDead)
    {
        _gameOverWindow.Show();
        _gameOverWindow.Initialize(isPlayerDead);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}