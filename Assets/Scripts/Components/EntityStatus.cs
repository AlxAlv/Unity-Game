using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityStatus : EntityComponent
{
	// Status Effects Enum
    public enum StatusEffects { Poisoned, Burned }

    // Poison Variables
    private bool _isPoisoned = false;
    private int _currentPoisonTick = 0;
    private int _numberOfPoisonTicks = 0;
    private float _poisonPercentPerTick = 0.0f;
    private float _poisonTickTimer = 0.0f;
    private float _poisonTickDuration = 1.0f;
    private string _poisonSoundFxPath = "Audio/SoundEffects/PoisonDamageFx";

    // Poison Variables
    private bool _isBurned = false;
    private int _currentBurnTick = 0;
    private int _numberOfBurnTicks = 0;
    private float _burnDamagePerTick = 0.0f;
    private float _burnTickTimer = 0.0f;
    private float _burnTickDuration = 1.0f;
    private string _burnSoundFxPath = "Audio/SoundEffects/PoisonDamageFx";

	protected override void HandleComponent()
	{
		// Handle Status Effects
		HandleStatusEffects();

		base.HandleComponent();
	}

	protected void HandleStatusEffects()
	{
		HandlePoison();
		HandleBurn();
	}

	public void ApplyPoison(float damagePercentPerTick, int numberOfTicks, float poisonTickDuration = 1.0f)
	{
		_poisonPercentPerTick = damagePercentPerTick;
		_numberOfPoisonTicks = numberOfTicks;
		_poisonTickDuration = poisonTickDuration;
		_currentPoisonTick = 0;

		_isPoisoned = true;
	}

	protected void HandlePoison()
	{
		if (!_isPoisoned)
			return;

		if (Time.time > _poisonTickTimer)
		{
			_poisonTickTimer = Time.time + _poisonTickDuration;

			// Deal Damage
			_health.TakeDamage(Mathf.Round(_health.m_maxHealth * _poisonPercentPerTick) , "PoisonStatus");
			++_currentPoisonTick;

			SoundManager.Instance.Playsound(_poisonSoundFxPath);

			if (_currentPoisonTick >= _numberOfPoisonTicks)
				_isPoisoned = false;
		}
	}

	public void ApplyBurn(float damagePerTick, int numberOfTicks, float burnTickDuration = 1.0f)
	{
		_burnDamagePerTick = damagePerTick;
		_numberOfBurnTicks = numberOfTicks;
		_burnTickDuration = burnTickDuration;
		_currentBurnTick = 0;

		_isBurned = true;
	}

	protected void HandleBurn()
	{
		if (!_isBurned)
			return;

		if (Time.time > _burnTickTimer)
		{
			_burnTickTimer = Time.time + _burnTickDuration;

			// Deal Damage
			_health.TakeDamage(Mathf.Round(_burnDamagePerTick), "burnStatus");
			++_currentBurnTick;

			SoundManager.Instance.Playsound(_burnSoundFxPath);

			if (_currentBurnTick >= _numberOfBurnTicks)
				_isBurned = false;
		}
	}
}
