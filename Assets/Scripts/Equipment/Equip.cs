using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equip : MonoBehaviour
{
	[SerializeField] private int _intToAdd = 0;
	[SerializeField] private int _dexToAdd = 0;
	[SerializeField] private int _strToAdd = 0;

	[Header("Settings")]
	[SerializeField] public string equipName = "Equip";

	public int IntBonus => _intToAdd;
	public int DexBonus => _dexToAdd;
	public int StrBonus => _strToAdd;

	public virtual bool IsHelmet()
	{
		return false;
	}

	public virtual bool IsArmor()
	{
		return false;
	}

	public virtual bool IsFootwear()
	{
		return false;
	}

	public void SetBonuses(int strBonus, int intBonus, int dexBonus)
	{
		_strToAdd = strBonus;
		_intToAdd = intBonus;
		_dexToAdd = dexBonus;
	}
}
