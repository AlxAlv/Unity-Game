using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipType
{
	Helmet = 0,
	Armor = 1,
	Footwear = 2
}

[CreateAssetMenu(menuName = "Items/Equip", fileName = "Item Equip")]
public class EquipData : ScriptableObject
{
	public Equip equipToEquip;
	public Sprite equipSprite;
	public EquipType equipType;
}
