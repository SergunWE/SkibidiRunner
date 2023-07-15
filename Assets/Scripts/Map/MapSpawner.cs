using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using SkibidiRunner.Managers;
using SkibidiRunner.ReadOnly;
using TempleRun;
using UnityEngine;
using UnityEngine.Events;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

namespace SkibidiRunner.Map
{
    public class MapSpawner : MonoBehaviourInitializable
    {
        [SerializeField] private CurrentSetup gameMapSetup;
        [SerializeField] private Transform startSpawnPosition;
        [SerializeField] private UnityEvent<Vector3> playerPositionEvent;

        [SerializeField] [ReadOnly] private List<Tile> straightTiles;
        [SerializeField] [ReadOnly] private List<Tile> turnTiles;
        [SerializeField] [ReadOnly] private List<GameObject> obstacles;

        private float _straightSize;
        private int _minimumTiles;
        private int _maximumTiles;
        private int _obstacleCount;
        private float _obstacleFrequency;
        private float _increasedFrequencyObstacles;
        private int _withoutObstaclesTile;

        private GameObject _currentTurnTile;
        private readonly List<GameObject> _activeObstacles = new();

        [ContextMenu("Generate map items")]
        private void GenerateMapItems()
        {
            DeleteMapItems();

            var currentSpawnPosition = startSpawnPosition.position;

            for (int i = gameMapSetup.GameMapSetup.MaximumTile; i > 0; i--)
            {
                var tile = Instantiate(gameMapSetup.GameMapSetup.StraightTilePrefab, currentSpawnPosition,
                    startSpawnPosition.rotation);
                straightTiles.Add(tile);

                foreach (var obstaclePrefab in gameMapSetup.GameMapSetup.ObstaclesPrefab)
                {
                    int index = gameMapSetup.GameMapSetup.MaximumTile - i;
                    var obstacle = Instantiate(obstaclePrefab, currentSpawnPosition,
                        startSpawnPosition.rotation).gameObject;
                    obstacle.name += index;
                    obstacle.SetActive(false);
                    obstacles.Add(obstacle);
                }

                currentSpawnPosition += startSpawnPosition.forward * tile.Size;
            }

            foreach (var tile in gameMapSetup.GameMapSetup.TurnstileTiles)
            {
                turnTiles.Add(Instantiate(tile, currentSpawnPosition,
                    startSpawnPosition.rotation * tile.transform.rotation));
                turnTiles[^1].gameObject.SetActive(false);
            }
        }

        [ContextMenu("Delete map items")]
        private void DeleteMapItems()
        {
            if (straightTiles != null)
            {
                foreach (var tile in straightTiles.Where(tile => tile != null))
                {
                    DestroyImmediate(tile.gameObject);
                }
            }

            straightTiles = new List<Tile>();

            if (turnTiles != null)
            {
                foreach (var tile in turnTiles.Where(tile => tile != null))
                {
                    DestroyImmediate(tile.gameObject);
                }
            }

            turnTiles = new List<Tile>();

            if (obstacles != null)
            {
                foreach (var obstacle in obstacles.Where(obstacle => obstacle != null))
                {
                    DestroyImmediate(obstacle.gameObject);
                }
            }

            obstacles = new List<GameObject>();
        }

        [ContextMenu("Find map items")]
        private void FindMapItems()
        {
            var tiles = FindObjectsOfType<Tile>();
            straightTiles = new List<Tile>(tiles.Where(x => x.Type == TileType.Straight));
            turnTiles = new List<Tile>(tiles.Where(x => x.Type != TileType.Straight));

            obstacles = new List<GameObject>(FindObjectsOfType<Obstacle>().Select(x => x.gameObject));
        }

        public override void Initialize()
        {
            var stopwatch = Stopwatch.StartNew();
            var setup = gameMapSetup.GameMapSetup;
            _straightSize = setup.StraightTilePrefab.Size;
            _minimumTiles = setup.MinimumTile;
            _maximumTiles = setup.MaximumTile;
            _obstacleCount = setup.ObstaclesPrefab.Count;
            _obstacleFrequency = setup.ObstacleFrequency;
            _increasedFrequencyObstacles = setup.IncreasedFrequencyObstacles;
            _withoutObstaclesTile = setup.WithoutObstaclesTile;

            SpawnTiles(setup.InitialTile);
            Debug.Log($"{nameof(MapSpawner)} init {stopwatch.ElapsedMilliseconds}ms");
        }

        public void AddNewTiles()
        {
            SpawnTiles(Random.Range(_minimumTiles, _maximumTiles), true);
        }

        public void OnLifeGained()
        {
            SpawnTiles(gameMapSetup.GameMapSetup.InitialTile);
        }

        public void IncreaseFrequencyObstacles()
        {
            _obstacleFrequency += _increasedFrequencyObstacles;
        }

        private void SpawnTiles(int count, bool spawnObstacle = false)
        {
            var turnstileTile = turnTiles[Random.Range(0, turnTiles.Count)];

            if (_currentTurnTile != null)
            {
                _currentTurnTile.SetActive(false);
            }

            _currentTurnTile = turnstileTile.gameObject;
            _currentTurnTile.SetActive(true);

            //Debug.Log($"Tiles {count} - player offset {_maximumTiles - count}");

            for (int i = _activeObstacles.Count - 1; i >= 0 ; i--)
            {
                _activeObstacles[i].SetActive(false);
                _activeObstacles.RemoveAt(i);
            }

            if (spawnObstacle)
            {
                for (int i = _maximumTiles - count + _withoutObstaclesTile; i < _maximumTiles; i++)
                {
                    float probability = Random.Range(0f, 1f);
                    if (!(probability < _obstacleFrequency)) continue;
                    int index = i * _obstacleCount + Random.Range(0, _obstacleCount);
                    _activeObstacles.Add(obstacles[index]);
                    obstacles[index].SetActive(true);
                } 
            }

            playerPositionEvent?.Invoke(startSpawnPosition.position +
                                        _straightSize * (_maximumTiles - count) * startSpawnPosition.forward);

            
        }
    }
}