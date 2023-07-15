using System;
using System.Collections;
using SkibidiRunner.Map;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using YandexSDK.Scripts;

namespace SkibidiRunner.Managers
{
    public class ScoreManager : MonoBehaviourInitializable
    {
        [SerializeField] private CurrentSetup gameMapSetup;
        [SerializeField] private UnityEvent<int> scoreEvent;

        [SerializeField] private TMP_Text gameOverText;
        
        private float _scoreMultiplier;
        private float _score;

        public override void Initialize()
        {
            _scoreMultiplier = gameMapSetup.GameMapSetup.ScoreMultiplier;
            if (DateTime.UtcNow < LocalYandexData.Instance.EndTime30Adv)
            {
                _scoreMultiplier += _scoreMultiplier * 0.3f;
            }
            _score = LocalYandexData.Instance.BonusScore;
            StopAllCoroutines();
            StartCoroutine(ScoreMeter());
        }

        public void GameOver()
        {
            StopAllCoroutines();
            int score = (int) _score;
            gameOverText.text = score.ToString();
            LocalYandexData.Instance.ScoreRecord = score;
            YandexGamesManager.SetToLeaderboard(score);
        }

        public void OnLifeGained()
        {
            StartCoroutine(ScoreMeter());
        }

        private IEnumerator ScoreMeter()
        {
            while (true)
            {
                _score += _scoreMultiplier * Time.deltaTime;
                scoreEvent?.Invoke((int)_score);
                yield return null;
            }
            // ReSharper disable once IteratorNeverReturns
        }
    }
}