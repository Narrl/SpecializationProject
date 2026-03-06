using UnityEngine;

public class Excavator : BuildingLogic
{
    [SerializeField] private int m_producePerTick = 1;
    [SerializeField] private int m_maxBuffer = 10;

    private ResourceType m_resourceType;
    private int m_buffer = 0;

    public override void Setup(Building building, BuildingGrid grid)
    {
        base.Setup(building, grid);
        m_resourceType = m_grid.Grid[m_gridPos.x, m_gridPos.y].ResourceType;
    }

    public override void FactoryTick(float deltaTime)
    {
        if (m_resourceType == ResourceType.None) return;

        if (m_buffer < m_maxBuffer)
            m_buffer += m_producePerTick;

        while (m_buffer > 0)
        {
            if (!TryPushAll(m_resourceType)) break;
            m_buffer--;
        }
    }
}