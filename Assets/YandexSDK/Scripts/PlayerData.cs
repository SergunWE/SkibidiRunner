using System;

namespace YandexSDK.Scripts
{
    [Serializable]
    public class PlayerData
    {
        public float MusicVolume;
        public float SoundVolume;
        public int ScoreRecord;
        public int SelectedMusic;
        public int BonusScore;
        public long LastSaveTimeTicks;
    }
}