using Assets.Scripts.Skills.Archery;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonArrow : ArcherySkill
{
	// Skillbar Helper Static
	public static float ResourceAmount = 5.0f;
	public static Resource ResourceType = Resource.Mana;

	public PoisonArrow(Bow bowToUse) : base(bowToUse)
	{
		_stunTime = 0.35f;
		_knockBackAmount = 20f;
		_loadingTime = 0.5f;
		_spritePath = "SkillIcons/PoisonArrowIcon";
		_projectilePrefabPath = "Prefabs/Projectiles/PoisonArrow";
		_soundPath = "Audio/SoundEffects/RangedAttackFx";
		_projectileCollisionsoundPath = "Audio/SoundEffects/ArrowHitFx";
		_skillName = "PoisonArrow";

		_resourceAmount = ResourceAmount;
		_resourceToUse = ResourceType;

		/* This Is A Status Projectile Being Shot */
		_isStatusProjectile = true;
		_numberOfTicks = 5;
		_amountPerTick = 0.025f;
		_timePerTick = 1.0f;

		SetProjectileGameObject(_projectilePrefabPath);
	}

	protected override void Execute()
	{
		base.Execute();

		_bowToUse.EvaluateProjectileSpawnPosition();
		_bowToUse.SpawnProjectile(_bowToUse.ProjectileSpawnPosition, _pooler);
	}

	public override void SetOwner(Entity anEntity)
	{
		base.SetOwner(anEntity);
	}

	protected override void UpdateDamage()
	{
		_damageAmount = _statManager.Dexterity.TotalAmount + _bowToUse.WeaponInfo.Damage;

		base.UpdateDamage();
	}
}