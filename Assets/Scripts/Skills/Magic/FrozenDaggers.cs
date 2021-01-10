using Assets.Scripts.Skills.Magic;
using UnityEngine;
using UnityEngine.PlayerLoop;

enum ChargingStatus { NotCharging, Charging}

public class FrozenDaggers : MagicSkill
{
	// Skillbar Helper Static
	public static float ResourceAmount = 0.5f;
	public static Resource ResourceType = Resource.Mana;

	private int _currentNumOfDaggers = 0;
	private int _maxNumOfDaggers = 50;

	// Rapid Fire Timers
	private float _timer = 0.0f;
	private float _timerDuration = .10f;

	// Charging Timer
	private float _chargeTimer = 0.0f;
	private float _extraChargeTime = 0.1f;

	private int _mouseButton = 0;

	private ChargingStatus _chargingStatus = ChargingStatus.NotCharging;

	public FrozenDaggers(Staff staffToUse) : base(staffToUse)
	{
		_loadingMovementSpeedModifier = 1.0f;
		_loadedMovementSpeedModifier = 1.0f;

		_stunTime = 0.25f;
		_staffToUse = staffToUse;
		_loadingTime = 1.0f;
		_knockBackAmount = 15f;
		_spritePath = "SkillIcons/FrozenDaggersIcon";
		_projectilePrefabPath = "Prefabs/Projectiles/FrozenDaggersPrefab";
		_soundPath = "Audio/SoundEffects/IceBoltFx";
		_projectileCollisionsoundPath = "Audio/SoundEffects/BoltHitFx";
		_skillName = "FrozenDaggers";

		_resourceAmount = ResourceAmount;
		_resourceToUse = ResourceType;

		SetProjectileGameObject(_projectilePrefabPath);
	}

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

			_staffToUse.PlayAnimation();
			_staffToUse.EvaluateProjectileSpawnPosition();
			_staffToUse.SpawnProjectile(_staffToUse.ProjectileSpawnPosition, _pooler);

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
		_damageAmount = _statManager.Intelligence.StatAmount * 3 + (_staffToUse.WeaponInfo.Damage / 3);

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