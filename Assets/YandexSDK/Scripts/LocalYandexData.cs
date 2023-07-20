﻿using System;
using System.Threading.Tasks;
using UnityEngine;

namespace YandexSDK.Scripts
{
    public class LocalYandexData : ICloudData
    {
        private static LocalYandexData _instance;
        public static LocalYandexData Instance => _instance ??= new LocalYandexData();
        
        public DateTime EndTimeSplashAdv { get; set; }
        public DateTime EndTime10Adv { get; set; }
        public DateTime EndTime30Adv { get; set; }
        public DateTime EndTimeLiveAdv { get; set; }

        public int BonusScore
        {
            get => _playerData.BonusScore;
            set
            {
                if(value <= _playerData.BonusScore) return;
                _playerData.BonusScore = value;
                SaveData();
            }
        }

        public float MusicVolume
        {
            get => _playerData.MusicVolume;
            set
            {
                if(value.Equals(_playerData.MusicVolume)) return;
                _playerData.MusicVolume = value;
                SaveData();
            }
        }

        public float SoundVolume
        {
            get => _playerData.SoundVolume;
            set
            {
                if(value.Equals(_playerData.SoundVolume)) return;
                _playerData.SoundVolume = value;
                SaveData();
            }
        }

        public int ScoreRecord
        {
            get => _playerData.ScoreRecord;
            set
            {
                if(value <= _playerData.ScoreRecord) return;
                _playerData.ScoreRecord = value;
                SaveData();
            }
        }

        public int SelectedMusic
        {
            get => _playerData.SelectedMusic;
            set
            {
                if(value == _playerData.SelectedMusic) return;
                _playerData.SelectedMusic = value;
                SaveData();
            }
        }
        
        public bool YandexDataLoaded { get; private set; }
        
        public event Action OnYandexDataLoaded;

        private readonly PlayerData _playerData;

        private LocalYandexData()
        {
            _playerData = new PlayerData
            {
                SoundVolume = 0.5f,
                MusicVolume = 0.5f
            };
        }
        
        public void SetPlayerData(PlayerData playerData)
        {
            YandexDataLoaded = true;
            if (playerData.LastSaveTimeTicks != 0)
            {
                BonusScore = playerData.BonusScore;
                MusicVolume = playerData.MusicVolume;
                SoundVolume = playerData.SoundVolume;
                ScoreRecord = playerData.ScoreRecord;
                SelectedMusic = playerData.SelectedMusic;
            }
            OnYandexDataLoaded?.Invoke();
        }

        private void SaveData()
        {
            _playerData.LastSaveTimeTicks = DateTime.UtcNow.Ticks;
            YandexGamesManager.SavePlayerData(_playerData);
        }
    }
}