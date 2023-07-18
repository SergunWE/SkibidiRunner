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
            SetSliders();
            LocalYandexData.Instance.OnYandexDataLoaded += SetSliders;
        }

        private void OnDisable()
        {
            LocalYandexData.Instance.OnYandexDataLoaded -= SetSliders;
            LocalYandexData.Instance.MusicVolume = musicSlider.value;
            LocalYandexData.Instance.SoundVolume = soundSlider.value;
        }

        private void SetSliders()
        {
            musicSlider.value = LocalYandexData.Instance.MusicVolume;
            soundSlider.value = LocalYandexData.Instance.SoundVolume;
        }
    }
}