using UnityEngine;

public class SceneManager : MonoBehaviour
{
    [SerializeField] private MainMenuWindowController _mainMenuWindow;
    [SerializeField] private GameOverWindowController _gameOverWindow;
    [SerializeField] private OptionsWindowController _optionsWindow;
    
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
        _optionsWindow.Hide();
        _mainMenuWindow.Show();
    }

    private void OnLevelReset(bool isPlayerDead)
    {
        AudioManager.Instance.CrossfadeSoundtrack(SongTitle.OstGameplay, SongTitle.OstMainMenu, 2f);
        _gameOverWindow.Show();
        _gameOverWindow.Initialize(isPlayerDead);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}