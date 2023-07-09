using SkibidiRunner.Map;
using UnityEngine;

namespace SkibidiRunner.Managers
{
    public class GameSpeedManager : MonoBehaviourInitializable
    {
        [SerializeField] private CurrentSetup gameMapSetup;
        private float _speedChangeRate;
        
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
            Time.timeScale = gameMapSetup.GameMapSetup.InitialSpeedTime;
        }
    }
}