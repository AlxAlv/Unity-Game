using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponReward : Collectables
{
	[SerializeField] private GameObject _weaponPrefab;
	[SerializeField] private ItemData _itemWeaponData;
	private Color _weaponColor;
	private int _weaponMinDamage;
	private int _weaponMaxDamage;
	private int _criticalChance;
	private int _skillHaste;
	private string _prefixEnchant;
	private string _suffixEnchant;

	private void Awake()
	{
		_pickupSoundPath = "Audio/SoundEffects/StashingItemFx";
	}

	protected override bool Pick()
	{
		return EquipWeapon();
	}

	private bool EquipWeapon()
	{
		if (_entity != null)
		{
			EntityWeapon entityWeapon = _entity.GetComponent<EntityWeapon>();
			return entityWeapon.AddWeaponToInventory(_itemWeaponData.WeaponToEquip, _weaponMinDamage, _weaponMaxDamage, _criticalChance, _skillHaste, _prefixEnchant, _suffixEnchant,_weaponColor);
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

	public void SetDamage(int minDamage, int maxDamage, int criticalChance, int skillHaste, string prefixEnchant, string suffixEnchant)
	{
		_weaponMinDamage = minDamage;
		_weaponMaxDamage = maxDamage;
		_criticalChance = criticalChance;
		_skillHaste = skillHaste;
		_prefixEnchant = prefixEnchant;
		_suffixEnchant = suffixEnchant;
	}

	public void SetWeaponData()
	{
		_itemWeaponData.WeaponToEquip = _weaponPrefab.GetComponent<Weapon>();
		_itemWeaponData.WeaponSprite = _weaponPrefab.GetComponent<Weapon>().WeaponRenderer.sprite;
	}
}
