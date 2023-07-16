using UnityEngine;
using UnityEngine.Events;

namespace SkibidiRunner.Touchscreen
{
    public class PlayerTouchscreenControl : MonoBehaviour
    {
        [SerializeField] private SwipeDetection swipeDetection;
        [SerializeField, Range(0,1)] private float accuracy;
        
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
            if (Vector2.Dot(direction, Vector2.up) >= accuracy)
            {
                onSwipeUp?.Invoke(); 
            }
            else if(Vector2.Dot(direction, Vector2.down) >= accuracy)
            {
                onSwipeDown?.Invoke(); 
            }
            else if(Vector2.Dot(direction, Vector2.left) >= accuracy)
            {
                onSwipeLeft?.Invoke(); 
            }
            else if(Vector2.Dot(direction, Vector2.right) >= accuracy)
            {
                onSwipeRight?.Invoke(); 
            }
        }
    }
}