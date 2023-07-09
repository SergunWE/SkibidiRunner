using UnityEngine;
using UnityEngine.SceneManagement;

namespace SkibidiRunner.MenuManagers
{
    public class MainMenuManager : MonoBehaviour
    {
        public void StartGame()
        {
            SceneManager.LoadSceneAsync(sceneBuildIndex: 1);
        }
    }
}