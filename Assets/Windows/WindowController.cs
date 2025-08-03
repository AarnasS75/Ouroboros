using UnityEngine;

public abstract class WindowController : MonoBehaviour
{
    [SerializeField] private Cursor[] _cursors;

    private int _currentCursorIndex = 0;
    
    protected Cursor ActiveCursor => _cursors[_currentCursorIndex];
    
    protected virtual void OnEnable()
    {
        for (var i = 0; i < _cursors.Length; i++)
        {
            if (i == 0)
            {
                _currentCursorIndex = i;
                _cursors[i].Enable();
            }
            else
            {
                _cursors[i].Disable();
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
        _cursors[_currentCursorIndex].Select();
    }
    
    protected virtual void OnMoveCursor(Vector2 direction)
    {
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
        AudioManager.Instance.PlaySFX(SfxTitle.MenuSelect);

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

    public virtual void Show()
    {
        gameObject.SetActive(true);
    }

    public virtual void Hide(bool useTransition = false)
    {
        gameObject.SetActive(false);
    }
}
