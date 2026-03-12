using UnityEngine;

public interface IResourceInput
{
    public bool TryDeposit(ResourceType type, Vector2Int targetCell, GridDirection fromDirection);
}