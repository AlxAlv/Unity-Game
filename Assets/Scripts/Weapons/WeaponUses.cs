using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WeaponUses : MonoBehaviour
{
	private Weapon m_weapon;
	private readonly string WEAPON_AMMO_SAVE = "Weapon_";

	private void Awake()
	{
		m_weapon = GetComponent<Weapon>();
		RefillUses();
	}

	public void ConsumeUse()
	{

	}

	public void RefillUses()
	{

	}

	public bool CanUse()
	{
		if (m_weapon.m_currentUses > 0)
		{
			return true;
		}

		return false;
	}
}
