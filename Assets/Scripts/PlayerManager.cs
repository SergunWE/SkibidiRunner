using UnityEngine;
using UnityEngine.Events;

namespace SkibidiRunner
{
    public class PlayerManager : MonoBehaviourInitializable
    {
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private Transform playerSpawnPosition;
        [SerializeField] private GameObject currentPlayerGameObject;
        
        [SerializeField] private UnityEvent<Vector3> turnEvent;

        private PlayerController _currentPlayerController;
        
        public override void Initialize()
        {
            if (currentPlayerGameObject == null)
            {
                currentPlayerGameObject = Instantiate(playerPrefab);
            }

            currentPlayerGameObject.transform.position = playerSpawnPosition.position;
            _currentPlayerController = currentPlayerGameObject.GetComponentInChildren<PlayerController>();
            _currentPlayerController.TurnEvent = OnPlayerTurned;
        }

        private void OnPlayerTurned(Vector3 targetDirection)
        {
            turnEvent?.Invoke(targetDirection);
        }
    }
}