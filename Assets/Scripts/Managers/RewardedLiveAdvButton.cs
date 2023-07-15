using System;
using UnityEngine;
using UnityEngine.Events;
using YandexSDK.Scripts;

namespace SkibidiRunner.Managers
{
    public class RewardedLiveAdvButton : AdvButton
    {
        [SerializeField] private int maximumNumberLives = 1;
        [SerializeField] private string unavailableRuText;
        [SerializeField] private string unavailableEnText;
        [SerializeField] private string unavailableTrText;
        [SerializeField] private UnityEvent lifeReceived;

        private int _currentLifeNumber;

        protected override bool AdsAvailable()
        {
            return DateTime.UtcNow - LocalYandexData.Instance.EndTimeLiveAdv > TimeSpan.FromSeconds(delaySeconds) &&
                   _currentLifeNumber < maximumNumberLives;
        }

        protected override string TextBeforeAccess()
        {
            return AdsAvailable() ? null : GetUnavailableText();
        }

        public override void ShowAdv()
        {
            if (DateTime.UtcNow < LocalYandexData.Instance.EndTimeLiveAdv) return;
#if UNITY_EDITOR
            LocalYandexData.Instance.EndTimeLiveAdv = DateTime.UtcNow + TimeSpan.FromSeconds(delaySeconds);
            _currentLifeNumber++;
            lifeReceived?.Invoke();
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
                    LocalYandexData.Instance.EndTimeLiveAdv = DateTime.UtcNow + TimeSpan.FromSeconds(delaySeconds);
                    _currentLifeNumber++;
                    lifeReceived?.Invoke();
                    break;
                case 2:
                    advEnded?.Invoke();
                    break;
            }
        }
        
        private string GetUnavailableText()
        {
            switch (YandexGamesManager.GetLanguage())
            {
                case Language.Russian:
                    return unavailableRuText;
                case Language.Turkey:
                    return unavailableTrText;
                case Language.English:
                default:
                    return unavailableEnText;
            }
        }
    }
}