using System;

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
                _playerData.BonusScore = value;
                SaveData();
            }
        }

        public float MusicVolume
        {
            get => _playerData.MusicVolume;
            set
            {
                _playerData.MusicVolume = value;
                SaveData();
            }
        }

        public float SoundVolume
        {
            get => _playerData.SoundVolume;
            set
            {
                _playerData.SoundVolume = value;
                SaveData();
            }
        }

        public int ScoreRecord
        {
            get => _playerData.ScoreRecord;
            set
            {
                if(value <=  _playerData.ScoreRecord) return;
                _playerData.ScoreRecord = value;
                SaveData();
            }
        }

        public int SelectedMusic
        {
            get => _playerData.SelectedMusic;
            set
            {
                _playerData.SelectedMusic = value;
                SaveData();
            }
        }

        private readonly PlayerData _playerData;

        private LocalYandexData()
        {
            _playerData = YandexGamesManager.LoadPlayerData() ?? new PlayerData
            {
                MusicVolume = 0.5f,
                SoundVolume = 0.5f
            };
        }

        private void SaveData()
        {
            YandexGamesManager.SavePlayerData(_playerData);
        }
    }
}