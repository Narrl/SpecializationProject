using UnityEngine;

/// <summary>
/// Represents configuration data for a building asset, including its name, required placement resources, visual
/// model, and associated logic prefab.
/// </summary>

[CreateAssetMenu(menuName = "Data/Building")]
public class BuildingData : ScriptableObject
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public ResourceType[] RequiredPlacedOnResources { get; private set; }
    [field: SerializeField] public BuildingModel Model { get; private set; }
    [field: SerializeField] public BuildingLogic LogicPrefab { get; private set; }
}
