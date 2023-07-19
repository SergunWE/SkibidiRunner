using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using YandexSDK.Scripts;

namespace SkibidiRunner.UI
{
    [RequireComponent(typeof(TMP_Text))]
    public class TranslatedText : MonoBehaviour
    {
        [SerializeField] private string ruText;
        [SerializeField] private string enText;
        [SerializeField] private string trText;
        [SerializeField] private TMP_Text text;
        
        private void Start()
        {
            SetText();
        }

        public void SetText(string newText = null)
        {
            text.text = string.IsNullOrEmpty(newText) ? CurrentText() : newText;
        }

        public string CurrentText()
        {
            switch (YandexGamesManager.GetLanguage())
            {
                case Language.Russian:
                    return ruText;
                case Language.Turkey:
                    return trText;
                case Language.English:
                default:
                    return enText;
            }
        }
    }
}