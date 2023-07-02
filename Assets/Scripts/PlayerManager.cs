using Cinemachine;
using UnityEngine;
using UnityEngine.Events;

namespace SkibidiRunner
{
    public class PlayerManager : MonoBehaviourInitializable
    {
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private Transform playerSpawnPosition;
        [SerializeField] private GameObject currentPlayerGameObject;
        [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
        
        [SerializeField] private UnityEvent<Vector3> turnEvent;
        [SerializeField] private UnityEvent gameOverEvent;

        private PlayerController _currentPlayerController;
        
        public override void Initialize()
        {
            if (currentPlayerGameObject == null)
            {
                currentPlayerGameObject = Instantiate(playerPrefab);
            }

            currentPlayerGameObject.transform.position = playerSpawnPosition.position;
            cinemachineVirtualCamera.Follow = currentPlayerGameObject.transform;
            cinemachineVirtualCamera.LookAt = currentPlayerGameObject.transform;
            _currentPlayerController = currentPlayerGameObject.GetComponentInChildren<PlayerController>();
            
            _currentPlayerController.TurnEvent = OnPlayerTurned;
            _currentPlayerController.GameOverEvent = OnGameOver;
        }

        private void OnPlayerTurned(Vector3 targetDirection)
        {
            turnEvent?.Invoke(targetDirection);
        }

        private void OnGameOver()
        {
            gameOverEvent?.Invoke();
        }
    }
}