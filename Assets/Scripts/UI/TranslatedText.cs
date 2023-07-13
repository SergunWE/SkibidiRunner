using TMPro;
using UnityEngine;
using YandexSDK.Scripts;

namespace SkibidiRunner.UI
{
    [RequireComponent(typeof(TMP_Text))]
    public class TranslatedText : MonoBehaviour
    {
        [SerializeField] private string ruText;
        [SerializeField] private string enText;
        [SerializeField] private string trText;

        public TMP_Text Text { get; private set; }
        
        private void Awake()
        {
            Text = GetComponent<TMP_Text>();
            SetText();
        }

        public void SetText()
        {
            switch (YandexGamesManager.GetLanguage())
            {
                case Language.Russian:
                    Text.text = ruText;
                    break;
                case Language.Turkey:
                    Text.text = trText;
                    break;
                case Language.English:
                default:
                    Text.text =enText;
                    break;
            }
        }
    }
}