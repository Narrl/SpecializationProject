using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This is the UI class for one of the elements that shows the input and output resources of a building's current recipe in the BuildingInfoPanel.
/// </summary>

public class InputOutputRecipeUI : MonoBehaviour
{
    [SerializeField] private TMP_Text m_recipeName;
    [SerializeField] private TMP_Text m_recipeInputOutputAmount;
    [SerializeField] private Image m_recipeImage;

    public void Setup(ResourceStruct inputOutputStruct, Sprite inputOutputSprite)
    {
        m_recipeName.text = inputOutputStruct.ResourceType.ToString();
        m_recipeInputOutputAmount.text = inputOutputStruct.Amount.ToString();
        m_recipeImage.sprite = inputOutputSprite;
    }
}
