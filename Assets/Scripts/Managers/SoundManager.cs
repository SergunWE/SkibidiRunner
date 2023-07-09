using UnityEngine;

namespace SkibidiRunner.Managers
{
    public class SoundManager : MonoBehaviourInitializable
    {
        [SerializeField] private AudioSource audioSource;

        private float _volume;
        
        public override void Initialize()
        {
            //todo get volume in yandexSDK
            _volume = 1f;

            audioSource.volume = _volume;
        }

        public void PlaySound(AudioClip audioClip)
        {
            audioSource.PlayOneShot(audioClip);
        }
    }
}