using UnityEngine;

public enum GridDirection { North, East, South, West }

public static class GridDirectionExtensions
{
    public static Vector2Int ToVector(this GridDirection dir)
    {
        switch (dir)
        {
            case GridDirection.North: return new Vector2Int(0, 1);
            case GridDirection.East: return new Vector2Int(1, 0);
            case GridDirection.South: return new Vector2Int(0, -1);
            case GridDirection.West: return new Vector2Int(-1, 0);
            default: return Vector2Int.zero;
        }
    }

    public static GridDirection Opposite(this GridDirection dir)
    {
        switch (dir)
        {
            case GridDirection.North: return GridDirection.South;
            case GridDirection.South: return GridDirection.North;
            case GridDirection.East: return GridDirection.West;
            case GridDirection.West: return GridDirection.East;
            default: return dir;
        }
    }

    public static GridDirection Rotate(this GridDirection dir, int steps)
    {
        return (GridDirection)(((int)dir + steps) % 4);
    }
}

public class BuildingShapeUnit : MonoBehaviour
{
    [Tooltip("Directions this cell pushes resources out to")]
    public GridDirection[] OutputDirections;

    [Tooltip("Directions this cell accepts resources from")]
    public GridDirection[] InputDirections;

    public bool HasOutputs => OutputDirections != null && OutputDirections.Length > 0;
    public bool HasInputs => InputDirections != null && InputDirections.Length > 0;

    public bool AcceptsFrom(GridDirection dir)
    {
        if (InputDirections == null) return false;
        foreach (var d in InputDirections)
            if (d == dir) return true;
        return false;
    }
}