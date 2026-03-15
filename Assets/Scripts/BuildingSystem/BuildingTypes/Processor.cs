using Mono.Cecil;
using UnityEngine;

/// <summary>
/// This class represents a Processor building, which takes in resources according to a selected recipe and produces output resources.
/// </summary>

public class Processor : BuildingLogic, IResourceInput, IRecipeHolder
{
    [SerializeField] private RecipeData[] m_availableRecipes;

    [SerializeField] private int m_maxOutputBuffer = 10;

    private ResourceContainer m_input = new ResourceContainer();
    private int m_outputBuffer = 0;

    public RecipeData CurrentRecipe { get; private set; }
    public RecipeData[] AvailableRecipes => m_availableRecipes;

    public override void Setup(Building building, BuildingGrid grid)
    {
        base.Setup(building, grid);
        
        if (m_availableRecipes.Length > 0)
            CurrentRecipe = m_availableRecipes[0];
    }

    public void SetRecipe(RecipeData recipe)
    {
        CurrentRecipe = recipe;
        m_outputBuffer = 0;
    }

    public override void FactoryTick(float deltaTime)
    {
        bool bHasInputs = true;

        foreach (var inputStruct in CurrentRecipe.Inputs)
        {
            if (m_input.GetAmount(inputStruct.ResourceType) < inputStruct.Amount)
                bHasInputs = false;
        }

        if (bHasInputs && m_outputBuffer < m_maxOutputBuffer)
        {
            foreach (var inputStruct in CurrentRecipe.Inputs)
            {
                m_input.TryRemove(inputStruct.ResourceType, inputStruct.Amount);
            }

            m_outputBuffer += CurrentRecipe.Output.Amount;
        }

        while (m_outputBuffer > 0)
        {
            if (!TryPushFromOutputs(CurrentRecipe.Output.ResourceType)) break;
            m_outputBuffer--;
        }
    }

    // Iterates all input shape units and tries to deposit the resource if the fromDirection is valid.
    public bool TryDeposit(ResourceType type, Vector2Int targetCell, GridDirection fromDirection)
    {
        bool bIsValidType = false;

        foreach (var inputStruct in CurrentRecipe.Inputs)
        {
            if (type == inputStruct.ResourceType) bIsValidType = true;
        }

        if (!bIsValidType) return false;

        foreach (var unit in m_building.Model.ShapeUnits)
        {
            if (!unit.HasInputs) continue;

            Vector2Int unitGridPos = m_grid.WorldToGridPosition(unit.transform.position);
            if (unitGridPos != targetCell) continue;
            if (!unit.AcceptsFrom(fromDirection)) continue;

            m_input.Add(type, 1);
            return true;
        }

        return false;
    }
}