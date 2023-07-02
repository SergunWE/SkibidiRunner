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

        private float playerSpeed;
        private float gravity;
        private Vector3 movementDirection = Vector3.forward;
        private PlayerInput playerInput;
        private InputAction turnAction;
        private InputAction jumpAction;
        private InputAction slideAction;
        private CharacterController controller;
        private Vector3 playerVelocity;
        private bool sliding;
        private int slidingAnimationId;
        private int jumpAnimationId;
        private int deathAnimationId;


        private Collider[] _hitColliders = new Collider[5];

        private void Awake()
        {
            playerInput = GetComponent<PlayerInput>();
            controller = GetComponent<CharacterController>();
            slidingAnimationId = Animator.StringToHash("SprintingForwardRoll");
            jumpAnimationId = Animator.StringToHash("Jump");
            deathAnimationId = Animator.StringToHash("FallingBackDeath");
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
            if (!IsGrounded(20))
            {
                GameOver();
                return;
            }

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
            if (turnPosition.HasValue)
            {
                var targetDirection = Quaternion.AngleAxis(90 * context.ReadValue<float>(), Vector3.up) *
                                      movementDirection;
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
            if (!sliding && IsGrounded())
            {
                StartCoroutine(Slide());
            }
        }

        private void PlayerJump(InputAction.CallbackContext context)
        {
            if (!IsGrounded()) return;
            playerVelocity.y += Mathf.Sqrt(jumpHeight * gravity * -3f);
            animator.Play(jumpAnimationId);
        }

        private IEnumerator Slide()
        {
            sliding = true;
            // Shrink the collider
            Vector3 originalControllerCenter = controller.center;
            Vector3 newControllerCenter = originalControllerCenter;
            controller.height /= 2;
            newControllerCenter.y -= controller.height / 2;
            controller.center = newControllerCenter;
            // PLay the sliding animation
            animator.Play(slidingAnimationId);
            yield return new WaitForSeconds(0.9f);
            // Set the character controller collider back to normal after sliding.
            controller.height *= 2;
            controller.center = originalControllerCenter;
            sliding = false;
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

        private void GameOver()
        {
            Debug.Log("Game Over!");
            animator.Play(deathAnimationId);
            GameOverEvent?.Invoke();
            enabled = false;
            //gameObject.SetActive(false);
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
            raycastOriginFirst.y -= controller.height / 2f;
            raycastOriginFirst.y += .1f;

            var raycastOriginSecond = raycastOriginFirst;
            var forward = transform1.forward;
            raycastOriginFirst -= forward * .2f;
            raycastOriginSecond += forward * .2f;

            //Debug.DrawLine(raycastOriginFirst, raycastOriginFirst - Vector3.down * length, Color.green, 2f);
            //Debug.DrawLine(raycastOriginSecond, raycastOriginFirst - Vector3.down * length, Color.red, 2f);

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