using System.Collections.Generic;
using TempleRun;
using UnityEngine;
using UnityEngine.Serialization;

namespace SkibidiRunner
{
    public class TileSpawner : MonoBehaviourInitializable
    {
        [SerializeField] private int tileStartCount = 10;
        [SerializeField] private int minimumStraightTiles = 3;
        [SerializeField] private int maximumStraightTiles = 20;
        [SerializeField] private GameObject startingTile;
        [SerializeField] private List<GameObject> turnstilePrefabs;
        [SerializeField] private List<GameObject> obstaclePrefabs;

        private Vector3 _currentTileLocation = Vector3.zero;
        private Vector3 _currentTileDirection = Vector3.forward;
        private List<Tile> _currentTiles;
        private List<GameObject> _currentObstacles;

        private Tile _prevTile;
        private Renderer _prevTileRenderer;

        private BoxCollider _startingTileBoxCollider;

        public override void Initialize()
        {
            _currentTiles = new List<Tile>();
            _currentObstacles = new List<GameObject>();
            _startingTileBoxCollider = startingTile.GetComponent<BoxCollider>();

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
            // Vector3 tilePlacementScale;
            // if (_prevTile.type == TileType.Sideways)
            // {
            //     tilePlacementScale = Vector3.Scale(_prevTileRenderer.bounds.size / 2 + (Vector3.one *
            //         _startingTileBoxCollider.size.z / 2), _currentTileDirection);
            // }
            // else
            // {
            //     tilePlacementScale = Vector3.Scale((_prevTileRenderer.bounds.size - (Vector3.one * 2)
            //         ) + (Vector3.one * _startingTileBoxCollider.size.z / 2), _currentTileDirection);
            // }
            
            //_currentTileLocation += _prevTile.size;
            _currentTileLocation = _prevTile.pivot.position + _currentTileDirection * _prevTile.size;
            int currentPathLength = Random.Range(minimumStraightTiles, maximumStraightTiles);
            for (int i = 0; i < currentPathLength; i++) 
            {
                SpawnTile(startingTile.GetComponent<Tile>(), (i != 0));
            }
            
            SpawnTile(turnstilePrefabs.GetRandomItem().GetComponent<Tile>());
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
            _prevTileRenderer = _prevTile.GetComponentInChildren<Renderer>();
            _currentTiles.Add(_prevTile);
            
            if(spawnObstacle) SpawnObstacle();
            
            // (3, 4, 5) * (0, 0, 1) => (0,0,-5)'
            if (_prevTile.type == TileType.Straight)
            {
                _currentTileLocation += _currentTileDirection * _prevTile.size;
                //Vector3.Scale(_prevTile.size * Vector3.forward, _currentTileDirection);
            }
        }
        
        private void SpawnObstacle()
        {
            if (Random.value > 0.4f) return;
            GameObject obstaclePrefab = obstaclePrefabs.GetRandomItem();
            Quaternion newObjectRotation  = obstaclePrefab.gameObject.transform.rotation * Quaternion.LookRotation(_currentTileDirection, Vector3.up);
            GameObject obstacle = Instantiate(obstaclePrefab, _currentTileLocation, newObjectRotation);
            _currentObstacles.Add(obstacle);
        }
    }
}