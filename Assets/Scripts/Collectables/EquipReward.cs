using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipReward : Collectables
{
	[SerializeField] private EquipData _itemEquipData;

	private Color _equipColor;
	private int _strBonus;
	private int _intBonus;
	private int _dexBonus;
	private EquipType _equipType;

	protected override bool Pick()
	{
		return Equip();
	}

	private bool Equip()
	{
		if (_entity != null)
		{
			EntityEquips entityEquips = _entity.GetComponent<EntityEquips>();
			return entityEquips.AddEquipToInventory(_itemEquipData.equipToEquip, _strBonus, _intBonus, _dexBonus, _equipColor, _equipType);
		}
		else
			return false;
	}

	public void SetEquip(EquipData dataToReward)
	{
		_itemEquipData = dataToReward;
	}

	public void SetEquipColor(Color color)
	{
		_equipColor = color;
	}

	public void SetBonusValues(int str, int intelligence, int dex)
	{
		_strBonus = str;
		_intBonus = intelligence;
		_dexBonus = dex;
	}

	public void SetEquipType(EquipType type)
	{
		_equipType = type;
	}
}
