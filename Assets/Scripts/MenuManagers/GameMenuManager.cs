using SkibidiRunner.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;
using YandexSDK.Scripts;

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
            YandexGamesManager.RequestReviewGame();
            SceneManager.LoadSceneAsync(0);
        }
    }
}