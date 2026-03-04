using UnityEngine;
using System.Collections.Generic;

public class Excavator : BuildingLogic
{
    [SerializeField] private int m_pullPerTick = 1;
    [SerializeField] private int m_maxResourceAmount = 100;

    private ResourceContainer m_output = new ResourceContainer();
    public ResourceContainer Output => m_output;

    public override void FactoryTick(float deltaTime)
    {
        ResourceType type = m_building.Data.RequiredPlacedOnResource;
        if (type == ResourceType.None) return;
        if (m_output.GetAmount(type) >= m_maxResourceAmount) return;

        m_output.Add(type, m_pullPerTick);
        Debug.Log($"Excavator extracted {m_pullPerTick}x {type}");
    }
}