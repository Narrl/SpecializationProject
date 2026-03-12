using Actions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BuildingInfoPanel : ActionStack.ActionBehavior
{
    [SerializeField] private TMP_Text m_buildingName;
    [SerializeField] private Transform m_availableRecipesPanelTransform;
    [SerializeField] private Transform m_inputsPanelTransform;
    [SerializeField] private Transform m_outputsPanelTransform;

    [SerializeField] private GameObject m_chooseRecipePrefab;

    private Building m_building;

    private bool m_bIsDone = false;

    public override bool IsDone()
    {
        return m_bIsDone;
    }

    public override void OnBegin(bool bFirstTime)
    {
        base.OnBegin(bFirstTime);

        m_buildingName.text = m_building.Data.Name;

        foreach (var recipeData in m_building.GetComponent<Processor>().AvailableRecipes)
        {
            GameObject recipeUI = Instantiate(m_chooseRecipePrefab, m_availableRecipesPanelTransform);
            recipeUI.GetComponentInChildren<ChooseRecipeUI>().Setup(recipeData);
        }
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (Input.GetKeyDown(KeyCode.Escape))
            m_bIsDone = true;
    }

    public override void OnEnd()
    {
        base.OnEnd();
        Destroy(gameObject);
    }

    public static BuildingInfoPanel Create(GameObject buildingInfoPanelGO, Transform parent, Building building)
    {
        GameObject go = Instantiate(buildingInfoPanelGO, parent);
        BuildingInfoPanel createdBuildingInfoPanel = go.GetComponent<BuildingInfoPanel>();
        createdBuildingInfoPanel.m_building = building;
        return createdBuildingInfoPanel;
    }
}
