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

    private void Awake()
    {
        _pressAction = playerInput.actions["PrimaryContact"];
        _positionAction = playerInput.actions["PrimaryPosition"];
    }

    private void OnEnable()
    {
        _pressAction.started += OnStarted;
        _pressAction.canceled += DetectSwipe;
    }

    private void OnDisable()
    {
        _pressAction.started -= OnStarted;
        _pressAction.canceled -= DetectSwipe;
    }

    private void OnStarted(InputAction.CallbackContext context)
    {
        _initialPos = _positionAction.ReadValue<Vector2>();
    }

    private void DetectSwipe(InputAction.CallbackContext context)
    {
        var delta = _positionAction.ReadValue<Vector2>() - _initialPos;
        if (delta.magnitude < swipeResistance) return;
        var direction = delta.normalized;
        if (direction != Vector2.zero & SwipePerformed != null)
            SwipePerformed(direction);
    }
}