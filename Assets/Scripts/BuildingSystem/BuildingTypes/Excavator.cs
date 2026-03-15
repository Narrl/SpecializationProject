using UnityEngine;

/// <summary>
/// This class represents an Excavator building, which produces resources based on the type of resource present in the cells it occupies. 
/// It has a buffer to store produced resources before pushing them to outputs.
/// </summary>

public class Excavator : BuildingLogic
{
    [SerializeField] private int m_producePerTick = 1;
    [SerializeField] private int m_maxBuffer = 10;

    private ResourceType m_resourceType;
    private int m_buffer = 0;

    public override void Setup(Building building, BuildingGrid grid)
    {
        base.Setup(building, grid);

        m_resourceType = GetResourceType();
    }

    public override void FactoryTick(float deltaTime)
    {
        if (m_resourceType == ResourceType.None) return;

        if (m_buffer < m_maxBuffer)
            m_buffer += m_producePerTick;

        while (m_buffer > 0)
        {
            if (!TryPushFromOutputs(m_resourceType)) break;
            m_buffer--;
        }
    }

    private ResourceType GetResourceType()
    {
        foreach (var cell in m_building.OccupiedCells)
        {
            var resourceType = m_grid.GetResourceAt(cell);
            if (resourceType != ResourceType.None)
            {
                return resourceType;
            }
        }

        return ResourceType.None;
    }
}