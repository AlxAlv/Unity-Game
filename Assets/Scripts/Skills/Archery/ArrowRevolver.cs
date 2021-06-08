using Assets.Scripts.Skills.Archery;
using UnityEngine;

public class ArrowRevolver : ArcherySkill
{
	// Skill Specific
	private const int NUMBER_OF_ARROWS = 5;
	private int _currentNumOfArrows = 0;
	private float _timer = 0.0f;
	private float _timerDuration = (.05f);

	private void SetupSkill()
	{
		_skillName = this.GetType().Name;
		_iconName = _skillName + "Icon";
		_spritePath += _iconName;

		// Things To Update
		_projectilePrefabPath += "Arrow";
		_soundPath += "RangedAttackFx";
		_projectileCollisionsoundPath += "ArrowHitFx";
		_toolTipInfo = "Load up 5 arrows to shoot!";
		_stunTime = 0.8f;
		_knockBackAmount = 25f;
		_loadingTime = 0.8f;
		_loadingMovementSpeedModifier = 0.8f;
		_loadedMovementSpeedModifier = 0.0f;
		_resourceAmount = 5.0f;
		_resourceToUse = Resource.Stamina;
		_weaponTypeToUse = WeaponType.Bow;
	}

	public ArrowRevolver() : base() { SetupSkill(); }

	public ArrowRevolver(Weapon bowToUse) : base(bowToUse) { SetupSkill(); }

	protected override void Awake()
	{
		base.Awake();
	}

	public override void Update()
	{
		base.Update();

		if (IsLoaded())
			_entitySkill.SkillUses.text = _currentNumOfArrows.ToString();
	}

	public override void Trigger()
	{
		base.Trigger();
	}

	protected override void SkillLoaded()
	{
		base.SkillLoaded();
		_currentNumOfArrows = NUMBER_OF_ARROWS;
	}

	protected override void Execute()
	{
		if ((_timer == 0.0f) || (Time.time > _timer))
		{
			_timer = Time.time + _timerDuration;

			SoundManager.Instance.Playsound(_soundPath);
			_currentNumOfArrows--;

			_bowToUse.EvaluateProjectileSpawnPosition();
			ShootProjectile(_bowToUse.ProjectileSpawnPosition);

			if (_currentNumOfArrows == 0)
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
		_damageAmount = _statManager.Dexterity.TotalAmount * 3 + (Random.Range(_bowToUse.WeaponInfo.MinDamage, _bowToUse.WeaponInfo.MaxDamage + 1)) * 5;

		base.UpdateDamage();
	}
}
