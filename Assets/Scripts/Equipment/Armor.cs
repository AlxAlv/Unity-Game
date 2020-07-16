using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor : Equip
{
	private void Awake()
	{
		equipName = "Armor";
	}

	public override bool IsArmor()
	{
		return true;
	}
}
