using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthReward : Collectables
{
	[SerializeField] private int _healthToAdd = 15;

	private void Awake()
	{
		_pickupSoundPath = "Audio/SoundEffects/HealFx";
	}

	protected override bool Pick()
	{
		AddHealth(_entity);

		return true;
	}

	public void AddHealth(Entity entityToAddHealth)
	{
		if (entityToAddHealth != null)
		{
			entityToAddHealth.GetComponent<Health>().GainHealth(_healthToAdd);
		}
	}
}
