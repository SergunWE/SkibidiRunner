using System;
using SkibidiRunner.UI;
using UnityEngine;
using UnityEngine.UI;

namespace SkibidiRunner.Managers
{
    [RequireComponent(typeof(Button))]
    public class AdvButton : MonoBehaviour
    {
        [SerializeField] protected int delaySeconds;
        
        private Button _button;
        private TranslatedText _translatedText;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _translatedText = GetComponentInChildren<TranslatedText>();
        }

        private void Update()
        {
            if (AdsAvailable())
            {
                if(_button.interactable) return;
                _button.interactable = true;
                _translatedText.SetText();
            }
            else
            {
                _button.interactable = false;
                _translatedText.Text.text = TimeBeforeAccess().ToString(@"mm\:ss");
            }
        }

        protected virtual bool AdsAvailable()
        {
            return true;
        }

        protected virtual TimeSpan TimeBeforeAccess()
        {
            return TimeSpan.Zero;
        }
    }
}