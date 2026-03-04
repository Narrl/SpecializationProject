using System.Collections.Generic;

public enum ResourceType
{
    None,
    Stone,
    IronOre,
    IronIngot
}

[System.Serializable]
public struct ResourceStack
{
    public ResourceType Type;
    public int Amount;

    public ResourceStack(ResourceType type, int amount)
    {
        Type = type;
        Amount = amount;
    }
}

public class ResourceContainer
{
    private readonly Dictionary<ResourceType, int> m_amounts = new Dictionary<ResourceType, int>();

    public int GetAmount(ResourceType type)
    {
        int value;
        return m_amounts.TryGetValue(type, out value) ? value : 0;
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