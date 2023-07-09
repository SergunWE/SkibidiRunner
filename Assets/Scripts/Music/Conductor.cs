using System.Diagnostics;
using SkibidiRunner.Managers;
using SkibidiRunner.Map;
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

        [SerializeField] private UnityEvent beat;
        
        private float _songBpm;
        private float _secPerBeat = float.MaxValue;
        private float _songPosition;
        private int _lastBeat;
        
        private float _dspSongTime;

        private bool _init;

        public override void Initialize()
        {
            var stopwatch = Stopwatch.StartNew();
            var music = gameMusic.CurrentMusic;
            _songBpm = music.SongBpm;
            _secPerBeat = 60f / _songBpm;
            _dspSongTime = (float)AudioSettings.dspTime - music.FirstBeatOffset;
            musicSource.clip = music.Song;
            musicSource.volume = LocalYandexData.Instance.MusicVolume;
            musicSource.Play();
            _init = true;
            Debug.Log($"{nameof(Conductor)} init {stopwatch.ElapsedMilliseconds}ms");
        }
        
        private void Update()
        {
            if(!_init) return;
            _songPosition = (float)(AudioSettings.dspTime - _dspSongTime);
            _songPositionInBeats = _songPosition / _secPerBeat;
            if (_lastBeat >= (int) _songPositionInBeats) return;
            _lastBeat = (int) _songPositionInBeats;
            beat?.Invoke();
        }
    }
}