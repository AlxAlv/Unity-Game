using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusProjectile : MonoBehaviour
{
	public int NumberOfTicks = 0;
	public float AmountPerTick = 0.0f;
	public float TimePerTick = 1.0f;

	public virtual void ApplyEffect(EntityStatus entityStatus)
	{
		if (entityStatus == null)
			return;
	}
}
