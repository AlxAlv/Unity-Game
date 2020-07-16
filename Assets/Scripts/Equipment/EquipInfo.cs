using UnityEngine;

public class EquipInfo
{
    public int StrBonus { get; set; }
    public int DexBonus { get; set; }
    public int IntBonus { get; set; }
    public Color Color { get; set; }
    public string InternalName { get; set; }
    public EquipType EquipType { get; set; }

    public EquipInfo(int strBonus, int dexBonus, int intelligenceBonus, Color equipColor, EquipType type)
    {
        StrBonus = strBonus;
        DexBonus = dexBonus;
        IntBonus = intelligenceBonus;
        Color = equipColor;
        EquipType = type;
    }
}
