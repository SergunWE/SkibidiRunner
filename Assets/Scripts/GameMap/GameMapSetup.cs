using System.Collections.Generic;
using TempleRun;
using UnityEngine;

namespace SkibidiRunner.GameMap
{
    [CreateAssetMenu(menuName = "Game/Game Map Setup")]
    public class GameMapSetup : ScriptableObject
    {
        [field: SerializeField] public int InitialTile { get; private set; }
        [field: SerializeField] public int MinimumTile { get; private set; }
        [field: SerializeField] public int MaximumTile { get; private set; }
        [field: SerializeField] public int WithoutObstaclesTile { get; private set; }
        
        [field: SerializeField] public Tile StraightTilePrefab { get; private set; }
        [field: SerializeField] public Tile LeftTilePrefab { get; private set; }
        [field: SerializeField] public Tile RightTilePrefab { get; private set; }
        [field: SerializeField] public Tile SidewaysTilePrefab { get; private set; }
        [field: SerializeField] public List<Obstacle> ObstaclesPrefab { get; private set; }
        
        [field: SerializeField] public float InitialSpeedTime { get; private set; }
        [field: SerializeField] public float IncreaseSpeedTime { get; private set; }
        [field: SerializeField] public float ObstacleFrequency { get; private set; }
        [field: SerializeField] public float IncreasedFrequencyObstacles { get; private set; }
    }
}