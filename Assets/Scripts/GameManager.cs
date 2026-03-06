using UnityEngine;
using Actions;

public class GameManager : ActionStack.ActionBehavior
{
    private static GameManager sm_instance;

    [SerializeField] private BuildingSystem m_buildingSystem;

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

        if (Input.GetKeyDown(KeyCode.Alpha1))
            ActionStack.Main.PushAction(new PlaceBuildingAction(m_buildingSystem, m_buildingSystem.ExcavatorData));

        if (Input.GetKeyDown(KeyCode.Alpha2))
            ActionStack.Main.PushAction(new PlaceBuildingAction(m_buildingSystem, m_buildingSystem.ProcessorData));

        if (Input.GetKeyDown(KeyCode.Alpha3))
            ActionStack.Main.PushAction(new PlaceBuildingAction(m_buildingSystem, m_buildingSystem.ConveyorData));

        if (Input.GetKeyDown(KeyCode.X))
            ActionStack.Main.PushAction(new DemolishAction(m_buildingSystem));
    }

    public override bool IsDone() => false;
}
