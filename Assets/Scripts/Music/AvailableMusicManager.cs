using System.Collections.Generic;
using UnityEngine;
using YandexSDK.Scripts;

namespace SkibidiRunner.Music
{
    public class AvailableMusicManager : MonoBehaviour
    {
        [SerializeField] private List<MusicItem> musicItems;

        private MusicItem _selectedMusicItem;

        private void OnEnable()
        {
            CheckAvailableMusic();

            for (int i = 0; i < musicItems.Count; i++)
            {
                int index = i;
                musicItems[i].Button.onClick.AddListener(() => OnMusicButtonClicked(index));
            }

            LocalYandexData.Instance.OnYandexDataLoaded += CheckAvailableMusic;
        }

        private void OnDisable()
        {
            LocalYandexData.Instance.OnYandexDataLoaded -= CheckAvailableMusic;
            foreach (var t in musicItems)
            {
                t.Button.onClick.RemoveAllListeners();
            }
        }

        private void OnMusicButtonClicked(int index)
        {
            if (musicItems.IndexOf(_selectedMusicItem) == index) return;
            if (musicItems[index].RequiredNumberPoints >= LocalYandexData.Instance.ScoreRecord) return;
            LocalYandexData.Instance.SelectedMusic = index;
            _selectedMusicItem.Activate();
            _selectedMusicItem = musicItems[index];
            _selectedMusicItem.Select();
        }

        private void CheckAvailableMusic()
        {
            int currentRecord = LocalYandexData.Instance.ScoreRecord;
            foreach (var musicItem in musicItems)
            {
                if (currentRecord >= musicItem.RequiredNumberPoints)
                {
                    musicItem.Activate();
                }
                else
                {
                    musicItem.Deactivate();
                }
            }

            _selectedMusicItem = musicItems[LocalYandexData.Instance.SelectedMusic];
            _selectedMusicItem.Select();
        }
    }
}