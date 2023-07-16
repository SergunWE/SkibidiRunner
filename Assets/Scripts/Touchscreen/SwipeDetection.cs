using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwipeDetection : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private float swipeResistance = 100;

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
        if (delta.magnitude < swipeResistance) return;
        var direction = delta.normalized;
        if (direction != Vector2.zero & SwipePerformed != null)
            SwipePerformed(direction);
    }
}