using System;

namespace Static_Events
{
    public static class StaticEventHandler
    {
        public static event Action<Food> OnFoodConsumed;
        public static event Action<Food> OnFoodDestroyed;
        public static event Action OnGameFinished;
        
        public static void CallFoodConsumedEvent(Food consumedFood)
        {
            OnFoodConsumed?.Invoke(consumedFood);
        }
        
        public static void CallFoodDestroyedEvent(Food destoryedFood)
        {
            OnFoodDestroyed?.Invoke(destoryedFood);
        }
        
        public static void CallGameFinishedEvent()
        {
            OnGameFinished?.Invoke();
        }
    }
}