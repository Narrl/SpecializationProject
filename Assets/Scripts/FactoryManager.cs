using System.Collections.Generic;
using UnityEngine;

public interface IFactoryTickable
{
    void FactoryTick(float deltaTime);
}

/// <summary>
/// This class is the main manager for the factory simulation, responsible for ticking all factory-related logic at a fixed rate.
/// </summary>

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

    #region Regular Building Handling

    public void Register(IFactoryTickable tickable)
    {
        if (tickable != null && !m_tickables.Contains(tickable))
            m_tickables.Add(tickable);
    }

    public void Unregister(IFactoryTickable tickable)
    {
        m_tickables.Remove(tickable);
    }

    #endregion

    #region Conveyor Handling

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

    // Here I sort conveyors so that the end of a conveyor chain is ticked first,
    // solving the problem of items being pushed multiple times in a single tick for example.
    public void SortConveyors()
    {
        List<Conveyor> sorted = new List<Conveyor>();
        HashSet<Conveyor> remaining = new HashSet<Conveyor>(m_conveyors);

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

        sorted.AddRange(remaining);

        m_conveyors.Clear();
        m_conveyors.AddRange(sorted);
    }

    #endregion

    private void Update()
    {
        if (m_tickRate <= 0.0f) return;

        m_accumulator += Time.deltaTime;
        float tickInterval = 1.0f / m_tickRate;

        if (m_accumulator >= tickInterval)
        {
            // Regular buildings first (excavators, processors etc.)
            for (int i = 0; i < m_tickables.Count; i++)
                m_tickables[i].FactoryTick(tickInterval);

            // Then conveyors, in sorted order
            for (int i = 0; i < m_conveyors.Count; i++)
                m_conveyors[i].FactoryTick(tickInterval);

            m_accumulator -= tickInterval;
        }
    }
}