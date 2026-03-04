using UnityEngine;

[CreateAssetMenu(menuName = "Data/Building")]
public class BuildingData : ScriptableObject
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public int Cost { get; private set; }
    [field: SerializeField] public ResourceType RequiredPlacedOnResource { get; private set; }
    [field: SerializeField] public BuildingModel Model { get; private set; }
    [field: SerializeField] public BuildingLogic LogicPrefab { get; private set; }
}
