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

        private void Awake()
        {
            switch (YandexGamesManager.GetLanguage())
            {
                case Language.Russian:
                    GetComponent<TMP_Text>().text = ruText;
                    break;
                case Language.Turkey:
                    GetComponent<TMP_Text>().text = trText;
                    break;
                case Language.English:
                default:
                    GetComponent<TMP_Text>().text =enText;
                    break;
            }
        }
    }
}