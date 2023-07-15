using System.Collections;
using TempleRun;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace SkibidiRunner.Player
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

        public UnityEvent TurnEvent { private get; set; }
        public UnityEvent GameOverEvent { private get; set; }
        public UnityEvent JumpEvent { private get; set; }
        public UnityEvent SlidingEvent { private get; set; }

        private float _playerSpeed;
        private float _gravity;
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
        private int _idleAnimationId;

        private readonly Collider[] _hitColliders = new Collider[5];
        private Vector3 _originalControllerCenter;
        private Vector3 _slideControllerCenter;
        private float _originalControllerHeight;
        private float _slideControllerHeight;
        private Coroutine _slideCoroutine;

        public void Awake()
        {
            _playerInput = GetComponent<PlayerInput>();
            _controller = GetComponent<CharacterController>();
            _slidingAnimationId = Animator.StringToHash("RunningSlide");
            _jumpAnimationId = Animator.StringToHash("Jump");
            _deathAnimationId = Animator.StringToHash("FallingBackDeath");
            _idleAnimationId = Animator.StringToHash("StandardRun");
            _turnAction = _playerInput.actions["Turn"];
            _jumpAction = _playerInput.actions["Jump"];
            _slideAction = _playerInput.actions["Slide"];
            _originalControllerCenter = _controller.center;
            _originalControllerHeight = _controller.height;
            _slideControllerCenter =
                _originalControllerCenter + Vector3.down * (_originalControllerHeight * 0.5f * 0.5f);
            _slideControllerHeight = _originalControllerHeight / 2;
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

        public void ChangePosition(Vector3 position)
        {
            _controller.enabled = false;
            transform.position = position;
            _controller.enabled = true;
        }

        public void OnLifeGained()
        {
            enabled = true;
            _playerInput.actions.Enable();
            _controller.height = _originalControllerHeight;
            _controller.center = _originalControllerCenter;
            animator.Play(_idleAnimationId);
        }

        public void PlayerTurn(float contextValue)
        {
            var turnPosition = CheckTurn(contextValue);
            if (turnPosition.HasValue)
            {
                TurnEvent?.Invoke();
            }
            else
            {
                GameOver();
            }
        }

        public void PlayerSlide()
        {
            if (_sliding) return;
            if (_slideCoroutine != null)
            {
                StopCoroutine(_slideCoroutine);
            }
            animator.StopPlayback();
            _slideCoroutine = StartCoroutine(Slide());
            SlidingEvent?.Invoke();
        }

        public void PlayerJump()
        {
            if (!IsGrounded()) return;
            _controller.center = _originalControllerCenter;
            _controller.height = _originalControllerHeight;
            _playerVelocity.y = 0;
            _playerVelocity.y += Mathf.Sqrt(jumpHeight * _gravity * -3f);
            animator.Play(_jumpAnimationId);
            JumpEvent?.Invoke();
        }

        private void PlayerTurn(InputAction.CallbackContext context)
        {
            PlayerTurn(context.ReadValue<float>());
        }

        private void PlayerSlide(InputAction.CallbackContext context)
        {
            PlayerSlide();
        }

        private void PlayerJump(InputAction.CallbackContext context)
        {
            PlayerJump();
        }

        private IEnumerator Slide()
        {
            //_sliding = true;
            _controller.height = _slideControllerHeight;
            _controller.center = _slideControllerCenter;
            _playerVelocity.y -= Mathf.Sqrt(jumpHeight * _gravity * -3f);
            animator.Play(_slidingAnimationId, -1, 0);
            yield return new WaitForSeconds(0.7f);
            _controller.center = _originalControllerCenter;
            _controller.height = _originalControllerHeight;
            _sliding = false;
        }

        private void GameOver()
        {
            _playerInput.actions.Disable();
            _controller.height = _originalControllerHeight;
            _controller.center = _originalControllerCenter;
            animator.Play(_deathAnimationId);
            GameOverEvent?.Invoke();
            enabled = false;
        }

        private Vector3? CheckTurn(float turnValue)
        {
            int size = Physics.OverlapSphereNonAlloc(transform.position, .1f, _hitColliders, turnLayer);
            if (size == 0) return null;
            var tile = _hitColliders[0].transform.parent.GetComponent<Tile>();
            var type = tile.Type;
            if ((type == TileType.Left && turnValue == -1) || (type == TileType.Right && turnValue == 1) ||
                (type == TileType.Sideways))
            {
                return tile.Pivot.position;
            }

            return null;
        }

        private bool IsGrounded(float length = .6f)
        {
            var raycastPosition = transform.position + _slideControllerCenter;
            return Physics.Raycast(raycastPosition, Vector3.down, out var hit, length, groundLayer);
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