﻿using SkibidiRunner.GameMap;
using UnityEngine;

namespace SkibidiRunner
{
    public class DifficultManager : MonoBehaviourInitializable
    {
        [SerializeField] private GameMapSetup gameMapSetup;
        private float _speedChangeRate;
        
        public override void Initialize()
        {
            Time.timeScale = gameMapSetup.InitialSpeedTime;
            _speedChangeRate = gameMapSetup.IncreaseSpeedTime;
        }

        public void IncreaseTheDifficulty()
        {
            Time.timeScale += _speedChangeRate;
        }

        public void GameOver()
        {
            Time.timeScale = gameMapSetup.InitialSpeedTime;
        }
    }
}