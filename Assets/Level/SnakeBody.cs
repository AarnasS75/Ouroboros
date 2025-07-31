using System.Linq;
using Static_Events;
using UnityEngine;

public class SnakeBody : MonoBehaviour
{
    private GameObject[] _parts;

    private void Awake()
    {
        _parts = new GameObject[transform.childCount];
        
        for (var i = 0; i < transform.childCount; i++)
        {
            _parts[i] = transform.GetChild(i).gameObject;
            _parts[i].SetActive(false);
        }
    }

    private void OnEnable()
    {
        StaticEventHandler.OnFoodConsumed += EnablePart;
    }

    private void OnDisable()
    {
        StaticEventHandler.OnFoodConsumed -= EnablePart;
    }

    private void EnablePart(Food food)
    {
        _parts.FirstOrDefault(x => !x.activeSelf)?.gameObject.SetActive(true);

        if (_parts.All(x => x.activeSelf))
        {
            StaticEventHandler.CallGameFinishedEvent();
            print("Game Over! Well Done");
        }
    }
}
