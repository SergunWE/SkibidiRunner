using System;
using TMPro;
using UnityEngine;

namespace SkibidiRunner.UI
{
    [RequireComponent(typeof(TMP_Text))]
    public class ScoreDisplay : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;
        private int _prevValue = -1;

        public void OnScoreChanged(int score)
        {
            if(_prevValue == score) return;
            _prevValue = score;
            text.text = score.ToString();
        }
    }
}