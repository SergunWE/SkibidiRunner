using UnityEngine;

namespace SkibidiRunner.UI
{
    public class BeatSplashes : MonoBehaviour
    {
        [SerializeField] private int splashStep;
        [SerializeField] private float splashSize = 2;

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
                _transform.localScale = Vector3.one * splashSize;
                _beatNumber = -1;
            }

            _beatNumber++;
        }
    }
}