using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class OptionsWindowController : WindowController
{
    [SerializeField] private Image _masterVolumeImageFill;
    [SerializeField] private Image _sfxVolumeImageFill;
    [SerializeField] private Toggle _muteToggle;

    protected override void OnEnable()
    {
        base.OnEnable();
        
        _masterVolumeImageFill.fillAmount = PlayerPrefs.GetFloat("Master", 1);
        _sfxVolumeImageFill.fillAmount = PlayerPrefs.GetFloat("Sfx", 1);
        _muteToggle.isOn = PlayerPrefs.GetInt("Muted") == 1;
    }

    protected override void OnMoveCursor(Vector2 direction)
    {
        base.OnMoveCursor(direction);

        if (direction == Vector2.right)
        {
            ActiveCursor.Increase();
        }
        else if (direction == Vector2.left)
        {
            ActiveCursor.Decrease();
        }
    }
}
