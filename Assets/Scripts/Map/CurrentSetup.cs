using System.Collections.Generic;
using SkibidiRunner.Managers;
using SkibidiRunner.Music;
using UnityEngine;

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
            //todo get in YandexSDK
            _currentMusicIndex = 1;
            
            CurrentMusic = gameMusics[_currentMusicIndex];
        }
    }
}