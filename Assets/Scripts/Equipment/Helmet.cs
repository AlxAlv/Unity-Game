using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helmet : Equip
{
	private void Awake()
	{
		equipName = "Helmet";
	}

	public override bool IsHelmet()
	{
		return true;
	}
}
