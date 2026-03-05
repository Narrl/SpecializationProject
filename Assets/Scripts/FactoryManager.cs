using System.Collections.Generic;
using UnityEngine;

public interface IFactoryTickable
{
    void FactoryTick(float deltaTime);
}

public class FactoryManager : MonoBehaviour
{
    [SerializeField] private float m_tickRate = 10.0f;

    private readonly List<IFactoryTickable> m_tickables = new List<IFactoryTickable>();
    private readonly List<Conveyor> m_conveyors = new List<Conveyor>();
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
        m_tickables.Remove(tickable);
    }

    public void RegisterConveyor(Conveyor conveyor)
    {
        if (conveyor != null && !m_conveyors.Contains(conveyor))
        {
            m_conveyors.Add(conveyor);
            SortConveyors();
        }
    }

    public void UnregisterConveyor(Conveyor conveyor)
    {
        m_conveyors.Remove(conveyor);
        SortConveyors();
    }

    // Sort conveyors so chain-ends are ticked first, working backwards up each chain.
    // This means when a conveyor ticks, the one ahead of it has already cleared space.
    public void SortConveyors()
    {
        List<Conveyor> sorted = new List<Conveyor>();
        HashSet<Conveyor> remaining = new HashSet<Conveyor>(m_conveyors);

        // Keep adding conveyors whose forward neighbor is already sorted (or not a conveyor)
        bool bMadeProgress = true;
        while (bMadeProgress && remaining.Count > 0)
        {
            bMadeProgress = false;
            foreach (Conveyor c in new List<Conveyor>(remaining))
            {
                Conveyor next = c.GetForwardConveyor();
                if (next == null || sorted.Contains(next))
                {
                    sorted.Add(c);
                    remaining.Remove(c);
                    bMadeProgress = true;
                }
            }
        }

        // Any remaining conveyors are part of a loop — just append them
        sorted.AddRange(remaining);

        m_conveyors.Clear();
        m_conveyors.AddRange(sorted);
    }

    private void Update()
    {
        if (m_tickRate <= 0.0f) return;

        m_accumulator += Time.deltaTime;
        float tickInterval = 1.0f / m_tickRate;

        while (m_accumulator >= tickInterval)
        {
            // Regular buildings first (excavators, processors)
            for (int i = 0; i < m_tickables.Count; i++)
                m_tickables[i].FactoryTick(tickInterval);

            // Conveyors ticked end-first so pushing cascades cleanly
            for (int i = 0; i < m_conveyors.Count; i++)
                m_conveyors[i].FactoryTick(tickInterval);

            m_accumulator -= tickInterval;
        }
    }
}