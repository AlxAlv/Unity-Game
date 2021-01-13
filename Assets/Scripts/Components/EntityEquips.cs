using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityEquips : MonoBehaviour
{
    // Components
    private Entity _entity;
    private StatManager _statManager;

    // Current Equips
    public Equip CurrentHelmet { get; set; }
    public Equip CurrentArmor { get; set; }
    public Equip CurrentFootwear { get; set; }

    // Current Equips Info
    public EquipInfo HelmetInfo;
    public EquipInfo ArmorInfo;
    public EquipInfo FootwearInfo;

    private void Start()
    {
        _entity = GetComponent<Entity>();
        _statManager = GetComponent<StatManager>();
    }

    public bool AddEquipToInventory(Equip equipToEquip, int strBonus, int intBonus, int dexBonus, Color equipColor, EquipType type)
    {
	    if (_entity.EntityType == Entity.EntityTypes.Player)
		    return GetComponent<Inventory>().AddEquipToInventory(equipToEquip, new EquipInfo(strBonus, dexBonus, intBonus, equipColor, type));
	    else
		    return false;
    }

    public void EquipHelmet(Equip equip, EquipInfo equipInfo)
    {
        // Remove Previous Stats
        if (CurrentHelmet != null)
            _statManager.ModifyEquipStats((0 - CurrentHelmet.StrBonus), (0 - CurrentHelmet.IntBonus), (0 - CurrentHelmet.DexBonus));

        CurrentHelmet = equip;
        HelmetInfo = equipInfo;

        // Add New Stats
        _statManager.ModifyEquipStats((CurrentHelmet.StrBonus), (CurrentHelmet.IntBonus), (CurrentHelmet.DexBonus));
    }

    public void EquipArmor(Equip equip, EquipInfo equipInfo)
    {
        // Remove Previous Stats
        if (CurrentArmor != null)
            _statManager.ModifyEquipStats((0 - CurrentArmor.StrBonus), (0 - CurrentArmor.IntBonus), (0 - CurrentArmor.DexBonus));

        CurrentArmor = equip;
        ArmorInfo = equipInfo;

        // Add New Stats
        _statManager.ModifyEquipStats((CurrentArmor.StrBonus), (CurrentArmor.IntBonus), (CurrentArmor.DexBonus));
    }

    public void EquipFootwear(Equip equip, EquipInfo equipInfo)
    {
        // Remove Previous Stats
        if (CurrentFootwear != null)
            _statManager.ModifyEquipStats((0 - CurrentFootwear.StrBonus), (0 - CurrentFootwear.IntBonus), (0 - CurrentFootwear.DexBonus));

        CurrentFootwear = equip;
        FootwearInfo = equipInfo;

        // Add New Stats
        _statManager.ModifyEquipStats((CurrentFootwear.StrBonus), (CurrentFootwear.IntBonus), (CurrentFootwear.DexBonus));
    }
}
