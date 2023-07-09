using System;
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
            int currentRecord = LocalYandexData.Instance.ScoreRecord;
            for (int i = 0; i < musicItems.Count; i++)
            {
                if (currentRecord >= musicItems[i].RequiredNumberPoints)
                {
                    musicItems[i].Activate();
                }
                else
                {
                    musicItems[i].Deactivate();
                }

                int index = i;
                musicItems[i].Button.onClick.AddListener(() => OnMusicButtonClicked(index));
            }

            _selectedMusicItem = musicItems[LocalYandexData.Instance.SelectedMusic];
            _selectedMusicItem.Select();
        }

        private void OnDisable()
        {
            foreach (var t in musicItems)
            {
                t.Button.onClick.RemoveAllListeners();
            }
        }

        private void OnMusicButtonClicked(int index)
        {
            if(musicItems.IndexOf(_selectedMusicItem) == index) return;
            if(musicItems[index].RequiredNumberPoints <  LocalYandexData.Instance.ScoreRecord) return;
            LocalYandexData.Instance.SelectedMusic = index;
            _selectedMusicItem.Activate();
            _selectedMusicItem = musicItems[index];
            _selectedMusicItem.Select();
        }
    }
}