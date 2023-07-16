using System;
using YandexSDK.Scripts;

namespace SkibidiRunner.Managers
{
    public class Rewarded10AdvButton : AdvButton
    {
        protected override bool AdsAvailable()
        {
            return DateTime.UtcNow - LocalYandexData.Instance.EndTime10Adv > TimeSpan.Zero;
        }

        protected override string TextBeforeAccess()
        {
            return (LocalYandexData.Instance.EndTime10Adv - DateTime.UtcNow)
                .ToString(@"mm\:ss");
        }

        public override void ShowAdv()
        {
            if (DateTime.UtcNow < LocalYandexData.Instance.EndTime10Adv) return;
#if UNITY_EDITOR
            LocalYandexData.Instance.EndTime10Adv = DateTime.UtcNow + TimeSpan.FromSeconds(delaySeconds);
            LocalYandexData.Instance.BonusScore += 10;
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
                    LocalYandexData.Instance.EndTime10Adv = DateTime.UtcNow + TimeSpan.FromSeconds(delaySeconds);
                    LocalYandexData.Instance.BonusScore += 10;
                    break;
                case 2:
                    advEnded?.Invoke();
                    break;
            }
        }
    }
}