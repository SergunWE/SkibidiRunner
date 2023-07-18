using System.Collections.Generic;
using SkibidiRunner.Managers;
using SkibidiRunner.Music;
using UnityEngine;
using YandexSDK.Scripts;

namespace SkibidiRunner.Map
{
    public class CurrentSetup : MonoBehaviourInitializable
    {
        [field:SerializeField] public GameMapSetup GameMapSetup { get; private set; }
        [SerializeField] protected List<GameMusic> gameMusics;
        
        public GameMusic CurrentMusic { get; protected set; }
        private int _currentMusicIndex;
        
        public override void Initialize()
        {
            _currentMusicIndex = LocalYandexData.Instance.SelectedMusic;
            CurrentMusic = gameMusics[_currentMusicIndex];
        }
    }
}