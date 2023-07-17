using SkibidiRunner.Map;
using UnityEngine;

namespace SkibidiRunner.Managers
{
    public class GameSpeedManager : MonoBehaviourInitializable
    {
        [SerializeField] private CurrentSetup gameMapSetup;
        [SerializeField] private PauseManager pauseManager;
        private float _speedChangeRate;
        private float _currentSpeed;

        public override void Initialize()
        {
            var setup = gameMapSetup.GameMapSetup;
            Time.timeScale = setup.InitialSpeedTime;
            _speedChangeRate = setup.IncreaseSpeedTime;
        }

        public void IncreaseSpeed()
        {
            Time.timeScale += _speedChangeRate;
        }

        public void GameOver()
        {
            _currentSpeed = Time.timeScale;
            Time.timeScale = gameMapSetup.GameMapSetup.InitialSpeedTime;
        }

        public void OnLifeGained()
        {
            _currentSpeed /= 2;
            pauseManager.SetNewTimeScale(_currentSpeed);
        }
    }
}