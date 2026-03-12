using Mono.Cecil;
using UnityEngine;

public class Processor : BuildingLogic, IResourceInput, IRecipeHolder
{
    [SerializeField] private RecipeData[] m_availableRecipes;

    //[SerializeField] private ResourceStruct[] m_inputStructs;

    //[SerializeField] private ResourceType m_outputType;
    //[SerializeField] private int m_outputAmount = 1;

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
        bool hasInputs = true;

        foreach (var inputStruct in CurrentRecipe.Inputs)
        {
            if (m_input.GetAmount(inputStruct.ResourceType) < inputStruct.Amount)
                hasInputs = false;
        }

        if (hasInputs && m_outputBuffer < m_maxOutputBuffer)
        {
            foreach (var inputStruct in CurrentRecipe.Inputs)
            {
                m_input.TryRemove(inputStruct.ResourceType, inputStruct.Amount);
            }

            m_outputBuffer += CurrentRecipe.Output.Amount;
        }

        while (m_outputBuffer > 0)
        {
            if (!TryPushAll(CurrentRecipe.Output.ResourceType)) break;
            m_outputBuffer--;
        }
    }

    // IResourceInput — checks that targetCell and fromDirection match one of our input shape units
    public bool TryDeposit(ResourceType type, Vector2Int targetCell, GridDirection fromDirection)
    {
        bool isValidType = false;

        foreach (var inputStruct in CurrentRecipe.Inputs)
        {
            if (type == inputStruct.ResourceType) isValidType = true;
        }

        if (!isValidType) return false;

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