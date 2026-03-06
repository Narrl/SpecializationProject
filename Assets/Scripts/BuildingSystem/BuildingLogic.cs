using UnityEngine;

public abstract class BuildingLogic : MonoBehaviour, IFactoryTickable
{
    protected Building m_building;
    protected BuildingGrid m_grid;
    protected Vector2Int m_gridPos;

    public virtual void Setup(Building building, BuildingGrid grid)
    {
        m_building = building;
        m_grid = grid;
        m_gridPos = grid.WorldToGridPosition(building.transform.position);

        FactoryManager.Instance.Register(this);
    }

    public virtual void OnDestroy()
    {
        FactoryManager.Instance.Unregister(this);
    }

    public abstract void FactoryTick(float deltaTime);

    // Iterates all output shape units and tries to push one resource in each output direction.
    // Returns true if at least one push succeeded.
    protected bool TryPushAll(ResourceType type)
    {
        bool bAnyPushed = false;

        foreach (var unit in m_building.Model.ShapeUnits)
        {
            if (!unit.HasOutputs) continue;

            Vector2Int unitGridPos = m_grid.WorldToGridPosition(unit.transform.position);

            foreach (var dir in unit.OutputDirections)
            {
                Vector2Int targetPos = unitGridPos + dir.ToVector();
                IResourceInput input = m_grid.GetLogicAt<IResourceInput>(targetPos);
                if (input != null && input.TryDeposit(type, targetPos, dir.Opposite()))
                    bAnyPushed = true;
            }
        }

        return bAnyPushed;
    }
}