using System;
using UnityEngine;

namespace SkibidiRunner.Managers
{
    public class PauseManager : MonoBehaviour
    {
        private float _timeScale;

        private void Awake()
        {
            _timeScale = Time.timeScale;
        }

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