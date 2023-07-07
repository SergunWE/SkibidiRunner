using System.Collections;
using SkibidiRunner.GameMap;
using UnityEngine;
using UnityEngine.Events;

namespace SkibidiRunner
{
    public class ScoreManager : MonoBehaviourInitializable
    {
        [SerializeField] private GameMapSetup gameMapSetup;
        [SerializeField] private UnityEvent<int> scoreEvent;
        
        private float _scoreMultiplier;
        private float _score;

        public override void Initialize()
        {
            _scoreMultiplier = gameMapSetup.ScoreMultiplier;
            StopAllCoroutines();
            StartCoroutine(ScoreMeter());
        }

        public void GameOver()
        {
            StopAllCoroutines();
            scoreEvent?.Invoke((int)_score);
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