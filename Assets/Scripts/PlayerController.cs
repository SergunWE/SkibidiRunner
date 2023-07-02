﻿using System;
using TempleRun;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace SkibidiRunner
{
    [RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float initialPlayerSpeed = 4f;
        [SerializeField] private float maximumPlayerSpeed = 30f;
        [SerializeField] private float playerSpeedIncreaseRate = .1f;
        [SerializeField] private float jumpHeight = 1.0f;
        [SerializeField] private float initialGravityValue = -9.81f;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private LayerMask turnLayer;
        
        public Action<Vector3> TurnEvent { private get; set; }

        private float playerSpeed;
        private float gravity;
        private Vector3 movementDirection = Vector3.forward;
        private PlayerInput playerInput;
        private InputAction turnAction;
        private InputAction jumpAction;
        private InputAction slideAction;
        private CharacterController controller;
        private Vector3 playerVelocity;
        

        private Collider[] _hitColliders = new Collider[5];

        private void Awake()
        {
            playerInput = GetComponent<PlayerInput>();
            controller = GetComponent<CharacterController>();
            turnAction = playerInput.actions["Turn"];
            jumpAction = playerInput.actions["Jump"];
            slideAction = playerInput.actions["Slide"];
        }

        private void Start()
        {
            gravity = initialGravityValue;
            playerSpeed = initialPlayerSpeed;
        }

        private void OnEnable()
        {
            turnAction.performed += PlayerTurn;
            slideAction.performed += PlayerSlide;
            jumpAction.performed += PlayerJump;
        }

        private void OnDisable()
        {
            turnAction.performed -= PlayerTurn;
            slideAction.performed -= PlayerSlide;
            jumpAction.performed -= PlayerJump;
        }

        private void Update()
        {
            controller.Move(transform.forward * (playerSpeed * Time.deltaTime));

            if (IsGrounded() && playerVelocity.y < 0)
            {
                playerVelocity.y = 0f;
            }

            playerVelocity.y += gravity * Time.deltaTime;
            controller.Move(playerVelocity * Time.deltaTime);
        }

        private void PlayerTurn(InputAction.CallbackContext context)
        {
            float contextValue = context.ReadValue<float>();
            
            var turnPosition = CheckTurn(contextValue);
            if (!turnPosition.HasValue) return;
            var targetDirection = Quaternion.AngleAxis(90 * context.ReadValue<float>(), Vector3.up) *
                                  movementDirection;
            TurnEvent?.Invoke(targetDirection);
            Debug.Log("!!!!!!!!!!!!!!!!!");
            Turn(contextValue, turnPosition.Value);
        }

        private void PlayerSlide(InputAction.CallbackContext context)
        {
        }

        private void PlayerJump(InputAction.CallbackContext context)
        {
            if (!IsGrounded()) return;
            playerVelocity.y += Mathf.Sqrt(jumpHeight * gravity * -3f);
        }

        private void Turn(float turnValue, Vector3 turnPosition)
        {
            var transform1 = transform;
            var tempPlayerPosition = new Vector3(turnPosition.x, transform1.position.y, turnPosition.z);
            controller.enabled = false;
            transform1.position = tempPlayerPosition;
            controller.enabled = true;
            var targetRotation = transform1.rotation * Quaternion.Euler(0, 90 * turnValue, 0);
            var transform2 = transform;
            transform2.rotation = targetRotation;
            movementDirection = transform2.forward.normalized;
        }

        private Vector3? CheckTurn(float turnValue)
        {
            int size = Physics.OverlapSphereNonAlloc(transform.position, .1f, _hitColliders, turnLayer);
            if (size == 0) return null;
            var tile = _hitColliders[0].transform.parent.GetComponent<Tile>();
            var type = tile.type;
            if ((type == TileType.Left && turnValue == -1) || (type == TileType.Right && turnValue == 1) ||
                (type == TileType.Sideways))
            {
                return tile.pivot.position;
            }

            return null;
        }

        private bool IsGrounded(float length = .2f)
        {
            var transform1 = transform;
            var raycastOriginFirst = transform1.position;
            raycastOriginFirst.y -= controller.height / 2f;
            raycastOriginFirst.y += .1f;

            var raycastOriginSecond = raycastOriginFirst;
            var forward = transform1.forward;
            raycastOriginFirst -= forward * .2f;
            raycastOriginSecond += forward * .2f;

            Debug.DrawLine(raycastOriginFirst, raycastOriginFirst - Vector3.down * length, Color.green, 2f);
            Debug.DrawLine(raycastOriginSecond, raycastOriginFirst - Vector3.down * length, Color.red, 2f);

            return Physics.Raycast(raycastOriginFirst, Vector3.down, out var hit, length, groundLayer) ||
                   Physics.Raycast(raycastOriginSecond, Vector3.down, out var hit2, length, groundLayer);
        }
    }
}