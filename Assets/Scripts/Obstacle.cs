using UnityEngine;

namespace SkibidiRunner
{
    public enum ObstacleType
    {
        Jumping,
        Sliding,
        Changeable
    }
    
    public class Obstacle : MonoBehaviour
    {
       [field:SerializeField] public ObstacleType ObstacleType { get; private set; }
    }
}