using SkibidiRunner.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SkibidiRunner.Managers
{
    public class AdvButton : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] protected int delaySeconds;
        [SerializeField] protected UnityEvent advStarted;
        [SerializeField] protected UnityEvent advEnded;
        
        private TranslatedText _translatedText;

        private void Awake()
        {
            _translatedText = button.GetComponentInChildren<TranslatedText>();
            button.onClick.AddListener(ShowAdv);
        }

        private void FixedUpdate()
        {
            if (AdsAvailable())
            {
                if(button.interactable) return;
                button.interactable = true;
                _translatedText.SetText();
            }
            else
            {
                button.interactable = false;
                _translatedText.SetText(TextBeforeAccess());
            }
        }

        protected virtual bool AdsAvailable()
        {
            return true;
        }

        protected virtual string TextBeforeAccess()
        {
            return "";
        }

        public virtual void ShowAdv()
        {
        }
    }
}