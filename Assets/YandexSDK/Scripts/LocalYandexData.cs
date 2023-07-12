namespace YandexSDK.Scripts
{
    public class LocalYandexData : ICloudData
    {
        private static LocalYandexData _instance;

        public static LocalYandexData Instance => _instance ??= new LocalYandexData();

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
            _playerData = YandexGamesManager.LoadPlayerData();
            if (_playerData.NotFirst) return;
            _playerData.MusicVolume = 1f;
            _playerData.SoundVolume = 1f;
            _playerData.NotFirst = true;
        }

        private void SaveData()
        {
            YandexGamesManager.SavePlayerData(_playerData);
        }
    }
}