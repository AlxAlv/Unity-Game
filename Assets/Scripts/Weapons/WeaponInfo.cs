using UnityEngine;

public class WeaponInfo
{
    public int Damage { get; set; }
    public Color Color { get; set; }
    public string InternalName { get; set; }

    public WeaponInfo(int damage, Color weaponColor)
    {
        Damage = damage;
        Color = weaponColor;
    }
}
