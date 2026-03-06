using UnityEngine;

public class Conveyor : BuildingLogic, IResourceInput
{
    private ResourceType? m_item;

    public ResourceType? Item => m_item;

    public override void Setup(Building building, BuildingGrid grid)
    {
        base.Setup(building, grid);

        FactoryManager.Instance.Unregister(this);
        FactoryManager.Instance.RegisterConveyor(this);
    }

    public override void OnDestroy()
    {
        FactoryManager.Instance.UnregisterConveyor(this);
    }

    public override void FactoryTick(float deltaTime)
    {
        if (!m_item.HasValue) return;

        if (TryPushAll(m_item.Value))
            m_item = null;
    }

    // IResourceInput — checks that targetCell and fromDirection match one of our input shape units
    public bool TryDeposit(ResourceType type, Vector2Int targetCell, GridDirection fromDirection)
    {
        if (m_item.HasValue) return false;

        foreach (var unit in m_building.Model.ShapeUnits)
        {
            if (!unit.HasInputs) continue;

            Vector2Int unitGridPos = m_grid.WorldToGridPosition(unit.transform.position);
            if (unitGridPos != targetCell) continue;
            if (!unit.AcceptsFrom(fromDirection)) continue;

            m_item = type;
            return true;
        }

        return false;
    }

    // Used by FactoryManager to determine sort order
    public Conveyor GetForwardConveyor()
    {
        foreach (var unit in m_building.Model.ShapeUnits)
        {
            if (!unit.HasOutputs) continue;

            Vector2Int unitGridPos = m_grid.WorldToGridPosition(unit.transform.position);
            foreach (var dir in unit.OutputDirections)
            {
                Conveyor next = m_grid.GetLogicAt<Conveyor>(unitGridPos + dir.ToVector());
                if (next != null) return next;
            }
        }

        return null;
    }
}