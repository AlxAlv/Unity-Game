﻿using Assets.Scripts.Skills.Magic;
using UnityEngine;
using UnityEngine.PlayerLoop;

enum ChargingStatus { NotCharging, Charging}

public class FrozenDaggers : MagicSkill
{
	private int _currentNumOfDaggers = 0;
	private int _maxNumOfDaggers = 50;

	// Rapid Fire Timers
	private float _timer = 0.0f;
	private float _timerDuration = .10f;

	// Charging Timer
	private float _chargeTimer = 0.0f;
	private float _extraChargeTime = (0.05f);

	private int _mouseButton = 0;

	private ChargingStatus _chargingStatus = ChargingStatus.NotCharging;

	public FrozenDaggers() : base() { SetupBaseSkill(this.GetType().Name); }

	public FrozenDaggers(Weapon staffToUse) : base(staffToUse) { SetupBaseSkill(this.GetType().Name); }

	protected override void Awake()
	{
		base.Awake();
	}

	public override void Update()
	{
		if (IsLoaded())
			_entitySkill.SkillUses.text = _currentNumOfDaggers.ToString();

		HandleInput();
		HandleCharging();

		base.Update();
	}

	public override void Trigger()
	{
		if ((_chargingStatus == ChargingStatus.NotCharging))
		{
			_mouseButton = (Input.GetMouseButton(0) ? 0 : 1);
		}

		base.Trigger();
	}

	protected override void SkillLoaded()
	{
		if ((_chargingStatus == ChargingStatus.NotCharging) && _currentNumOfDaggers == 0)
		{
			_currentNumOfDaggers = 1;

			if (Input.GetMouseButton(_mouseButton))
				_chargingStatus = ChargingStatus.Charging;
		}

		base.SkillLoaded();
	}

	protected override void Execute()
	{
		if ((_timer == 0.0f) || (Time.time > _timer))
		{
			_timer = Time.time + _timerDuration;

			SoundManager.Instance.Playsound(_soundPath);
			_currentNumOfDaggers--;

			_staffToUse.EvaluateProjectileSpawnPosition();
			ShootProjectile(_staffToUse.ProjectileSpawnPosition);

			if (_currentNumOfDaggers == 0)
			{
				base.Execute();
			}
		}
	}

	public override void SetOwner(Entity anEntity)
	{
		base.SetOwner(anEntity);
	}

	protected override void UpdateDamage()
	{
		_damageAmount = (_statManager.Intelligence.TotalAmount * 2) + (Random.Range(_staffToUse.WeaponInfo.MinDamage, _staffToUse.WeaponInfo.MaxDamage + 1));

		base.UpdateDamage();
	}

	private void HandleInput()
	{
		if (!Input.GetMouseButton(_mouseButton))
			_chargingStatus = ChargingStatus.NotCharging;
	}

	private void HandleCharging()
	{
		if (((_chargeTimer == 0.0f) || (Time.time > _chargeTimer)) && _chargingStatus == ChargingStatus.Charging)
		{
			_chargeTimer = Time.time + _extraChargeTime;
			_currentNumOfDaggers++;

			if (!UseAbilityResource(ResourceAmount) || (_currentNumOfDaggers >= _maxNumOfDaggers))
			{
				_chargingStatus = ChargingStatus.NotCharging;
			}
		}
	}
}