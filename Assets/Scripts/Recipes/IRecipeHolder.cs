using UnityEngine;

public interface IRecipeHolder
{
    public RecipeData CurrentRecipe { get; }
    public RecipeData[] AvailableRecipes { get; }

    public void SetRecipe(RecipeData recipe);
}
