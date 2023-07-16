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
                if(value == _playerData.BonusScore) return;
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
                if(value == _playerData.SelectedMusic) return;
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