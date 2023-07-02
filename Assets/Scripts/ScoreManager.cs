using UnityEngine;
using UnityEngine.Events;

namespace SkibidiRunner
{
    public class ScoreManager : MonoBehaviourInitializable
    {
        [SerializeField] private float scoreMultiplier;
        [SerializeField] private UnityEvent<int> scoreEvent;
        private float _score;
        private bool _init;

        public override void Initialize()
        {
            _init = true;
        }
        
        private void Update()
        {
            if(!_init) return;
            _score += scoreMultiplier * Time.deltaTime;
            scoreEvent?.Invoke((int)_score);
        }

        public void GameOver()
        {
            scoreEvent?.Invoke((int)_score);
            gameObject.SetActive(false);
        }
    }
}