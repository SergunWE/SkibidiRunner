using System.Collections.Generic;
using UnityEngine;

namespace SkibidiRunner.Managers
{
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField] private List<MonoBehaviourInitializable> initObjects;
        [SerializeField] private List<MonoBehaviourInitializable> startInitObjects;

        private void Awake()
        {
            foreach (var obj in initObjects)
            {
                obj.Initialize();
            }
        }
        
        private void Start()
        {
            foreach (var obj in startInitObjects)
            {
                obj.Initialize();
            }
        }
    }
}