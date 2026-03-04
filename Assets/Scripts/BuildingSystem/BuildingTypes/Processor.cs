using UnityEngine;

public class Processor : BuildingLogic
{
    [SerializeField] private ResourceType m_inputA;
    [SerializeField] private int m_inputAAmount = 1;

    [SerializeField] private ResourceType m_inputB;
    [SerializeField] private int m_inputBAmount = 1;

    [SerializeField] private ResourceType m_outputType;
    [SerializeField] private int m_outputAmount = 1;

    private ResourceContainer m_input = new ResourceContainer();
    private ResourceContainer m_output = new ResourceContainer();

    public ResourceContainer Input => m_input;
    public ResourceContainer Output => m_output;

    public override void FactoryTick(float deltaTime)
    {
        bool bHasA = m_input.GetAmount(m_inputA) >= m_inputAAmount;
        bool bHasB = m_input.GetAmount(m_inputB) >= m_inputBAmount;

        if (!bHasA || !bHasB) return;

        m_input.TryRemove(m_inputA, m_inputAAmount);
        m_input.TryRemove(m_inputB, m_inputBAmount);

        m_output.Add(m_outputType, m_outputAmount);
    }
}