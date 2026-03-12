using Actions;
using System;
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
    [SerializeField] private GameObject m_inputOutputRecipePrefab;

    private Building m_building;

    public static event Action<RecipeData> OnRecipeSelectedEvent;

    private bool m_bIsDone = false;

    public override bool IsDone()
    {
        return m_bIsDone;
    }

    public override void OnBegin(bool bFirstTime)
    {
        base.OnBegin(bFirstTime);

        m_buildingName.text = m_building.Data.Name;

        OnRecipeSelectedEvent += OnRecipeSelected;

        foreach (var recipeData in m_building.GetComponentInChildren<Processor>().AvailableRecipes)
        {
            GameObject recipeUI = Instantiate(m_chooseRecipePrefab, m_availableRecipesPanelTransform);
            recipeUI.GetComponentInChildren<ChooseRecipeUI>().Setup(recipeData, OnRecipeSelectedEvent);
        }

        OnRecipeSelected(m_building.GetComponentInChildren<Processor>().CurrentRecipe);
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
        OnRecipeSelectedEvent -= OnRecipeSelected;
        Destroy(gameObject);
    }

    public void OnRecipeSelected(RecipeData recipeData)
    {
        foreach (var child in m_inputsPanelTransform.GetComponentsInChildren<InputOutputRecipeUI>())
        {
            Destroy(child.gameObject);
        }

        foreach (var child in m_outputsPanelTransform.GetComponentsInChildren<InputOutputRecipeUI>())
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < recipeData.Inputs.Length; i++)
        {
            GameObject inputUI = Instantiate(m_inputOutputRecipePrefab, m_inputsPanelTransform);
            inputUI.GetComponentInChildren<InputOutputRecipeUI>().Setup(recipeData.Inputs[i], recipeData.InputSprites[i]);
        }

        GameObject outputUI = Instantiate(m_inputOutputRecipePrefab, m_outputsPanelTransform);
        outputUI.GetComponentInChildren<InputOutputRecipeUI>().Setup(recipeData.Output, recipeData.OutputSprite);

        m_building.GetComponentInChildren<Processor>().SetRecipe(recipeData);
    }

    public void SetBuilding(Building building)
    {
        m_building = building;
    }

    public static BuildingInfoPanel Create(GameObject buildingInfoPanelGO, Transform parent, Building building)
    {
        GameObject go = Instantiate(buildingInfoPanelGO, parent);
        BuildingInfoPanel createdBuildingInfoPanel = go.GetComponent<BuildingInfoPanel>();
        createdBuildingInfoPanel.SetBuilding(building);
        return createdBuildingInfoPanel;
    }
}
