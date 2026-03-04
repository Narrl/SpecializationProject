using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuildingModel : MonoBehaviour
{
    [SerializeField] private Transform m_wrapper;

    public float Rotation => m_wrapper.transform.eulerAngles.y;

    private BuildingShapeUnit[] m_shapeUnits;

    private void Awake()
    {
        m_shapeUnits = GetComponentsInChildren<BuildingShapeUnit>();
    }

    public void Rotate(float angle)
    {
        m_wrapper.Rotate(Vector3.up, angle);
    }

    public List<Vector3> GetAllBuildingPositions()
    {
        return m_shapeUnits.Select(unit => unit.transform.position).ToList();
    }
}
