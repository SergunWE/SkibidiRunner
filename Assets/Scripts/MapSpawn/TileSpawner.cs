using System.Collections.Generic;
using System.Linq;
using SkibidiRunner.GameMap;
using SkibidiRunner.ReadOnly;
using TempleRun;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace SkibidiRunner.MapSpawn
{
    public class TileSpawner : MonoBehaviourInitializable
    {
        [SerializeField] private GameMapSetup gameMapSetup;
        [SerializeField] private Transform startSpawnPosition;
        [SerializeField] private UnityEvent<Vector3> playerPositionEvent;

        [SerializeField] [ReadOnly] private List<Tile> straightTiles;
        [SerializeField] [ReadOnly] private List<Tile> turnTiles;
        [SerializeField] [ReadOnly] private Vector3 currentSpawnPosition;

        private GameObject _currentTurnTile;
        private float _straightSize;
        private int _minimumTiles;
        private int _maximumTiles;

        [ContextMenu("Generate map items")]
        private void GenerateMapItems()
        {
            DeleteMapItems();

            currentSpawnPosition = startSpawnPosition.position;

            for (int i = gameMapSetup.MaximumTile; i > 0; i--)
            {
                var tile = Instantiate(gameMapSetup.StraightTilePrefab, currentSpawnPosition,
                    startSpawnPosition.rotation);
                straightTiles.Add(tile);
                currentSpawnPosition += startSpawnPosition.forward * tile.Size;
            }

            var startSpawnRotation = startSpawnPosition.rotation;
            foreach (var tile in gameMapSetup.TurnstileTiles)
            {
                turnTiles.Add(Instantiate(tile, currentSpawnPosition,
                    startSpawnRotation * tile.transform.rotation));
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
            currentSpawnPosition = Vector3.zero;
        }

        [ContextMenu("Find map items")]
        private void FindMapItems()
        {
            var tiles = FindObjectsOfType<Tile>();
            straightTiles = new List<Tile>(tiles.Where(x => x.Type == TileType.Straight));
            turnTiles = new List<Tile>(tiles.Where(x => x.Type != TileType.Straight));
        }

        public override void Initialize()
        {
            _straightSize = gameMapSetup.StraightTilePrefab.Size;
            _minimumTiles = gameMapSetup.MinimumTile;
            _maximumTiles = gameMapSetup.MaximumTile;

            foreach (var turnTile in turnTiles)
            {
                turnTile.gameObject.SetActive(false);
            }

            SpawnTiles(gameMapSetup.InitialTile);
        }

        public void AddNewTiles()
        {
            SpawnTiles(Random.Range(_minimumTiles, _maximumTiles), true);
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

            playerPositionEvent?.Invoke(startSpawnPosition.position +
                                       _straightSize * (_maximumTiles - count) * startSpawnPosition.forward);

            Debug.Log($"Tiles {count} - player offset {_maximumTiles - count}" );
        }
    }
}