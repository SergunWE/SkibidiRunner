using System;
using UnityEngine;
using YandexSDK.Scripts;

namespace SkibidiRunner.Managers
{
    public class SoundManager : MonoBehaviourInitializable
    {
        [SerializeField] private AudioSource audioSource;

        private float _volume;

        private void OnEnable()
        {
            LocalYandexData.Instance.OnYandexDataLoaded += Initialize;
        }
        
        private void OnDisable()
        {
            LocalYandexData.Instance.OnYandexDataLoaded -= Initialize;
        }

        public override void Initialize()
        {
            _volume = LocalYandexData.Instance.SoundVolume;
            audioSource.volume = _volume;
        }

        public void PlaySound(AudioClip audioClip)
        {
            audioSource.PlayOneShot(audioClip);
        }
    }
}