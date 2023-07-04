using SkibidiRunner.Music;
using UnityEngine;

namespace SkibidiRunner.UI
{
    public class BeatSplashes : MonoBehaviour
    {
        [SerializeField] private int splashStep;

        private int _beatNumber;
        private Transform _transform;

        private void Awake()
        {
            _transform = transform;
        }

        private void Update()
        {
            _transform.localScale = Vector3.Slerp(_transform.localScale, Vector3.one, Time.deltaTime);
        }

        public void OnBeat()
        {
            if (_beatNumber == splashStep)
            {
                _transform.localScale = Vector3.one * 2;
                _beatNumber = -1;
            }

            _beatNumber++;
        }
    }
}