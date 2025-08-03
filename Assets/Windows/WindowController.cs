using UnityEngine;

public abstract class WindowController : MonoBehaviour
{
    [SerializeField] private Cursor[] _cursors;

    private int _currentCursorIndex = 0;
    private bool _isBusy;
    
    protected virtual void OnEnable()
    {
        _isBusy = false;
        for (var i = 0; i < _cursors.Length; i++)
        {
            _cursors[i].Disable();
            if (i == 0)
            {
                _cursors[i].Enable();
            }
        }

        InputManager.OnMoveDirectionChanged += OnMoveCursor;
        InputManager.OnInteract += OnInteract;
    }

    protected virtual void OnDisable()
    {
        InputManager.OnMoveDirectionChanged -= OnMoveCursor;
        InputManager.OnInteract -= OnInteract;
    }

    private void OnInteract()
    {
        _isBusy = true;
        _cursors[_currentCursorIndex].Select();
    }
    
    private void OnMoveCursor(Vector2 direction)
    {
        if (_isBusy)
        {
            return;
        }
        
        AudioManager.Instance.PlaySFX(AudioTitle.MenuSelect);
        
        if (direction == Vector2.up)
        {
            MoveCursor(-1);
        }
        else if (direction == Vector2.down)
        {
            MoveCursor(1);
        }
    }
    
    private void MoveCursor(int delta)
    {
        _cursors[_currentCursorIndex].Disable();

        _currentCursorIndex += delta;

        if (_currentCursorIndex < 0)
        {
            _currentCursorIndex = _cursors.Length - 1;
        }
        else if (_currentCursorIndex >= _cursors.Length)
        {
            _currentCursorIndex = 0;
        }

        _cursors[_currentCursorIndex].Enable();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
