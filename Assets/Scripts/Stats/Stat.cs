using UnityEngine;

public enum StatType : ushort
{
    Dexterity = 0,
    Intelligence = 1,
    Strength = 2
}

public class Stat
{
    public StatType Type { get; set; }
    public int StatAmount { get; set; }

    public Stat(StatType type, int amount)
    {
        Type = type;
        StatAmount = amount;
    }
}
