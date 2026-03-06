using UnityEngine;

public class Processor : BuildingLogic, IResourceInput
{
    [SerializeField] private ResourceType m_inputA;
    [SerializeField] private int m_inputAAmount = 1;

    [SerializeField] private ResourceType m_inputB;
    [SerializeField] private int m_inputBAmount = 1;

    [SerializeField] private ResourceType m_outputType;
    [SerializeField] private int m_outputAmount = 1;

    [SerializeField] private int m_maxBuffer = 10;

    private ResourceContainer m_input = new ResourceContainer();
    private int m_outputBuffer = 0;

    public override void FactoryTick(float deltaTime)
    {
        bool bHasA = m_input.GetAmount(m_inputA) >= m_inputAAmount;
        bool bHasB = m_input.GetAmount(m_inputB) >= m_inputBAmount;

        if (bHasA && bHasB && m_outputBuffer < m_maxBuffer)
        {
            m_input.TryRemove(m_inputA, m_inputAAmount);
            m_input.TryRemove(m_inputB, m_inputBAmount);
            m_outputBuffer += m_outputAmount;
        }

        while (m_outputBuffer > 0)
        {
            if (!TryPushAll(m_outputType)) break;
            m_outputBuffer--;
        }
    }

    // IResourceInput — checks that targetCell and fromDirection match one of our input shape units
    public bool TryDeposit(ResourceType type, Vector2Int targetCell, GridDirection fromDirection)
    {
        if (type != m_inputA && type != m_inputB) return false;

        foreach (var unit in m_building.Model.ShapeUnits)
        {
            if (!unit.HasInputs) continue;

            Vector2Int unitGridPos = m_grid.WorldToGridPosition(unit.transform.position);
            if (unitGridPos != targetCell) continue;
            if (!unit.AcceptsFrom(fromDirection)) continue;

            m_input.Add(type, 1);
            return true;
        }

        return false;
    }
}