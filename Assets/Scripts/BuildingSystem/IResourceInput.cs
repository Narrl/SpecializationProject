using UnityEngine;

public interface IResourceInput
{
    bool TryDeposit(ResourceType type, Vector2Int targetCell, GridDirection fromDirection);
}