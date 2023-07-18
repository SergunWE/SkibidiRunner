using System.Collections.Generic;
using System.Diagnostics;
using SkibidiRunner.Managers;
using SkibidiRunner.Map;
using SkibidiRunner.UI;
using UnityEngine;
using UnityEngine.Events;
using YandexSDK.Scripts;
using Debug = UnityEngine.Debug;

namespace SkibidiRunner.Music
{
    public class Conductor : MonoBehaviourInitializable
    {
        [SerializeField] private CurrentSetup gameMusic;
        [SerializeField] private AudioSource musicSource;

        private float _songPositionInBeats;

        [SerializeField] private UnityEvent beatEvent;
        [SerializeField] private List<BeatSplashes> beatSplashesList;
        
        private float _songBpm;
        private float _secPerBeat = float.MaxValue;
        private float _songPosition;
        private int _lastBeat;
        
        private float _dspSongTime;

        private bool _init;
        private float _initDsp;

        private void OnEnable()
        {
            LocalYandexData.Instance.OnYandexDataLoaded += OnYandexDataLoaded;
        }
        
        private void OnDisable()
        {
            LocalYandexData.Instance.OnYandexDataLoaded -= OnYandexDataLoaded;
        }

        public override void Initialize()
        {
            var stopwatch = Stopwatch.StartNew();
            var music = gameMusic.CurrentMusic;
            _songBpm = music.SongBpm;
            _secPerBeat = 60f / _songBpm;
            musicSource.clip = music.Song;
            musicSource.volume = LocalYandexData.Instance.MusicVolume;
            musicSource.Play();
            Debug.Log($"{nameof(Conductor)} init {stopwatch.ElapsedMilliseconds}ms");
        }
        
        private void Update()
        {
            if (!_init)
            {
                _dspSongTime = (float)AudioSettings.dspTime - gameMusic.CurrentMusic.FirstBeatOffset;
                _init = true;
            }
            else
            {
                _songPosition = (float)(AudioSettings.dspTime - _dspSongTime);
                _songPositionInBeats = _songPosition / _secPerBeat;
                if (_lastBeat >= (int) _songPositionInBeats) return;
                _lastBeat = (int) _songPositionInBeats;
                beatEvent?.Invoke();
                foreach (var beat in beatSplashesList)
                {
                    beat.OnBeat();
                }
            }
        }

        private void OnYandexDataLoaded()
        {
            musicSource.volume = LocalYandexData.Instance.MusicVolume;
        }
    }
}