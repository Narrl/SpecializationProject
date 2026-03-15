using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// This class represents the visual model of a building, that has a reference to its building shape units
/// and to a wrapper transform that can be rotated to rotate the building.
/// </summary>

public class BuildingModel : MonoBehaviour
{
    [SerializeField] private Transform m_wrapper;

    public float Rotation => m_wrapper.eulerAngles.y;

    private BuildingShapeUnit[] m_shapeUnits;

    public BuildingShapeUnit[] ShapeUnits => m_shapeUnits;

    private void Awake()
    {
        m_shapeUnits = GetComponentsInChildren<BuildingShapeUnit>();
    }

    public void Rotate(float angle)
    {
        m_wrapper.Rotate(Vector3.up, angle);

        int steps = Mathf.RoundToInt(((angle % 360) + 360) % 360 / 90);

        foreach (var unit in m_shapeUnits)
        {
            for (int i = 0; i < unit.OutputDirections.Length; i++)
                unit.OutputDirections[i] = unit.OutputDirections[i].Rotate(steps);

            for (int i = 0; i < unit.InputDirections.Length; i++)
                unit.InputDirections[i] = unit.InputDirections[i].Rotate(steps);
        }
    }

    public List<Vector3> GetAllBuildingPositions()
    {
        return m_shapeUnits.Select(unit => unit.transform.position).ToList();
    }
}