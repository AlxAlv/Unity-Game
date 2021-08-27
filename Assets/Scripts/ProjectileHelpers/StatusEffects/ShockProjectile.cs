using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockProjectile : StatusProjectile
{
	public override void ApplyEffect(EntityStatus entityStatus)
	{
		if (entityStatus == null)
			return;

		entityStatus.ApplyShock(AmountPerTick, NumberOfTicks, TimePerTick);
	}
}
