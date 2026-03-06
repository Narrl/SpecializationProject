using System.Collections.Generic;
using UnityEngine;

public class BuildingSystem : MonoBehaviour
{
    public const float CellSize = 1f;

    [SerializeField] private BuildingData m_excavatorData;
    [SerializeField] private BuildingData m_processorData;
    [SerializeField] private BuildingData m_conveyorData;
    [SerializeField] private BuildingPreview m_previewPrefab;
    [SerializeField] private Building m_buildingPrefab;
    [SerializeField] private BuildingGrid m_grid;

    public BuildingData ExcavatorData => m_excavatorData;
    public BuildingData ProcessorData => m_processorData;
    public BuildingData ConveyorData => m_conveyorData;
    public BuildingGrid Grid => m_grid;

    public Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        if (groundPlane.Raycast(ray, out float distance))
            return ray.GetPoint(distance);
        return Vector3.zero;
    }

    // Returns the building on whichever grid cell the mouse is hovering, or null
    public Building GetBuildingAtMouse()
    {
        Vector3 mouseWorld = GetMouseWorldPosition();
        Vector2Int gridPos = m_grid.WorldToGridPosition(mouseWorld);
        return m_grid.GetBuildingAt(gridPos);
    }

    public void DemolishBuilding(Building building)
    {
        if (building == null) return;
        m_grid.ClearBuilding(building);
        Destroy(building.gameObject);
    }

    public BuildingPreview CreatePreview(BuildingData data, Vector3 position)
    {
        BuildingPreview preview = Instantiate(m_previewPrefab, position, Quaternion.identity);
        preview.Setup(data);
        return preview;
    }

    public void CancelPreview(BuildingPreview preview)
    {
        if (preview != null)
            Destroy(preview.gameObject);
    }

    public bool TrySnapAndValidate(BuildingPreview preview, out List<Vector3> buildPositions)
    {
        buildPositions = preview.Model.GetAllBuildingPositions();
        bool bCanBuild = m_grid.CanPlaceBuilding(preview.Data.RequiredPlacedOnResources, buildPositions);

        if (bCanBuild)
        {
            Vector3 snappedPos = m_grid.GetSnappedCenterPosition(buildPositions);
            preview.transform.position = snappedPos;
            preview.ChangeState(BuildingPreview.PreviewState.Valid);
        }
        else
        {
            preview.ChangeState(BuildingPreview.PreviewState.Invalid);
        }

        return bCanBuild;
    }

    public void PlaceFromPreview(BuildingPreview preview, List<Vector3> buildPositions)
    {
        Building building = Instantiate(m_buildingPrefab, preview.transform.position, Quaternion.identity);
        building.Setup(preview.Data, preview.Model.Rotation, m_grid);
        m_grid.SetBuilding(building, buildPositions);
        Destroy(preview.gameObject);
    }
}