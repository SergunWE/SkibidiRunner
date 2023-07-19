using SkibidiRunner.Managers;
using TMPro;
using UnityEngine;
using YandexSDK.Scripts;

namespace SkibidiRunner.UI
{
    public class RecordDisplay : MonoBehaviour
    {
        [SerializeField] private string ruText;
        [SerializeField] private string enText;
        [SerializeField] private string trText;
        [SerializeField] private TMP_Text recordText;

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
            string value = CurrentText() + " " + LocalYandexData.Instance.ScoreRecord;
            recordText.text = value;
        }
        
        private string CurrentText()
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