using System.Linq;
using Static_Events;
using UnityEngine;

public class SnakeBodyController : MonoBehaviour
{
    private GameObject[] _parts;

    public bool IsWaitingForHead;
    
    private void Awake()
    {
        _parts = new GameObject[transform.childCount];
        
        for (var i = 0; i < transform.childCount; i++)
        {
            _parts[i] = transform.GetChild(i).gameObject;
            _parts[i].SetActive(false);
        }
    }

    public void EnablePart()
    {
        _parts.FirstOrDefault(x => !x.activeSelf)?.gameObject.SetActive(true);

        if (_parts.Count(x => x.activeSelf) + 1 == _parts.Length)
        {
            IsWaitingForHead = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent(out PlayerMovement player) || !IsWaitingForHead)
        {
            return;
        }
        
        EnablePart();
        IsWaitingForHead = false;
        StaticEventHandler.CallGameFinishedEvent(new GameOverEventArgs
        {
            IsPlayerDead = false
        });
    }

    public void Reset()
    {
        IsWaitingForHead = false;
        for (var i = 0; i < transform.childCount; i++)
        {
            _parts[i].SetActive(false);
        }
    }
}
