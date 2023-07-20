using SkibidiRunner.Managers;
using UnityEngine;

namespace SkibidiRunner.UI
{
    public class ContinueButton : MonoBehaviour
    {
        [SerializeField] private PauseManager pauseManager;

        private void OnEnable()
        {
            pauseManager.enabled = false;
        }

        private void OnDisable()
        {
            pauseManager.enabled = true;
            pauseManager.ResumeGame();
        }
    }
}