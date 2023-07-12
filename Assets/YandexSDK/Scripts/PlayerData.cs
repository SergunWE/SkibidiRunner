using System;

namespace YandexSDK.Scripts
{
    [Serializable]
    public class PlayerData
    {
        public float MusicVolume { get; set; }
        public float SoundVolume { get; set; }
        public int ScoreRecord { get; set; }
        public int SelectedMusic { get; set; }
        public bool NotFirst { get; set; }
    }
}