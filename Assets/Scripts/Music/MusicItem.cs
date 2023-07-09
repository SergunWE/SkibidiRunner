using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace SkibidiRunner.Music
{
    public class MusicItem : MonoBehaviour
    {
        [field:SerializeField] public int RequiredNumberPoints { get; private set; }
        [field:SerializeField] public Button Button { get; private set; }
        [SerializeField] private TMP_Text buttonText;

        public void Activate()
        {
            buttonText.text = "Выбрать";
            Button.interactable = true;
        }

        public void Deactivate()
        {
            buttonText.text = $"Набери {RequiredNumberPoints}";
            Button.interactable = false;
        }

        public void Select()
        {
            buttonText.text = "Выбрано";
        }
    }
}