using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponReward : Collectables
{
	[SerializeField] private ItemData _itemWeaponData;
	private Color _weaponColor;
	private int _weaponDamage;

	protected override bool Pick()
	{
		return EquipWeapon();
	}

	private bool EquipWeapon()
	{
		if (_entity != null)
		{
			EntityWeapon entityWeapon = _entity.GetComponent<EntityWeapon>();
			return entityWeapon.AddWeaponToInventory(_itemWeaponData.WeaponToEquip, _weaponDamage, _weaponColor);
		}
		else
			return false;
	}

	public void SetWeapon(ItemData dataToReward)
	{
		_itemWeaponData = dataToReward;
	}

	public void SetWeaponColor(Color color)
	{
		_weaponColor = color;
	}

	public void SetDamage(int damage)
	{
		_weaponDamage = damage;
	}
}
