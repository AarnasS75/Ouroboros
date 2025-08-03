using TMPro;
using UnityEngine;

public class GameOverWindowController : WindowController
{
    [SerializeField] private TextMeshProUGUI _title;
    
    [SerializeField] private Color _gameLostTitleColor;
    [SerializeField] private Color _gameWonTitleColor;
    
    public void Initialize(bool isPlayerDead)
    {
        if (isPlayerDead)
        {
            _title.text = "Game Over";
            _title.color = _gameLostTitleColor;
        }
        else
        {
            _title.text = "Victory";
            _title.color = _gameWonTitleColor;
        }
        AudioManager.Instance.CrossfadeSoundtrack(AudioTitle.OstGameplay, AudioTitle.OstMainMenu, 2f);
    }
}
