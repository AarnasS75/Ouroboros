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
    
    private Transform _transform;
    private Coroutine _swayCoroutine;
    private Coroutine _colorCoroutine;
    private Color _initialColor;
    
    private void Awake()
    {
        _initialColor = Camera.main.backgroundColor;
        _transform = transform;
    }

    private void OnEnable()
    {
        StaticEventHandler.OnGameFinished += OnGameFinished;
    }

    private void OnDisable()
    {
        StaticEventHandler.OnGameFinished -= OnGameFinished;
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

        Camera.main.backgroundColor = _initialColor;
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
                Camera.main.backgroundColor = Color.Lerp(from, to, t);
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
}
