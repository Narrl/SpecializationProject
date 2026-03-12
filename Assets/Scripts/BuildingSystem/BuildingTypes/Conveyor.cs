using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

[System.Serializable]
public struct ItemVisualEntry
{
    public ResourceType Type;
    public GameObject VisualPrefab;

    public GameObject GetVisualPrefab(ResourceType type)
    {
        if (Type == type){
            return VisualPrefab;
        }

        return null;
    }
}

public class Conveyor : BuildingLogic, IResourceInput
{
    private ResourceType? m_item;
    private GameObject m_itemVisual = null;

    [SerializeField] private ItemVisualEntry[] m_itemVisuals;

    public ResourceType? Item => m_item;

    public override void Setup(Building building, BuildingGrid grid)
    {
        base.Setup(building, grid);

        FactoryManager.Instance.Unregister(this);
        FactoryManager.Instance.RegisterConveyor(this);
    }

    public override void OnDestroy()
    {
        FactoryManager.Instance.UnregisterConveyor(this);
    }

    public override void FactoryTick(float deltaTime)
    {
        if (!m_item.HasValue) return;

        if (TryPushAll(m_item.Value))
        {
            Vector2Int targetGridPos = m_gridPos + m_building.Model.ShapeUnits[0].OutputDirections[0].ToVector();
            Vector3 targetPos = m_grid.GridToWorldPosition(targetGridPos) + new Vector3(0, 0.25f, 0);

            StartCoroutine(MoveItemVisual(targetPos, deltaTime * 2));
        }
            
    }

    // IResourceInput — checks that targetCell and fromDirection match one of our input shape units
    public bool TryDeposit(ResourceType type, Vector2Int targetCell, GridDirection fromDirection)
    {
        if (m_item.HasValue) return false;

        foreach (var unit in m_building.Model.ShapeUnits)
        {
            if (!unit.HasInputs) continue;

            Vector2Int unitGridPos = m_grid.WorldToGridPosition(unit.transform.position);
            if (unitGridPos != targetCell) continue;
            if (!unit.AcceptsFrom(fromDirection)) continue;

            m_item = type;
            foreach (var entry in m_itemVisuals)
            {
                if (entry.GetVisualPrefab(m_item.Value) != null)
                {
                    m_itemVisual = Instantiate(entry.GetVisualPrefab(m_item.Value), transform.position + new Vector3(0, 0.25f, 0), Quaternion.identity, transform);
                    break;
                }
            }

            return true;
        }

        return false;
    }

    // Used by FactoryManager to determine sort order
    public Conveyor GetForwardConveyor()
    {
        foreach (var unit in m_building.Model.ShapeUnits)
        {
            if (!unit.HasOutputs) continue;

            Vector2Int unitGridPos = m_grid.WorldToGridPosition(unit.transform.position);
            foreach (var dir in unit.OutputDirections)
            {
                Conveyor next = m_grid.GetLogicAt<Conveyor>(unitGridPos + dir.ToVector());
                if (next != null) return next;
            }
        }

        return null;
    }

    IEnumerator MoveItemVisual(Vector3 targetPos, float deltaTime)
    {
        while (m_itemVisual.transform.position != targetPos)
        {
            m_itemVisual.transform.position = Vector3.MoveTowards(m_itemVisual.transform.position, targetPos, deltaTime);
            yield return null;
        }

        m_item = null;
        Destroy(m_itemVisual);
        m_itemVisual = null;
    }
}