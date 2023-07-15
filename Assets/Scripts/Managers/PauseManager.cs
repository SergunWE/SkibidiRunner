using UnityEngine;

namespace SkibidiRunner.Managers
{
    public class PauseManager : MonoBehaviour
    {
        private float _timeScale;
        
        public void PauseGame()
        {
            _timeScale = Time.timeScale;
            Time.timeScale = 0;
            AudioListener.pause = true;
        }

        public void ResumeGame()
        {
            Time.timeScale = _timeScale;
            AudioListener.pause = false;
        }
    }
}