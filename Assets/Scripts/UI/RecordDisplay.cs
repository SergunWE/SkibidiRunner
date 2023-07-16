using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using YandexSDK.Scripts;

namespace SkibidiRunner.UI
{
    public class RecordDisplay : MonoBehaviour
    {
        [SerializeField] private TranslatedText translatedText;

        private void Start()
        {
            string value = translatedText.CurrentText() + " " + LocalYandexData.Instance.ScoreRecord;
            translatedText.SetText(value);
        }
    }
}