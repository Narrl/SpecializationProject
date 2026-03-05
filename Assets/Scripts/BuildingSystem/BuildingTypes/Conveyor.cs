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

        if (TryPushForward(m_item.Value))
            m_item = null;
    }

    // IResourceInput — upstream buildings push into us
    public bool TryDeposit(ResourceType type)
    {
        if (m_item.HasValue) return false;
        m_item = type;
        return true;
    }

    // Used by FactoryManager to determine sort order
    public Conveyor GetForwardConveyor()
    {
        return m_grid.GetLogicAt<Conveyor>(m_gridPos + m_forward);
    }
}