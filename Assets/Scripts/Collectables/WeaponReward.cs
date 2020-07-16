using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponReward : Collectables
{
	[SerializeField] private ItemData _itemWeaponData;
	private Color _weaponColor;
	private int _weaponDamage;

	protected override void Pick()
	{
		EquipWeapon();
	}

	private void EquipWeapon()
	{
		if (_entity != null)
		{
			EntityWeapon entityWeapon = _entity.GetComponent<EntityWeapon>();
			entityWeapon.AddWeaponToInventory(_itemWeaponData.WeaponToEquip, _weaponDamage, _weaponColor);
		}
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
