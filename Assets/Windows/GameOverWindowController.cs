using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class GameOverWindowController : WindowController
{
    [SerializeField] private TextMeshProUGUI _title;
    [SerializeField] private TextMeshProUGUI _thanks;
    
    [SerializeField] private Color _gameLostTitleColor;
    [SerializeField] private Color _gameWonTitleColor;

    
    [Header("Transition Configuration")]
    [SerializeField] private GameObject[] _elementsToHide;
    [SerializeField] private float _transitionDuration = 2f;
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private Vector3 _targetScale = Vector3.one;
    [SerializeField] private UnityEvent _onTransitionEnded;
    
    private Coroutine _transitionCoroutine;
    
    public void Initialize(bool isPlayerDead)
    {
        if (isPlayerDead)
        {
            _title.text = "Game Over";
            _title.color = _gameLostTitleColor;
            _thanks.gameObject.SetActive(false);
        }
        else
        {
            _title.text = "Victory";
            _thanks.gameObject.SetActive(true);
            _title.color = _gameWonTitleColor;
        }
    }
    
    public void Reset()
    {
        _rectTransform.localScale = Vector3.one;
        foreach (var elementToHide in _elementsToHide)
        {
            elementToHide.SetActive(true);
        }
    }
    
    public override void Hide(bool useTransition = false)
    {
        if (!useTransition)
        {
            gameObject.SetActive(false);
            return;
        }
        
        AudioManager.Instance.CrossfadeSoundtrack(SongTitle.OstMainMenu, SongTitle.OstGameplay, _transitionDuration);
        
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
        gameObject.SetActive(false);
    }
}
