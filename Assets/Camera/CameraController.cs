using System.Collections;
using Static_Events;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float _swaySpeed = 0.5f;
    [SerializeField] private float _maxSwayAngle = 1f;

    [Header("Background color animation")]
    [SerializeField] private Color[] _colorsToLoop;
    [SerializeField] private float _speed = 1f;
    
    [Header("Zoom on food consumption")]
    [SerializeField] private float _zoomSize = 3f;
    [SerializeField] private float _zoomDuration = 0.15f;
    [SerializeField] private float _returnDuration = 0.25f;
    
    private Coroutine _zoomCoroutine;
    private Camera _mainCamera;
    private float _originalSize;
    
    private Transform _transform;
    private Coroutine _swayCoroutine;
    private Coroutine _colorCoroutine;
    private Color _initialColor;
    
    private void Awake()
    {
        _mainCamera = Camera.main;
        _originalSize = _mainCamera.orthographicSize;
        _initialColor = _mainCamera.backgroundColor;
        _transform = transform;
    }

    private void OnEnable()
    {
        StaticEventHandler.OnGameFinished += OnGameFinished;
        StaticEventHandler.OnFoodConsumed += OnFoodConsumed;
    }

    private void OnDisable()
    {
        StaticEventHandler.OnGameFinished -= OnGameFinished;
        StaticEventHandler.OnFoodConsumed -= OnFoodConsumed;
    }

    private void OnFoodConsumed(Food food)
    {
        if (_zoomCoroutine != null)
        {
            StopCoroutine(_zoomCoroutine);
        }

        _zoomCoroutine = StartCoroutine(ZoomInAndOut());
    }

    public void StartSway()
    {
        _colorCoroutine= StartCoroutine(LoopingBackgroundColorRoutine(_colorsToLoop, _speed));
        _swayCoroutine = StartCoroutine(SwayRoutine());
    }

    private void OnGameFinished(GameOverEventArgs obj)
    {
        StopSway();
        StopColorSwitch();
        _transform.localEulerAngles = Vector3.zero;
    }

    public void StopSway()
    {
        if (_swayCoroutine != null)
        {
            StopCoroutine(_swayCoroutine);
        }
    }
    
    public void StopColorSwitch()
    {
        if (_colorCoroutine != null)
        {
            StopCoroutine(_colorCoroutine);
        }

        _mainCamera.backgroundColor = _initialColor;
    }
    
    private IEnumerator LoopingBackgroundColorRoutine(Color[] colors, float duration)
    {
        int index = 0;

        while (true)
        {
            Color from = colors[index];
            Color to = colors[(index + 1) % colors.Length];
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);
                _mainCamera.backgroundColor = Color.Lerp(from, to, t);
                yield return null;
            }

            index = (index + 1) % colors.Length;
        }
    }


    private IEnumerator SwayRoutine()
    {
        var time = 0f;

        while (true)
        {
            time += Time.deltaTime * _swaySpeed;
            var angle = Mathf.Sin(time) * _maxSwayAngle;

            _transform.localRotation = Quaternion.Euler(0f, 0f, angle);

            yield return null;
        }
    }
    
    private IEnumerator ZoomInAndOut()
    {
        float elapsed = 0f;
        float startSize = _mainCamera.orthographicSize;

        // Zoom in
        while (elapsed < _zoomDuration)
        {
            elapsed += Time.deltaTime;
            _mainCamera.orthographicSize = Mathf.Lerp(startSize, _zoomSize, elapsed / _zoomDuration);
            yield return null;
        }

        // Zoom out
        elapsed = 0f;
        while (elapsed < _returnDuration)
        {
            elapsed += Time.deltaTime;
            _mainCamera.orthographicSize = Mathf.Lerp(_zoomSize, _originalSize, elapsed / _returnDuration);
            yield return null;
        }

        _mainCamera.orthographicSize = _originalSize;
    }
}
