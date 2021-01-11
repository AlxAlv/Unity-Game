using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthReward : Collectables
{
	[SerializeField] private int _healthToAdd = 1;
	[SerializeField] private ParticleSystem _healthBonusFX;

	protected override void Pick()
	{
		AddHealth(_entity);
		SoundManager.Instance.Playsound("Audio/SoundEffects/HealFx");
	}

	protected override void PlayEffects()
	{
		Instantiate(_healthBonusFX, transform.position, Quaternion.identity);
	}

	public void AddHealth(Entity entityToAddHealth)
	{
		if (entityToAddHealth != null)
		{
			entityToAddHealth.GetComponent<Health>().GainHealth(_healthToAdd);
		}
	}
}
