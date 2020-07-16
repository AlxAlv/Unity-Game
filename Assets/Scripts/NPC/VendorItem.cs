using UnityEngine;

[CreateAssetMenu(menuName = "Vendor/Item")]
public class VendorItem : ScriptableObject
{
    public HealthReward HealthToSell;
    public ShieldReward ShieldToSell;
    public Weapon WeaponToSell;
    public int Damage;
    public int Cost;
}
