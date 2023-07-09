namespace YandexSDK.Scripts
{
    public class LocalYandexData
    {
        private static LocalYandexData _instance;
        
        public static LocalYandexData Instance => _instance ??= new LocalYandexData();

        public float MusicVolume
        {
            get => _musicVolume;
            set
            {
                _musicVolume = value;
                SaveData();
            }
        }

        public float SoundVolume
        {
            get => _soundVolume;
            set
            {
                _soundVolume = value;
                SaveData();
            }
        }

        public int ScoreRecord
        {
            get => _scoreRecord;
            set
            {
                _scoreRecord = value;
                SaveData();
            }
        }

        public int SelectedMusic
        {
            get => _selectedMusic;
            set
            {
                _selectedMusic = value;
                SaveData();
            }
        }

        private float _musicVolume;
        private float _soundVolume;
        private int _scoreRecord;
        private int _selectedMusic;

        private LocalYandexData()
        {
            MusicVolume = 1f;
            SoundVolume = 1f;
            ScoreRecord = 0;
            SelectedMusic = 0;
        }
        
        private void SaveData()
        {
        
        }
    }
    
    
}