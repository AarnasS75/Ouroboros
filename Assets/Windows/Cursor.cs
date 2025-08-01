using UnityEngine;
using UnityEngine.UI;

public class Cursor : MonoBehaviour
{
    [SerializeField] private Image _cursorImage;
    [SerializeField] private Button _button;
    
    public void Select()
    {
        _button.onClick.Invoke();
    }
    
    public void Enable()
    {
        _cursorImage.enabled = true;
    }

    public void Disable()
    {
        _cursorImage.enabled = false;
    }
}
