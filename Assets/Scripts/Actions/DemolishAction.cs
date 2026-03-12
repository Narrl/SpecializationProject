using Actions;
using UnityEngine;

public class DemolishAction : ActionStack.Action
{
    private readonly BuildingSystem m_buildingSystem;
    private bool m_bIsDone;

    private Building m_hoveredBuilding;

    public DemolishAction(BuildingSystem system)
    {
        m_buildingSystem = system;
    }

    public override void OnBegin(bool bFirstTime) { }

    public override void OnUpdate()
    {
        // Cancel
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1))
        {
            ClearHighlight();
            m_bIsDone = true;
            return;
        }

        // Update hovered building each frame
        Building hovered = m_buildingSystem.GetBuildingAtMouse();
        if (hovered != m_hoveredBuilding)
        {
            ClearHighlight();
            m_hoveredBuilding = hovered;
            SetHighlight(m_hoveredBuilding);
        }

        // Confirm demolish
        if (Input.GetMouseButtonDown(0) && m_hoveredBuilding != null)
        {
            m_buildingSystem.DemolishBuilding(m_hoveredBuilding);
            m_hoveredBuilding = null;
            m_bIsDone = true;
        }
    }

    public override void OnEnd()
    {
        ClearHighlight();
    }

    public override bool IsDone()
    {
        return m_bIsDone;
    }

    private void SetHighlight(Building building)
    {
        if (building == null) return;
        building.SetDemolishMaterialState(true);
    }

    private void ClearHighlight()
    {
        if (m_hoveredBuilding == null) return;
        m_hoveredBuilding.SetDemolishMaterialState(false);
        m_hoveredBuilding = null;
    }
}