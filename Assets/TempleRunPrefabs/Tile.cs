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
public class Tile : MonoBehaviour
{
    public TileType type;
    public Transform pivot;
    public float size;
}

}