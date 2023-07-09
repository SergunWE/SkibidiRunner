using System;
using Unity.VisualScripting;
using UnityEngine;

namespace TempleRun {

public enum TileType {
    Straight,
    Left,
    Right,
    Sideways
}

/// <summary>
/// Defines the attributes of a tile.
/// </summary>
[Serializable]
public class Tile : MonoBehaviour
{
    [field:SerializeField] public TileType Type { get; private set; }
    [field:SerializeField] public Transform Pivot { get; private set; }
    [field:SerializeField] public float Size { get; private set; }
}

}