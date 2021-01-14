using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonProjectile : StatusProjectile
{
	public override void ApplyEffect(EntityStatus entityStatus)
	{
		if (entityStatus == null)
			return;

		entityStatus.ApplyPoison(AmountPerTick, NumberOfTicks, TimePerTick);
	}
}
