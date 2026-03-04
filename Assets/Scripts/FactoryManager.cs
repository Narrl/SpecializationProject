using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public interface IFactoryTickable
{
    void FactoryTick(float deltaTime);
}

public class FactoryManager : MonoBehaviour
{
    [SerializeField] private float m_tickRate = 10.0f; // ticks per second

    private readonly List<IFactoryTickable> m_tickables = new List<IFactoryTickable>();
    private float m_accumulator;

    private static FactoryManager sm_instance;

    public static FactoryManager Instance => sm_instance;

    private void Awake()
    {
        if (sm_instance != null && sm_instance != this)
        {
            Destroy(gameObject);
            return;
        }
        sm_instance = this;
    }

    public void Register(IFactoryTickable tickable)
    {
        if (tickable != null && !m_tickables.Contains(tickable))
            m_tickables.Add(tickable);
    }

    public void Unregister(IFactoryTickable tickable)
    {
        if (tickable != null)
            m_tickables.Remove(tickable);
    }

    private void Update()
    {
        if (m_tickRate <= 0.0f) return;

        float dt = Time.deltaTime;
        m_accumulator += dt;

        float tickInterval = 1.0f / m_tickRate;
        while (m_accumulator >= tickInterval)
        {
            for (int i = 0; i < m_tickables.Count; i++)
                m_tickables[i].FactoryTick(tickInterval);

            m_accumulator -= tickInterval;
        }
    }
}