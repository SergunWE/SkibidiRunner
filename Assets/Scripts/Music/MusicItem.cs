using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YandexSDK.Scripts;

namespace SkibidiRunner.Music
{
    public class MusicItem : MonoBehaviour
    {
        [field:SerializeField] public int RequiredNumberPoints { get; private set; }
        [field:SerializeField] public Button Button { get; private set; }
        [SerializeField] private TMP_Text buttonText;

        private static readonly Dictionary<Language, string[]> Texts = new()
        {
            {Language.Russian, new[] {"Выбрать", "Набери", "Выбрано"}},
            {Language.English, new[] {"Select", "Dial it up", "Selected"}},
            {Language.Turkey, new[] {"Seçiniz", "Çevirin", "Seçilmiş"}}
        };
        
        private readonly Language _language = YandexGamesManager.GetLanguage();

        public void Activate()
        {
            buttonText.text = Texts[_language][0];
            Button.interactable = true;
        }

        public void Deactivate()
        {
            buttonText.text = $"{Texts[_language][1]} {RequiredNumberPoints}";
            Button.interactable = false;
        }

        public void Select()
        {
            buttonText.text = Texts[_language][2];
        }
    }
}