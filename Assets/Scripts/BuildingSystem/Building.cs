using UnityEngine;

public class Building : MonoBehaviour
{
    private BuildingModel m_model;
    private BuildingData m_data;

    public string Name => m_data.Name;
    public int Cost => m_data.Cost;
    public BuildingModel Model => m_model;
    public BuildingData Data => m_data;

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
}
