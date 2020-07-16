using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldSkill : BaseSkill
{
	protected Shield _shieldToUse;

	protected ShieldSkill(Shield shieldToUse) : base(shieldToUse)
	{
		_shieldToUse = shieldToUse;
	}

	public override bool IsBase()
	{
		return false;
	}
}
