using System.Collections.Generic;
using UnityEngine;

namespace SkibidiRunner.Managers
{
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField] private List<MonoBehaviourInitializable> initObjects;

        private void Awake()
        {
            foreach (var obj in initObjects)
            {
                obj.Initialize();
            }
        }
    }
}