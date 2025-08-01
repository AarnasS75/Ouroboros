using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class MainMenuWindowController : WindowController
{
    [Header("Transition Configuration")]
    [SerializeField] private GameObject[] _elementsToHide;
    [SerializeField] private float _transitionDuration = 2f;
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private Vector3 _targetScale = Vector3.one;
    [SerializeField] private UnityEvent _onTransitionEnded;

    private Coroutine _transitionCoroutine;

    public void Begin()
    {
        foreach (var elementToHide in _elementsToHide)
        {
            elementToHide.SetActive(false);
        }
        
        if (_transitionCoroutine != null)
        {
            StopCoroutine(_transitionCoroutine);
        }

        _transitionCoroutine = StartCoroutine(ScaleUpCoroutine());
    }

    public void Reset()
    {
        _rectTransform.localScale = Vector3.one;
        foreach (var elementToHide in _elementsToHide)
        {
            elementToHide.SetActive(true);
        }
    }

    private IEnumerator ScaleUpCoroutine()
    {
        Vector3 initialScale = _rectTransform.localScale;
        float elapsedTime = 0f;

        while (elapsedTime < _transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / _transitionDuration);
            _rectTransform.localScale = Vector3.Lerp(initialScale, _targetScale, t);
            yield return null;
        }

        _rectTransform.localScale = _targetScale;
        _onTransitionEnded?.Invoke();
    }
}