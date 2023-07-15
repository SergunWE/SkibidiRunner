using System;
using UnityEngine;
using YandexSDK.Scripts;

namespace SkibidiRunner.Managers
{
    public class Rewarded50AdvButton : AdvButton
    {
        protected override bool AdsAvailable()
        {
            return DateTime.UtcNow - LocalYandexData.Instance.EndTime50Adv > TimeSpan.FromSeconds(delaySeconds);
        }

        protected override TimeSpan TimeBeforeAccess()
        {
            return TimeSpan.FromSeconds(delaySeconds) - (DateTime.UtcNow - LocalYandexData.Instance.EndTime50Adv);
        }
        
        public void ShowAdv()
        {
            if (DateTime.UtcNow < LocalYandexData.Instance.EndTime50Adv) return;
            advStarted?.Invoke();
            YandexGamesManager.ShowRewardedAdv(gameObject.name, nameof(OnAdvShowed));
        }
        
        public void OnAdvShowed(int result)
        {
            if (result == 1)
            {
                LocalYandexData.Instance.EndTime50Adv = DateTime.UtcNow + TimeSpan.FromSeconds(delaySeconds);
            }
            advEnded?.Invoke();
        }
    }
}