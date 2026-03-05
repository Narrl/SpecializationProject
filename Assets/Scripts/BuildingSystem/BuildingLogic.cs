using UnityEngine;

public abstract class BuildingLogic : MonoBehaviour, IFactoryTickable
{
    protected Building m_building;
    protected BuildingGrid m_grid;
    protected Vector2Int m_gridPos;
    protected Vector2Int m_forward;

    public virtual void Setup(Building building, BuildingGrid grid)
    {
        m_building = building;
        m_grid = grid;
        m_gridPos = grid.WorldToGridPosition(building.transform.position);
        m_forward = RotationToDirection(building.Model.Rotation);

        FactoryManager.Instance.Register(this);
    }

    public virtual void OnDestroy()
    {
        FactoryManager.Instance.Unregister(this);
    }

    public abstract void FactoryTick(float deltaTime);

    // Tries to push one resource to whatever IResourceInput is directly ahead
    protected bool TryPushForward(ResourceType type)
    {
        IResourceInput next = m_grid.GetLogicAt<IResourceInput>(m_gridPos + m_forward);
        if (next != null)
            return next.TryDeposit(type);
        return false;
    }

    // Model's X axis is forward
    private static Vector2Int RotationToDirection(float rotation)
    {
        rotation = ((rotation % 360) + 360) % 360;

        if (rotation < 45 || rotation >= 315) return new Vector2Int(1, 0);   // east  (+X)
        if (rotation < 135) return new Vector2Int(0, -1);  // south (-Z)
        if (rotation < 225) return new Vector2Int(-1, 0);  // west  (-X)
        return new Vector2Int(0, 1);   // north (+Z)
    }
}