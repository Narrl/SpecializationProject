using System;
using UnityEngine;

public class Storage : BuildingLogic, IResourceInput
{
    [SerializeField] private int m_maxOutputBuffer = 100;

    private ResourceContainer m_input = new ResourceContainer();

    public override void FactoryTick(float deltaTime)
    {
        foreach (ResourceType resourceType in Enum.GetValues(typeof(ResourceType)))
        {
            if (m_input.GetAmount(resourceType) > 0)
            {
                if (TryPushAll(resourceType)) 
                {
                    m_input.TryRemove(resourceType, 1);
                }
            }
        }
    }

    // IResourceInput — checks that targetCell and fromDirection match one of our input shape units
    public bool TryDeposit(ResourceType type, Vector2Int targetCell, GridDirection fromDirection)
    {
        foreach (var unit in m_building.Model.ShapeUnits)
        {
            if (!unit.HasInputs) continue;

            Vector2Int unitGridPos = m_grid.WorldToGridPosition(unit.transform.position);
            if (unitGridPos != targetCell) continue;
            if (!unit.AcceptsFrom(fromDirection)) continue;

            if (m_input.GetAmount(type) < m_maxOutputBuffer)
            {
                m_input.Add(type, 1);
                return true; 
            }
        }

        return false;
    }
}
