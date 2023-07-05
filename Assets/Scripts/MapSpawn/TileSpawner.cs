using System.Collections.Generic;
using System.Linq;
using SkibidiRunner.GameMap;
using SkibidiRunner.ObjectsPool;
using TempleRun;
using UnityEngine;

namespace SkibidiRunner
{
    public class TileSpawner : MonoBehaviourInitializable
    {
        [SerializeField] private GameMapSetup gameMapSetup;
        [SerializeField] private Transform startSpawnPosition;

        [SerializeField] private List<Tile> currentTiles;

        [SerializeField] private int tileStartCount = 5;
        [SerializeField] private int minimumStraightTiles = 3;
        [SerializeField] private int maximumStraightTiles = 20;
        [SerializeField] [Range(0, 1)] private float frequencyObstacle = 0.5f;
        [SerializeField] [Range(0, 1)] private float frequencyObstacleChangeRate;
        [SerializeField] private GameObject startingTile;
        [SerializeField] private List<GameObject> turnstilePrefabs;
        [SerializeField] private List<GameObject> obstaclePrefabs;

        private Vector3 _currentTileLocation = Vector3.zero;
        private Vector3 _currentTileDirection = Vector3.forward;
        private List<GameObject> _currentObstacles;
        private Tile _prevTile;
        private float _frequencyObstacle;

        private GameObjectPool _tilePool;


        private Vector3 _currentSpawnPosition;

        [ContextMenu("Generate Map Items")]
        private void GenerateMapItems()
        {
            DeleteMapItems();
            _currentSpawnPosition = startSpawnPosition.position;

            for (int i = gameMapSetup.MaximumTile; i > 0; i--)
            {
                var tile = Instantiate(gameMapSetup.StraightTilePrefab, _currentSpawnPosition, startSpawnPosition.rotation);
                currentTiles.Add(tile);
                _currentSpawnPosition += startSpawnPosition.forward * tile.Size;
            }
        }

        [ContextMenu("Delete Map Items")]
        private void DeleteMapItems()
        {
            if (currentTiles == null) return;
            foreach (var tile in currentTiles.Where(tile => tile != null))
            {
                DestroyImmediate(tile.gameObject);
            }
            currentTiles = new List<Tile>();
        }

        public override void Initialize()
        {
            //_currentTiles = new List<GameObject>();
            _currentObstacles = new List<GameObject>();
            _frequencyObstacle = frequencyObstacle;

            _tilePool = new GameObjectPool(startingTile, maximumStraightTiles);

            for (int i = 0; i < tileStartCount; i++)
            {
                SpawnTile(_tilePool.Get());
            }

            SpawnTurn(turnstilePrefabs.GetRandomItem());
        }

        public void AddNewDirection(Vector3 direction)
        {
            _currentTileDirection = direction;
            DeletePreviousTiles();
            DeletePreviousObstacle();
            _currentTileLocation = _prevTile.Pivot.position + _currentTileDirection * _prevTile.Size;
            int currentPathLength = Random.Range(minimumStraightTiles, maximumStraightTiles);
            for (int i = 0; i < currentPathLength; i++)
            {
                SpawnTile(_tilePool.Get(), (i != 0));
            }

            SpawnTurn(turnstilePrefabs.GetRandomItem());
        }

        public void IncreaseObstacles()
        {
            if (_frequencyObstacle > 1)
            {
                _frequencyObstacle = 1;
            }
            else
            {
                _frequencyObstacle += frequencyObstacleChangeRate;
            }
        }

        private void DeletePreviousTiles()
        {
            _tilePool.ReturnAll();

            // while (_currentTiles.Count != 1)
            // {
            //     var tile = _currentTiles[0].gameObject;
            //     _currentTiles.RemoveAt(0);
            //     Destroy(tile);
            // }
        }

        private void DeletePreviousObstacle()
        {
            while (_currentObstacles.Count != 0)
            {
                var obstacle = _currentObstacles[0].gameObject;
                _currentObstacles.RemoveAt(0);
                Destroy(obstacle);
            }
        }

        private void SpawnTile(GameObject tile, bool spawnObstacle = false)
        {
            var newTileRotation = tile.transform.rotation *
                                  Quaternion.LookRotation(_currentTileDirection, Vector3.up);
            tile.transform.position = _currentTileLocation;
            tile.transform.rotation = newTileRotation;
            Instantiate(tile.gameObject, _currentTileLocation, newTileRotation);
            _prevTile = tile.GetComponent<Tile>();
            //_currentTiles.Add(tile);
            _currentTileLocation += _currentTileDirection * _prevTile.Size;
            if (spawnObstacle) SpawnObstacle();
        }

        private void SpawnTurn(GameObject turn)
        {
            var newTileRotation = turn.transform.rotation *
                                  Quaternion.LookRotation(_currentTileDirection, Vector3.up);
            var newTile = Instantiate(turn.gameObject, _currentTileLocation, newTileRotation);
            _prevTile = newTile.GetComponent<Tile>();
            //_currentTiles.Add(newTile);
        }

        private void SpawnObstacle()
        {
            if (Random.value >= _frequencyObstacle) return;
            GameObject obstaclePrefab = obstaclePrefabs.GetRandomItem();
            Quaternion newObjectRotation = obstaclePrefab.gameObject.transform.rotation *
                                           Quaternion.LookRotation(_currentTileDirection, Vector3.up);
            GameObject obstacle = Instantiate(obstaclePrefab, _currentTileLocation, newObjectRotation);
            _currentObstacles.Add(obstacle);
        }
    }
}