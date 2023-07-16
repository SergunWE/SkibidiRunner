using System.Diagnostics;
using Cinemachine;
using SkibidiRunner.Player;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using Debug = UnityEngine.Debug;

namespace SkibidiRunner.Managers
{
    public class PlayerManager : MonoBehaviourInitializable
    {
        [SerializeField] private Transform playerSpawnPosition;
        [SerializeField] private GameObject currentPlayerGameObject;
        [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;

        [SerializeField] private UnityEvent turnEvent;
        [SerializeField] private UnityEvent gameOverEvent;
        [SerializeField] private UnityEvent jumpEvent;
        [SerializeField] private UnityEvent slidingEvent;

        private PlayerController _currentPlayerController;

        private readonly Vector3 _offset = new(0, 4, -4);
        private Quaternion _rotation;

        public override void Initialize()
        {
            var stopwatch = Stopwatch.StartNew();
            currentPlayerGameObject.transform.position = playerSpawnPosition.position;
            cinemachineVirtualCamera.Follow = currentPlayerGameObject.transform;
            cinemachineVirtualCamera.LookAt = currentPlayerGameObject.transform;
            _currentPlayerController = currentPlayerGameObject.GetComponentInChildren<PlayerController>();
            _currentPlayerController.TurnEvent = turnEvent;
            _currentPlayerController.GameOverEvent = gameOverEvent;
            _currentPlayerController.JumpEvent = jumpEvent;
            _currentPlayerController.SlidingEvent = slidingEvent;
            _currentPlayerController.Awake();
            _rotation = Quaternion.Euler(-180, 0, 0);
            Debug.Log($"{nameof(PlayerManager)} init {stopwatch.ElapsedMilliseconds}ms");
        }

        public void OnTilesAdded(Vector3 playerPosition)
        {
            _currentPlayerController.ChangePosition(playerPosition + Vector3.up * playerSpawnPosition.position.y);
        }

        public void OnLifeGained()
        {
            cinemachineVirtualCamera.Follow = currentPlayerGameObject.transform;
            cinemachineVirtualCamera.LookAt = currentPlayerGameObject.transform;
        }
    }
}