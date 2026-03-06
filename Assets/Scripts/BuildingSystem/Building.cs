using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    private BuildingModel m_model;
    private BuildingData m_data;
    private List<Vector2Int> m_occupiedCells = new List<Vector2Int>();

    [SerializeField] private Material m_demolishMat;
    private Material m_defaultMat;

    public string Name => m_data.Name;
    public int Cost => m_data.Cost;
    public BuildingModel Model => m_model;
    public BuildingData Data => m_data;
    public List<Vector2Int> OccupiedCells => m_occupiedCells;

    public void Setup(BuildingData data, float rotation, BuildingGrid grid)
    {
        m_data = data;
        m_model = Instantiate(data.Model, transform.position, Quaternion.identity, transform);
        m_model.Rotate(rotation);

        if (data.LogicPrefab != null)
        {
            BuildingLogic logic = Instantiate(data.LogicPrefab, transform.position, Quaternion.identity, transform);
            logic.Setup(this, grid);
        }
    }

    public void SetOccupiedCells(List<Vector2Int> cells)
    {
        m_occupiedCells = cells;
    }

    public void SetDemolishMaterialState(bool bIsDemolishing)
    {
        Material mat = bIsDemolishing ? m_demolishMat : m_defaultMat;
        foreach (var renderer in m_model.GetComponentsInChildren<Renderer>())
        {
            if (m_defaultMat == null)
                m_defaultMat = renderer.material;
            renderer.material = mat;
        }
    }
}