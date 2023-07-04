using System;
using System.Collections;
using TempleRun;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SkibidiRunner
{
    [RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float initialPlayerSpeed = 4f;
        [SerializeField] private float jumpHeight = 1.0f;
        [SerializeField] private float initialGravityValue = -9.81f;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private LayerMask turnLayer;
        [SerializeField] private LayerMask obstacleLayer;
        [SerializeField] private Animator animator;

        public Action<Vector3> TurnEvent { private get; set; }
        public Action GameOverEvent { private get; set; }

        private float _playerSpeed;
        private float _gravity;
        private Vector3 _movementDirection = Vector3.forward;
        private PlayerInput _playerInput;
        private InputAction _turnAction;
        private InputAction _jumpAction;
        private InputAction _slideAction;
        private CharacterController _controller;
        private Vector3 _playerVelocity;
        private bool _sliding;
        private int _slidingAnimationId;
        private int _jumpAnimationId;
        private int _deathAnimationId;

        private readonly Collider[] _hitColliders = new Collider[5];

        private void Awake()
        {
            _playerInput = GetComponent<PlayerInput>();
            _controller = GetComponent<CharacterController>();
            _slidingAnimationId = Animator.StringToHash("RunningSlide");
            _jumpAnimationId = Animator.StringToHash("Jump");
            _deathAnimationId = Animator.StringToHash("FallingBackDeath");
            _turnAction = _playerInput.actions["Turn"];
            _jumpAction = _playerInput.actions["Jump"];
            _slideAction = _playerInput.actions["Slide"];
        }

        private void Start()
        {
            _gravity = initialGravityValue;
            _playerSpeed = initialPlayerSpeed;
        }

        private void OnEnable()
        {
            _turnAction.performed += PlayerTurn;
            _slideAction.performed += PlayerSlide;
            _jumpAction.performed += PlayerJump;
        }

        private void OnDisable()
        {
            _turnAction.performed -= PlayerTurn;
            _slideAction.performed -= PlayerSlide;
            _jumpAction.performed -= PlayerJump;
        }

        private void Update()
        {
            if (!IsGrounded(20))
            {
                GameOver();
                return;
            }

            _controller.Move(transform.forward * (_playerSpeed * Time.deltaTime));

            if (IsGrounded() && _playerVelocity.y < 0)
            {
                _playerVelocity.y = 0f;
            }

            _playerVelocity.y += _gravity * Time.deltaTime;
            _controller.Move(_playerVelocity * Time.deltaTime);
        }

        private void PlayerTurn(InputAction.CallbackContext context)
        {
            float contextValue = context.ReadValue<float>();

            var turnPosition = CheckTurn(contextValue);
            if (turnPosition.HasValue)
            {
                var targetDirection = Quaternion.AngleAxis(90 * context.ReadValue<float>(), Vector3.up) *
                                      _movementDirection;
                TurnEvent?.Invoke(targetDirection);
                Turn(contextValue, turnPosition.Value);
            }
            else
            {
                GameOver();
            }
        }

        private void PlayerSlide(InputAction.CallbackContext context)
        {
            if (!_sliding)
            {
                StartCoroutine(Slide());
            }
        }

        private void PlayerJump(InputAction.CallbackContext context)
        {
            if (!IsGrounded() && (!_sliding || !IsGrounded(1f))) return;
            _playerVelocity.y = 0;
            _playerVelocity.y += Mathf.Sqrt(jumpHeight * _gravity * -3f);
            animator.Play(_jumpAnimationId);
        }

        private IEnumerator Slide()
        {
            _sliding = true;
            _playerVelocity.y -= Mathf.Sqrt(jumpHeight * _gravity * -3f);
            // Shrink the collider
            Vector3 originalControllerCenter = _controller.center;
            Vector3 newControllerCenter = originalControllerCenter;
            _controller.height /= 2;
            newControllerCenter.y -= _controller.height / 2;
            _controller.center = newControllerCenter;
            // PLay the sliding animation
            animator.Play(_slidingAnimationId);
            yield return new WaitForSeconds(0.8f);
            // Set the character controller collider back to normal after sliding.
            _controller.height *= 2;
            _controller.center = originalControllerCenter;
            _sliding = false;
        }

        private void Turn(float turnValue, Vector3 turnPosition)
        {
            var transform1 = transform;
            var tempPlayerPosition = new Vector3(turnPosition.x, transform1.position.y, turnPosition.z);
            _controller.enabled = false;
            transform1.position = tempPlayerPosition;
            _controller.enabled = true;
            var targetRotation = transform1.rotation * Quaternion.Euler(0, 90 * turnValue, 0);
            var transform2 = transform;
            transform2.rotation = targetRotation;
            _movementDirection = transform2.forward.normalized;
        }

        private void GameOver()
        {
            animator.Play(_deathAnimationId);
            GameOverEvent?.Invoke();
            enabled = false;
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

        private bool IsGrounded(float length = .4f)
        {
            var transform1 = transform;
            var raycastOriginFirst = transform1.position;
            raycastOriginFirst.y -= _controller.height / 2f;
            raycastOriginFirst.y += .1f;

            var raycastOriginSecond = raycastOriginFirst;
            var forward = transform1.forward;
            raycastOriginFirst -= forward * .2f;
            raycastOriginSecond += forward * .2f;

            return Physics.Raycast(raycastOriginFirst, Vector3.down, out var hit, length, groundLayer) ||
                   Physics.Raycast(raycastOriginSecond, Vector3.down, out var hit2, length, groundLayer);
        }

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            if (((1 << hit.collider.gameObject.layer) & obstacleLayer) != 0)
            {
                GameOver();
            }
        }
    }
}