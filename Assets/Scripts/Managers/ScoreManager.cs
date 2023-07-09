﻿using System.Collections;
using SkibidiRunner.Map;
using UnityEngine;
using UnityEngine.Events;

namespace SkibidiRunner.Managers
{
    public class ScoreManager : MonoBehaviourInitializable
    {
        [SerializeField] private CurrentSetup gameMapSetup;
        [SerializeField] private UnityEvent<int> scoreEvent;
        
        private float _scoreMultiplier;
        private float _score;

        public override void Initialize()
        {
            _scoreMultiplier = gameMapSetup.GameMapSetup.ScoreMultiplier;
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