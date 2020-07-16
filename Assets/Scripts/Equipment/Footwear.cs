using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footwear : Equip
{
	private void Awake()
	{
		equipName = "Footwear";
	}

	public override bool IsFootwear()
	{
		return false;
	}
}
