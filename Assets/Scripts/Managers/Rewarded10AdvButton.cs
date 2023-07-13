using System;
using YandexSDK.Scripts;

namespace SkibidiRunner.Managers
{
    public class Rewarded10AdvButton : AdvButton
    {
        protected override bool AdsAvailable()
        {
            return DateTime.UtcNow - LocalYandexData.Instance.EndTime10Adv > TimeSpan.FromSeconds(delaySeconds);
        }

        protected override TimeSpan TimeBeforeAccess()
        {
            return TimeSpan.FromSeconds(delaySeconds) - (DateTime.UtcNow - LocalYandexData.Instance.EndTime10Adv);
        }

        public void ShowAdv()
        {
            if (DateTime.UtcNow < LocalYandexData.Instance.EndTime10Adv) return;
            YandexGamesManager.ShowRewardedAdv();
            LocalYandexData.Instance.EndTime10Adv = DateTime.UtcNow + TimeSpan.FromSeconds(delaySeconds);
            LocalYandexData.Instance.BonusScore += 10;
        }
    }
}