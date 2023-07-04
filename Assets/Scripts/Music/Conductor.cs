using UnityEngine;
using UnityEngine.Events;

namespace SkibidiRunner.Music
{
    public class Conductor : MonoBehaviourInitializable
    {
        [SerializeField] private GameMusic gameMusic;
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
            _songBpm = gameMusic.SongBpm;
            _secPerBeat = 60f / _songBpm;
            _dspSongTime = (float)AudioSettings.dspTime - gameMusic.FirstBeatOffset;
            musicSource.Play();
            _init = true;
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