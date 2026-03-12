using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ChooseRecipeUI : MonoBehaviour
{
    [SerializeField] private TMP_Text m_recipeName;
    [SerializeField] private TMP_Text m_recipeOutputAmount;
    [SerializeField] private Button m_recipeButton;

    public void Setup(RecipeData recipeData)
    {
        m_recipeName.text = recipeData.RecipeName;
        m_recipeOutputAmount.text = recipeData.Output.Amount.ToString();
        m_recipeButton.image.sprite = recipeData.OutputSprite;
    }

    // DON'T FORGET TO ADD THE ONCLICK EVENT
}
