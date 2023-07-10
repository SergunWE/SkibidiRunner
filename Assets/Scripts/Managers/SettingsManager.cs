using UnityEngine;
using UnityEngine.UI;
using YandexSDK.Scripts;

namespace SkibidiRunner.Managers
{
    public class SettingsManager : MonoBehaviour
    {
        [SerializeField] private Slider musicSlider;
        [SerializeField] private Slider soundSlider;

        private void OnEnable()
        {
            musicSlider.value = LocalYandexData.Instance.MusicVolume;
            soundSlider.value = LocalYandexData.Instance.SoundVolume;
        }

        private void OnDisable()
        {
             LocalYandexData.Instance.MusicVolume = musicSlider.value;
             LocalYandexData.Instance.SoundVolume = soundSlider.value;
        }
    }
}