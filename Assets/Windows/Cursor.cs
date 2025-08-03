using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Cursor : MonoBehaviour
{
    [SerializeField] private Image _cursorImage;
    [SerializeField] private UnityEvent _action;

    [Header("Options Cursor Configuration")]
    [SerializeField] private Image _fillImage;
    [SerializeField] private float _stepAmount = 0.1f; 
    
    public void Select()
    {
        _action.Invoke();
    }
    
    public void Enable()
    {
        _cursorImage.enabled = true;
    }

    public void Disable()
    {
        _cursorImage.enabled = false;
    }

    public void Toggle(Toggle toggle)
    {
        toggle.isOn = !toggle.isOn;
    }

    public void Increase()
    {
        if (!_fillImage)
        {
            return;
        }
        
        _fillImage.fillAmount = Mathf.Clamp(_fillImage.fillAmount + _stepAmount, 0f, 1f);
        _action.Invoke();
    }
    
    public void Decrease()
    {
        if (!_fillImage)
        {
            return;
        }
        
        _fillImage.fillAmount = Mathf.Clamp(_fillImage.fillAmount - _stepAmount, 0f, 1f);
        _action.Invoke();
    }
}
