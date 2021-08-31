using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityStatus : EntityComponent
{
	// Status Effects Enum
    public enum StatusEffects { Poisoned, Burned, Shocked }

    // Poison Variables
    private bool _isPoisoned = false;
    private int _currentPoisonTick = 0;
    private int _numberOfPoisonTicks = 0;
    private float _poisonPercentPerTick = 0.0f;
    private float _poisonTickTimer = 0.0f;
    private float _poisonTickDuration = 1.0f;
    private string _poisonSoundFxPath = "Audio/SoundEffects/PoisonDamageFx";

    // Burn Variables
    private bool _isBurned = false;
    private int _currentBurnTick = 0;
    private int _numberOfBurnTicks = 0;
    private float _burnDamagePerTick = 0.0f;
    private float _burnTickTimer = 0.0f;
    private float _burnTickDuration = 1.0f;
    private string _burnSoundFxPath = "Audio/SoundEffects/PoisonDamageFx";

	// Shock Variables
	private bool _isShocked = false;
	private int _currentShockTick = 0;
	private int _numberOfShockTicks = 0;
	private float _shockDamagePerTick = 0.0f;
	private float _shockTickTimer = 0.0f;
	private float _shockTickDuration = 1.0f;
	private string _shockSoundFxPath = "Audio/SoundEffects/PoisonDamageFx";

	protected override void Update()
	{
		// Handle Status Effects
		HandleStatusEffects();
	}

	protected void HandleStatusEffects()
	{
		HandlePoison();
		HandleBurn();
		HandleShock();
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
			float damageToTake = _health.m_maxHealth * (_poisonPercentPerTick / 100.0f);

			// Deal Damage
			_health.TakeDamage(damageToTake, "PoisonStatus", false);
			++_currentPoisonTick;

			SoundManager.Instance.Playsound(_poisonSoundFxPath);

			// Poison Particles
			GameObject poisonPS = Instantiate((Resources.Load("Prefabs/Effects/PoisonPS") as GameObject), transform.position, Quaternion.identity);
			poisonPS.GetComponent<ParticleSystemRenderer>().material = Resources.Load<Material>("Materials/PoisonParticleMaterial");
			poisonPS.transform.localScale = new Vector3((_entityFlip.m_FacingLeft ? 1 : -1), poisonPS.transform.localScale.y, poisonPS.transform.localScale.z);

			if (GetComponent<Entity>().EntityType == Entity.EntityTypes.Player)
				CameraFilter.Instance.Flash(Color.magenta, (1.75f), (0.45f));

			poisonPS.GetComponent<ParticleSystem>().Play();

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
			_health.TakeDamage(Mathf.Round(_burnDamagePerTick), "BurnStatus", false);
			++_currentBurnTick;

			SoundManager.Instance.Playsound(_burnSoundFxPath);

			// Burn Particles
			GameObject FirePS = Instantiate((Resources.Load("Prefabs/Effects/FirePS") as GameObject), transform.position, Quaternion.identity);
			FirePS.GetComponent<ParticleSystemRenderer>().material = Resources.Load<Material>("Materials/FireParticleMaterial");
			FirePS.transform.localScale = new Vector3((_entityFlip.m_FacingLeft ? 1 : -1), FirePS.transform.localScale.y, FirePS.transform.localScale.z);

			if (GetComponent<Entity>().EntityType == Entity.EntityTypes.Player)
				CameraFilter.Instance.Flash(Color.red, (1.75f), (0.45f));

			FirePS.GetComponent<ParticleSystem>().Play();

			if (_currentBurnTick >= _numberOfBurnTicks)
				_isBurned = false;
		}
	}

	public void ApplyShock(float damagePerTick, int numberOfTicks, float tickDuration = 1.0f)
	{
 		_shockDamagePerTick = damagePerTick;
		_numberOfShockTicks = numberOfTicks;
		_shockTickDuration = tickDuration;
		_currentShockTick = 0;

		_isShocked = true;
	}

	protected void HandleShock()
	{
		if (!_isShocked)
			return;

		if (Time.time > _shockTickTimer)
		{
			_shockTickTimer = Time.time + _shockTickDuration;

			// Deal Damage
			_health.TakeDamage(Mathf.Round(_shockDamagePerTick), "ShockStatus", false);
			++_currentShockTick;

			SoundManager.Instance.Playsound(_shockSoundFxPath);

			// Shock Particles
			GameObject ShockPS = Instantiate((Resources.Load("Prefabs/Effects/ShockPS") as GameObject), transform.position, Quaternion.identity);
			ShockPS.GetComponent<ParticleSystemRenderer>().material = Resources.Load<Material>("Materials/ShockParticleMaterial");
			ShockPS.transform.localScale = new Vector3((_entityFlip.m_FacingLeft ? 1 : -1), ShockPS.transform.localScale.y, ShockPS.transform.localScale.z);

			if (GetComponent<Entity>().EntityType == Entity.EntityTypes.Player)
				CameraFilter.Instance.Flash(Color.yellow, (1.75f), (0.45f));

			ShockPS.GetComponent<ParticleSystem>().Play();

			if (_currentShockTick >= _numberOfShockTicks)
				_isShocked = false;
		}
	}
}
