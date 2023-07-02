using UnityEngine;

namespace SkibidiRunner
{
    public class DifficultManager : MonoBehaviour
    {
        [SerializeField] private float speedChangeRate;

        public void IncreaseTheDifficulty()
        {
            Time.timeScale += speedChangeRate;
        }

        public void GameOver()
        {
            Time.timeScale = 1;
        }
    }
}