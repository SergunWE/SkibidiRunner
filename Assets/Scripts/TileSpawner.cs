using System.Collections.Generic;
using TempleRun;
using UnityEngine;

namespace SkibidiRunner
{
    public class TileSpawner : MonoBehaviourInitializable
    {
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
        private List<Tile> _currentTiles;
        private List<GameObject> _currentObstacles;
        private Tile _prevTile;
        private float _frequencyObstacle;

        public override void Initialize()
        {
            _currentTiles = new List<Tile>();
            _currentObstacles = new List<GameObject>();
            _frequencyObstacle = frequencyObstacle;

            for (int i = 0; i < tileStartCount; i++)
            {
                SpawnTile(startingTile.GetComponent<Tile>());
            }

            SpawnTile(turnstilePrefabs.GetRandomItem().GetComponent<Tile>());
        }

        public void AddNewDirection(Vector3 direction)
        {
            _currentTileDirection = direction;
            DeletePreviousTiles();
            DeletePreviousObstacle();
            _currentTileLocation = _prevTile.pivot.position + _currentTileDirection * _prevTile.size;
            int currentPathLength = Random.Range(minimumStraightTiles, maximumStraightTiles);
            for (int i = 0; i < currentPathLength; i++) 
            {
                SpawnTile(startingTile.GetComponent<Tile>(), (i != 0));
            }
            
            SpawnTile(turnstilePrefabs.GetRandomItem().GetComponent<Tile>());
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
            while (_currentTiles.Count != 1)
            {
                var tile = _currentTiles[0].gameObject;
                _currentTiles.RemoveAt(0);
                Destroy(tile);
            }
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

        private void SpawnTile(Component tile, bool spawnObstacle = false)
        {
            var newTileRotation = tile.gameObject.transform.rotation *
                                  Quaternion.LookRotation(_currentTileDirection, Vector3.up);
            var newTile = Instantiate(tile.gameObject, _currentTileLocation, newTileRotation);
            _prevTile = newTile.GetComponent<Tile>();
            _currentTiles.Add(_prevTile);
            
            if(spawnObstacle) SpawnObstacle();
            
            // (3, 4, 5) * (0, 0, 1) => (0,0,-5)'
            if (_prevTile.type == TileType.Straight)
            {
                _currentTileLocation += _currentTileDirection * _prevTile.size;
            }
        }
        
        private void SpawnObstacle()
        {
            if (Random.value >= _frequencyObstacle) return;
            GameObject obstaclePrefab = obstaclePrefabs.GetRandomItem();
            Quaternion newObjectRotation  = obstaclePrefab.gameObject.transform.rotation * Quaternion.LookRotation(_currentTileDirection, Vector3.up);
            GameObject obstacle = Instantiate(obstaclePrefab, _currentTileLocation, newObjectRotation);
            _currentObstacles.Add(obstacle);
        }
    }
}