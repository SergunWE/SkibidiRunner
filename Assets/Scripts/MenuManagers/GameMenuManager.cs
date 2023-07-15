using SkibidiRunner.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SkibidiRunner.MenuManagers
{
    public class GameMenuManager : MonoBehaviour
    {
        [SerializeField] private SplashAdvManager splashAdvManager;

        public void RestartLevel()
        {
#if UNITY_EDITOR
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
#else
            if (!splashAdvManager.ShowAdv()) return;
            splashAdvManager.advEnded.AddListener(() =>
                SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex));
#endif
        }

        public void ExitGame()
        {
#if UNITY_EDITOR
            SceneManager.LoadSceneAsync(0);
#else
            if (!splashAdvManager.ShowAdv()) return;
            splashAdvManager.advEnded.AddListener(() => SceneManager.LoadSceneAsync(0));
#endif
        }
    }
}