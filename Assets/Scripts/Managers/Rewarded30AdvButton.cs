using System;
using UnityEngine;
using YandexSDK.Scripts;

namespace SkibidiRunner.Managers
{
    public class Rewarded30AdvButton : AdvButton
    {
        protected override bool AdsAvailable()
        {
            return DateTime.UtcNow - LocalYandexData.Instance.EndTime30Adv > TimeSpan.Zero;
        }

        protected override string TextBeforeAccess()
        {
            return (LocalYandexData.Instance.EndTime30Adv - DateTime.UtcNow)
                .ToString(@"mm\:ss");
        }

        public override void ShowAdv()
        {
            if (DateTime.UtcNow < LocalYandexData.Instance.EndTime30Adv) return;
#if UNITY_EDITOR
            LocalYandexData.Instance.EndTime30Adv = DateTime.UtcNow + TimeSpan.FromSeconds(delaySeconds);
#else
            YandexGamesManager.ShowRewardedAdv(gameObject.name, nameof(OnAdvShowed));
#endif
        }

        public void OnAdvShowed(int result)
        {
            switch (result)
            {
                case 0:
                    advStarted?.Invoke();
                    break;
                case 1:
                    LocalYandexData.Instance.EndTime30Adv = DateTime.UtcNow + TimeSpan.FromSeconds(delaySeconds);
                    break;
                case 2:
                    advEnded?.Invoke();
                    break;
            }
        }
    }
}