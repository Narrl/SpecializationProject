using UnityEngine;
using Actions;

public class GameManager : ActionStack.ActionBehavior
{
    private static GameManager sm_instance;

    [SerializeField] private BuildingSystem m_buildingSystem;
    [SerializeField] private Canvas m_canvas;
    [SerializeField] private GameObject m_pauseMenuUI;
    [SerializeField] private GameObject m_buildingInfoPanelUI;

    private void Start()
    {
        if (sm_instance != null && sm_instance != this)
        {
            Destroy(gameObject);
            return;
        }
        sm_instance = this;

        ActionStack.Main.PushAction(this);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        Building hoveredBuilding = m_buildingSystem.GetBuildingAtMouse();
        // Make it only work for the Processor building for now
        if (hoveredBuilding != null && hoveredBuilding.GetComponentInChildren<Processor>() != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                BuildingInfoPanel buildingInfoPanel = BuildingInfoPanel.Create(m_buildingInfoPanelUI, m_canvas.transform, hoveredBuilding);
                ActionStack.Main.PushAction(buildingInfoPanel); 
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
            ActionStack.Main.PushAction(new PlaceBuildingAction(m_buildingSystem, m_buildingSystem.BuildingDatas[0]));

        if (Input.GetKeyDown(KeyCode.Alpha2))
            ActionStack.Main.PushAction(new PlaceBuildingAction(m_buildingSystem, m_buildingSystem.BuildingDatas[1]));

        if (Input.GetKeyDown(KeyCode.Alpha3))
            ActionStack.Main.PushAction(new PlaceBuildingAction(m_buildingSystem, m_buildingSystem.BuildingDatas[2]));

        if (Input.GetKeyDown(KeyCode.Alpha4))
            ActionStack.Main.PushAction(new PlaceBuildingAction(m_buildingSystem, m_buildingSystem.BuildingDatas[3]));

        if (Input.GetKeyDown(KeyCode.Alpha5))
            ActionStack.Main.PushAction(new PlaceBuildingAction(m_buildingSystem, m_buildingSystem.BuildingDatas[4]));

        if (Input.GetKeyDown(KeyCode.Alpha6))
            ActionStack.Main.PushAction(new PlaceBuildingAction(m_buildingSystem, m_buildingSystem.BuildingDatas[5]));

        if (Input.GetKeyDown(KeyCode.X))
            ActionStack.Main.PushAction(new DemolishAction(m_buildingSystem));

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseMenu pauseMenu = PauseMenu.Create(m_pauseMenuUI, m_canvas.transform);
            ActionStack.Main.PushAction(pauseMenu);
        }
    }

    public override bool IsDone()
    {
        return false;
    }
}
