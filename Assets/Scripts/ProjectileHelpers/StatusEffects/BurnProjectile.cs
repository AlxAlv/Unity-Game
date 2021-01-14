using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnProjectile : StatusProjectile
{
	public override void ApplyEffect(EntityStatus entityStatus)
	{
		if (entityStatus == null)
			return;

		entityStatus.ApplyBurn(AmountPerTick, NumberOfTicks, TimePerTick);
	}
}

