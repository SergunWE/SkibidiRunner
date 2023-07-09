using UnityEngine;
using YandexSDK.Scripts;

namespace SkibidiRunner.Managers
{
    public class SoundManager : MonoBehaviourInitializable
    {
        [SerializeField] private AudioSource audioSource;

        private float _volume;
        
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