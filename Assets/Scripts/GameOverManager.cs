using UnityEngine;
using UnityEngine.SceneManagement;

namespace SkibidiRunner
{
    public class GameOverManager : MonoBehaviour
    {
        public void StopGame()
        {
            
        }

        public void RestartLevel()
        {
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        }

        public void SubmitScore()
        {
        }
    }
}