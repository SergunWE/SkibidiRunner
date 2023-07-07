using Cinemachine;
using UnityEngine;
using UnityEngine.Events;

namespace SkibidiRunner
{
    public class PlayerManager : MonoBehaviourInitializable
    {
        [SerializeField] private Transform playerSpawnPosition;
        [SerializeField] private GameObject currentPlayerGameObject;
        [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;

        [SerializeField] private UnityEvent turnEvent;
        [SerializeField] private UnityEvent gameOverEvent;

        private PlayerController _currentPlayerController;

        public override void Initialize()
        {
            currentPlayerGameObject.transform.position = playerSpawnPosition.position;
            cinemachineVirtualCamera.Follow = currentPlayerGameObject.transform;
            cinemachineVirtualCamera.LookAt = currentPlayerGameObject.transform;
            _currentPlayerController = currentPlayerGameObject.GetComponentInChildren<PlayerController>();

            _currentPlayerController.TurnEvent = turnEvent;
            _currentPlayerController.GameOverEvent = gameOverEvent;
        }

        public void OnTilesAdded(Vector3 playerPosition)
        {
            _currentPlayerController.ChangePosition(playerPosition + Vector3.up * playerSpawnPosition.position.y);
        }
    }
}