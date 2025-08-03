using Static_Events;
using UnityEngine;

public class Food : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out PlayerMovement player))
        {
            AudioManager.Instance.PlaySFX(SfxTitle.FoodConsumed);
            StaticEventHandler.CallFoodConsumedEvent(this);
        }
    }
}
