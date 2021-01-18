using Assets.Scripts.Skills.Archery;
using UnityEngine;

public class ArrowRevolver : ArcherySkill
{
	// Skillbar Helper Static
	public static float ResourceAmount = 6.0f;
	public static Resource ResourceType = Resource.Stamina;

	private const int NUMBER_OF_ARROWS = 5;
	private int _currentNumOfArrows = 0;

	private float _timer = 0.0f;
	private float _timerDuration = (.05f);

	public ArrowRevolver(Bow bowToUse) : base(bowToUse)
	{
		_stunTime = 0.8f;
		_knockBackAmount = 25f;
		_bowToUse = bowToUse;
		_loadingTime = .8f;
		_spritePath = "SkillIcons/ArrowRevolverIcon";
		_projectilePrefabPath = "Prefabs/Projectiles/Arrow";
		_soundPath = "Audio/SoundEffects/RangedAttackFx";
		_projectileCollisionsoundPath = "Audio/SoundEffects/ArrowHitFx";
		_skillName = "ArrowRevolver";

		_resourceAmount = ResourceAmount;
		_resourceToUse = Resource.Stamina;
	}

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
		_damageAmount = _statManager.Dexterity.TotalAmount * 2 + Random.Range(_bowToUse.WeaponInfo.MinDamage, _bowToUse.WeaponInfo.MaxDamage + 1);

		base.UpdateDamage();
	}
}
