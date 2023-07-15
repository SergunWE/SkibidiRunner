using UnityEngine;
using UnityEngine.Events;

namespace SkibidiRunner.Touchscreen
{
    public class PlayerTouchscreenControl : MonoBehaviour
    {
        [SerializeField] private SwipeDetection swipeDetection;
        
        [SerializeField] private UnityEvent onSwipeUp;
        [SerializeField] private UnityEvent onSwipeDown;
        [SerializeField] private UnityEvent onSwipeLeft;
        [SerializeField] private UnityEvent onSwipeRight;

        private void Awake() 
        {
            swipeDetection.SwipePerformed += OnSwipe;
        }

        private void OnSwipe(Vector2 direction)
        {
            if (direction == Vector2.up)
            {
                onSwipeUp?.Invoke(); 
            }
            else if(direction == Vector2.down)
            {
                onSwipeDown?.Invoke(); 
            }
            else if(direction == Vector2.left)
            {
                onSwipeLeft?.Invoke(); 
            }
            else if(direction == Vector2.right)
            {
                onSwipeRight?.Invoke(); 
            }
        }
    }
}