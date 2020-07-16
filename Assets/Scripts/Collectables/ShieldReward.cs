using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldReward : Collectables
{
	[SerializeField] private int _shieldToAdd = 1;
	[SerializeField] private ParticleSystem _shieldFx;

	protected override void Pick()
	{
		AddShield(_entity);
	}

	protected override void PlayEffects()
	{
		Instantiate(_shieldFx, transform.position, Quaternion.identity);
	}

	public void AddShield(Entity entityToAddShield)
	{
		if (entityToAddShield != null)
		{
			entityToAddShield.GetComponent<Health>().GainShield(_shieldToAdd);
		}
	}
}
