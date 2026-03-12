using UnityEngine;

[CreateAssetMenu(menuName = "Data/Recipe")]
public class RecipeData : ScriptableObject
{
    [field: SerializeField] public string RecipeName { get; private set; }
    [field: SerializeField] public ResourceStruct[] Inputs { get; private set; }
    [field: SerializeField] public ResourceStruct Output { get; private set; }
    [field: SerializeField] public Sprite[] InputSprites { get; private set; }
    [field: SerializeField] public Sprite OutputSprite { get; private set; }
}
