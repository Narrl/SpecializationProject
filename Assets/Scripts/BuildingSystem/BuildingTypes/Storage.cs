using System;
using UnityEngine;

/// <summary>
/// This class represents a Storage building that can accept resources and store them in an internal buffer.
/// </summary>

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
                if (TryPushFromOutputs(resourceType)) 
                {
                    m_input.TryRemove(resourceType, 1);
                }
            }
        }
    }

    // Iterates all input shape units and tries to deposit the resource if the fromDirection is valid.
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
