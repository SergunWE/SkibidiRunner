using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwipeDetection : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private float swipeResistance = 100;
    [SerializeField] private float swipeMulti = 5;

    public event Action<Vector2> SwipePerformed;

    private InputAction _positionAction, _pressAction;
    private Vector2 _initialPos;
    private Vector2 CurrentPos => _positionAction.ReadValue<Vector2>();

    private void Awake()
    {
        _pressAction = playerInput.actions["PrimaryContact"];
        _positionAction = playerInput.actions["PrimaryPosition"];
        
        _pressAction.performed += _ => { _initialPos = CurrentPos; };
        _pressAction.canceled += _ => DetectSwipe();
    }

    private void DetectSwipe()
    {
        var delta = CurrentPos - _initialPos;
        var direction = Vector2.zero;

        if (Mathf.Abs(delta.x) > swipeResistance)
        {
            direction.x = Mathf.Clamp(delta.x * swipeMulti, -1, 1);
        }

        if (Mathf.Abs(delta.y) > swipeResistance)
        {
            direction.y = Mathf.Clamp(delta.y* swipeMulti, -1, 1);
        }

        if (direction != Vector2.zero & SwipePerformed != null)
            SwipePerformed(direction);
    }
}