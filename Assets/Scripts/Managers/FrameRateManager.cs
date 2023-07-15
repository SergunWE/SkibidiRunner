using System;
using UnityEngine;

namespace SkibidiRunner.Managers
{
    public class FrameRateManager : MonoBehaviour
    {
        [SerializeField] private int fps = -1;
        
        private void Awake()
        {
            Application.targetFrameRate = fps;
        }
    }
}