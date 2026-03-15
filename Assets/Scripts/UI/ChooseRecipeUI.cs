using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This is the UI class for one of the buttons that allows you to change the recipe of a building.
/// </summary>

public class ChooseRecipeUI : MonoBehaviour
{
    [SerializeField] private TMP_Text m_recipeName;
    [SerializeField] private TMP_Text m_recipeOutputAmount;
    [SerializeField] private Button m_recipeButton;

    public void Setup(RecipeData recipeData, Action<RecipeData> onRecipeSelectedEvent)
    {
        m_recipeName.text = recipeData.RecipeName;
        m_recipeOutputAmount.text = recipeData.Output.Amount.ToString();
        m_recipeButton.image.sprite = recipeData.OutputSprite;
        m_recipeButton.onClick.AddListener(() => onRecipeSelectedEvent(recipeData));
    }
}
