using UnityEngine;

public abstract class BuildingLogic : MonoBehaviour, IFactoryTickable
{
    protected Building m_building;
    protected BuildingGrid m_grid;

    public virtual void Setup(Building building, BuildingGrid grid)
    {
        m_building = building;
        m_grid = grid;
        FactoryManager.Instance.Register(this);
    }

    public virtual void OnDestroy()
    {
        FactoryManager.Instance.Unregister(this);
    }

    public abstract void FactoryTick(float deltaTime);
}