using System;
using Random = UnityEngine.Random;

namespace SkibidiRunner.Managers
{
    public class RandomManager : MonoBehaviourInitializable
    {
        public override void Initialize()
        {
            Random.InitState(DateTime.Now.Millisecond);
        }
    }
}