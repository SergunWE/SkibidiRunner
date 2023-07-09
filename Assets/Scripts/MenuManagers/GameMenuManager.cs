using UnityEngine;
using UnityEngine.SceneManagement;

namespace SkibidiRunner.MenuManagers
{
    public class GameMenuManager : MonoBehaviour
    {
        public void RestartLevel()
        {
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        }

        public void ExitGame()
        {
            SceneManager.LoadSceneAsync(0);
        }
    }
}