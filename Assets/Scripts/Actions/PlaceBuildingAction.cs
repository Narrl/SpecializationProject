using System.Collections.Generic;
using Actions;
using UnityEngine;

/// <summary>
/// This action handles the process of placing a building in the world.
/// </summary>

public class PlaceBuildingAction : ActionStack.Action
{
    private readonly BuildingSystem m_buildingSystem;
    private readonly BuildingData m_data;

    private BuildingPreview m_preview;
    private bool m_bIsDone;

    public PlaceBuildingAction(BuildingSystem system, BuildingData data)
    {
        m_buildingSystem = system;
        m_data = data;
    }

    public override void OnBegin(bool bFirstTime)
    {
        Vector3 mousePos = m_buildingSystem.GetMouseWorldPosition();
        m_preview = m_buildingSystem.CreatePreview(m_data, mousePos);
    }

    public override void OnUpdate()
    {
        if (m_preview == null)
        {
            m_bIsDone = true;
            return;
        }

        // Cancel Preview
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1))
        {
            m_buildingSystem.CancelPreview(m_preview);
            m_preview = null;
            m_bIsDone = true;
            return;
        }

        // Rotate
        if (Input.GetKeyDown(KeyCode.R))
        {
            m_preview.Rotate(90);
        }

        // Try place building
        Vector3 mousePos = m_buildingSystem.GetMouseWorldPosition();
        m_preview.transform.position = mousePos;

        List<Vector3> buildPositions;
        bool bCanBuild = m_buildingSystem.TrySnapAndValidate(m_preview, out buildPositions);

        if (bCanBuild && Input.GetMouseButtonDown(0))
        {
            m_buildingSystem.PlaceFromPreview(m_preview, buildPositions);
            m_preview = null;
            m_bIsDone = true;
        }
    }

    public override void OnEnd()
    {
        if (m_preview != null)
        {
            m_buildingSystem.CancelPreview(m_preview);
            m_preview = null;
        }
    }

    public override bool IsDone()
    {
        return m_bIsDone;
    }
}