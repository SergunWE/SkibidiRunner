using System;
using UnityEngine;
using YandexSDK.Scripts;

namespace SkibidiRunner.Managers
{
    public class SplashAdvManager : MonoBehaviourInitializable
    {
        [SerializeField] private int delaySeconds;
        [SerializeField] private bool showStartup;
        
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
            YandexGamesManager.ShowSplashAdv();
            LocalYandexData.Instance.EndTimeSplashAdv = DateTime.UtcNow + TimeSpan.FromSeconds(delaySeconds);
        }
    }
}