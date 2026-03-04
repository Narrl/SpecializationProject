using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public struct ResourceNodeEntry
{
    public Vector2Int Position;
    public ResourceType Type;
}

[System.Serializable]
public struct ResourceVisualEntry
{
    public ResourceType Type;
    public GameObject VisualPrefab;
}

public class BuildingGrid : MonoBehaviour
{
    [SerializeField] private int m_width;
    [SerializeField] private int m_height;
    [SerializeField] private ResourceNodeEntry[] m_resourceNodes;
    [SerializeField] private ResourceVisualEntry[] m_resourceVisuals;

    private BuildingGridCell[,] m_grid;

    public BuildingGridCell[,] Grid => m_grid;

    private void Awake()
    {
        m_grid = new BuildingGridCell[m_width, m_height];
        for (int x = 0; x < m_width; x++)
        {
            for (int y = 0; y < m_height; y++)
            {
                m_grid[x, y] = new BuildingGridCell();
            }
        }

        foreach (var entry in m_resourceNodes)
        {
            if (!IsWithinBounds(entry.Position)) continue;

            m_grid[entry.Position.x, entry.Position.y].SetResourceType(entry.Type);
            SpawnResourceVisual(entry.Type, entry.Position);
        }
    }

    private void SpawnResourceVisual(ResourceType type, Vector2Int gridPos)
    {
        foreach (var visual in m_resourceVisuals)
        {
            if (visual.Type == type && visual.VisualPrefab != null)
            {
                Vector3 worldPos = GridToWorldPosition(gridPos);
                Instantiate(visual.VisualPrefab, worldPos, Quaternion.identity, transform);
                return;
            }
        }
    }

    public void SetBuilding(Building building, List<Vector3> allBuildingPositions)
    {
        foreach (var position in allBuildingPositions)
        {
            var gridPos = WorldToGridPosition(position);
            m_grid[gridPos.x, gridPos.y].SetBuilding(building);
        }
    }

    public bool CanPlaceBuilding(ResourceType requiredResourceType, List<Vector3> allBuildingPositions)
    {
        int validPlacementCount = 0;

        foreach (var position in allBuildingPositions)
        {
            var gridPos = WorldToGridPosition(position);
            if (!IsWithinBounds(gridPos) || !m_grid[gridPos.x, gridPos.y].IsEmpty())
            {
                return false;
            }
            if (m_grid[gridPos.x, gridPos.y].HasResource(requiredResourceType))
            {
                validPlacementCount++;
            }
        }

        // If no resource is required, any empty placement is valid
        if (requiredResourceType == ResourceType.None) return true;

        return validPlacementCount > 0;
    }

    public Vector3 GetSnappedCenterPosition(List<Vector3> allBuildingPositions)
    {
        List<int> xs = allBuildingPositions.Select(pos => Mathf.FloorToInt(pos.x)).ToList();
        List<int> zs = allBuildingPositions.Select(pos => Mathf.FloorToInt(pos.z)).ToList();
        float centerX = (xs.Min() + xs.Max()) / 2f + BuildingSystem.CellSize / 2f;
        float centerZ = (zs.Min() + zs.Max()) / 2f + BuildingSystem.CellSize / 2f;
        return new Vector3(centerX, 0, centerZ);
    }

    private bool IsWithinBounds(Vector2Int gridPos)
    {
        return gridPos.x >= 0 && gridPos.x < m_width && gridPos.y >= 0 && gridPos.y < m_height;
    }

    public Vector2Int WorldToGridPosition(Vector3 worldPosition)
    {
        int x = Mathf.FloorToInt((worldPosition - transform.position).x / BuildingSystem.CellSize);
        int y = Mathf.FloorToInt((worldPosition - transform.position).z / BuildingSystem.CellSize);
        return new Vector2Int(x, y);
    }

    public Vector3 GridToWorldPosition(Vector2Int gridPos)
    {
        float x = gridPos.x * BuildingSystem.CellSize + BuildingSystem.CellSize / 2f + transform.position.x;
        float z = gridPos.y * BuildingSystem.CellSize + BuildingSystem.CellSize / 2f + transform.position.z;
        return new Vector3(x, 0, z);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        if (BuildingSystem.CellSize <= 0 || m_width <= 0 || m_height <= 0) return;

        Vector3 origin = transform.position;
        for (int x = 0; x <= m_width; x++)
        {
            Vector3 start = origin + new Vector3(x * BuildingSystem.CellSize, 0.01f, 0);
            Vector3 end = origin + new Vector3(x * BuildingSystem.CellSize, 0.01f, m_height * BuildingSystem.CellSize);
            Gizmos.DrawLine(start, end);
        }
        for (int y = 0; y <= m_height; y++)
        {
            Vector3 start = origin + new Vector3(0, 0.01f, y * BuildingSystem.CellSize);
            Vector3 end = origin + new Vector3(m_width * BuildingSystem.CellSize, 0.01f, y * BuildingSystem.CellSize);
            Gizmos.DrawLine(start, end);
        }
    }
}

public class BuildingGridCell
{
    private Building m_building;
    private ResourceType m_resourceType = ResourceType.None;

    public ResourceType ResourceType => m_resourceType;

    public void SetBuilding(Building building)
    {
        m_building = building;
    }

    public void SetResourceType(ResourceType resourceType)
    {
        m_resourceType = resourceType;
    }

    public bool IsEmpty()
    {
        return m_building == null;
    }

    public bool HasResource(ResourceType type)
    {
        if (type == ResourceType.None) return true;
        return m_resourceType == type;
    }
}