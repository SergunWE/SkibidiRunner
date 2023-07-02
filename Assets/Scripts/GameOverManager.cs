using UnityEngine;

namespace SkibidiRunner
{
    public class GameOverManager : MonoBehaviour
    {
        [SerializeField] private GameObject gameOverCanvas;

        public void StopGame()
        {
            gameOverCanvas.SetActive(true);
        }

        public void RestartLevel()
        {
        }

        public void SubmitScore()
        {
        }
    }
}