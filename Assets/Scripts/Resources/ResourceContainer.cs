using System.Collections.Generic;

public enum ResourceType
{
    None,
    CopperOre,
    IronOre,
    CopperIngot,
    IronIngot,
    CopperTube,
    IronPlate,
    CopperWire,
    SmallCircuit
}

[System.Serializable]
public struct ResourceStruct
{
    public ResourceType ResourceType;
    public int Amount;

    public ResourceStruct(ResourceType type, int amount)
    {
        ResourceType = type;
        Amount = amount;
    }
}

public class ResourceContainer
{
    private readonly Dictionary<ResourceType, int> m_amounts = new Dictionary<ResourceType, int>();

    public int GetAmount(ResourceType type)
    {
        return m_amounts.TryGetValue(type, out int value) ? value : 0;
    }

    public void Add(ResourceType type, int amount)
    {
        if (amount <= 0) return;
        m_amounts[type] = GetAmount(type) + amount;
    }

    public bool TryRemove(ResourceType type, int amount)
    {
        if (amount <= 0) return true;
        int current = GetAmount(type);
        if (current < amount) return false;
        m_amounts[type] = current - amount;
        return true;
    }
}