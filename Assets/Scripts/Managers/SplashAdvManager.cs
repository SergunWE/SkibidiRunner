using System;
using UnityEngine;
using UnityEngine.Events;
using YandexSDK.Scripts;

namespace SkibidiRunner.Managers
{
    public class SplashAdvManager : MonoBehaviourInitializable
    {
        [SerializeField] private int delaySeconds;
        [SerializeField] private bool showStartup;

        [SerializeField] private UnityEvent advStarted;
        [SerializeField] private UnityEvent advEnded;
        
        public override void Initialize()
        {
            if (showStartup)
            {
                ShowAdv();
            }
        }

        public void ShowAdv()
        {
            if (DateTime.UtcNow < LocalYandexData.Instance.EndTimeSplashAdv) return;
            advStarted?.Invoke();
            YandexGamesManager.ShowSplashAdv(gameObject.name, nameof(OnAdvShowed));
        }

        public void OnAdvShowed(int result)
        {
            if (result == 1)
            {
                LocalYandexData.Instance.EndTimeSplashAdv = DateTime.UtcNow + TimeSpan.FromSeconds(delaySeconds);
            }
            advEnded?.Invoke();
        }
    }
}