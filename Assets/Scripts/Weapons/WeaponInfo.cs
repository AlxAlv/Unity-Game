using UnityEngine;

public class WeaponInfo
{
    public int MinDamage { get; set; }
    public int MaxDamage { get; set; }
    public int CriticalChance { get; set; }
    public int SkillHaste { get; set; }
    public string PrefixEnchant { get; set; }
    public string SuffixEnchant { get; set; }
    public Color Color { get; set; }
    public string InternalName { get; set; }

    public WeaponInfo(int minDamage, int maxDamage, int criticalChance, int skillHaste, string prefixEnchant, string suffixEnchant, Color weaponColor)
    {
	    MinDamage = minDamage;
	    MaxDamage = maxDamage;
	    CriticalChance = criticalChance;
        SkillHaste = skillHaste;
        PrefixEnchant = prefixEnchant;
        SuffixEnchant = suffixEnchant;
        Color = weaponColor;
    }
}
