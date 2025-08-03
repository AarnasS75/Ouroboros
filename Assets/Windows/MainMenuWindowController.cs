using System;
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

    private void Start()
    {
        AudioManager.Instance.PlaySoundtrack(AudioTitle.OstMainMenu);
    }

    public void Begin()
    {
        AudioManager.Instance.CrossfadeSoundtrack(AudioTitle.OstMainMenu, AudioTitle.OstGameplay, _transitionDuration);
        
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
        var initialScale = _rectTransform.localScale;
        var elapsedTime = 0f;

        while (elapsedTime < _transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            var t = Mathf.Clamp01(elapsedTime / _transitionDuration);
            _rectTransform.localScale = Vector3.Lerp(initialScale, _targetScale, t);
            yield return null;
        }
        
        _rectTransform.localScale = _targetScale;
        _onTransitionEnded?.Invoke();
    }
}