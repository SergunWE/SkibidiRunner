using SkibidiRunner.Managers;
using UnityEngine;
using YandexSDK.Scripts;

namespace SkibidiRunner.UI
{
    public class RecordDisplay : MonoBehaviour
    {
        [SerializeField] private TranslatedText translatedText;

        private void OnEnable()
        {
            LocalYandexData.Instance.OnYandexDataLoaded += Start;
        }

        private void OnDisable()
        {
            LocalYandexData.Instance.OnYandexDataLoaded -= Start;
        }

        private void Start()
        {
            string value = translatedText.CurrentText() + " " + LocalYandexData.Instance.ScoreRecord;
            translatedText.SetText(value);
        }
    }
}