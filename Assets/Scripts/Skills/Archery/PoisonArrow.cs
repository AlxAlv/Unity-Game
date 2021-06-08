using Assets.Scripts.Skills.Archery;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonArrow : ArcherySkill
{
	private void SetupSkill()
	{
		_skillName = this.GetType().Name;
		_iconName = _skillName + "Icon";
		_spritePath += _iconName;

		// Things To Update
		_projectilePrefabPath += "PoisonArrow";
		_soundPath += "RangedAttackFx";
		_projectileCollisionsoundPath += "ArrowHitFx";
		_toolTipInfo = "Shoot an arrow covered in poison!";
		_stunTime = 0.35f;
		_knockBackAmount = 20f;
		_loadingTime = 0.5f;
		_loadingMovementSpeedModifier = 0.5f;
		_loadedMovementSpeedModifier = 0.0f;
		_resourceAmount = 5.0f;
		_resourceToUse = Resource.Mana;
		_weaponTypeToUse = WeaponType.Bow;

		// Skill Specific
		/* This Is A Status Projectile Being Shot */
		_isStatusProjectile = true;
		_numberOfTicks = 5;
		_amountPerTick = 0.025f;
		_timePerTick = 1.0f;
	}

	public PoisonArrow() : base() { SetupSkill(); }

	public PoisonArrow(Weapon bowToUse) : base(bowToUse) { SetupSkill(); }

	protected override void Execute()
	{
		base.Execute();

		_bowToUse.EvaluateProjectileSpawnPosition();
		ShootProjectile(_bowToUse.ProjectileSpawnPosition);
	}

	public override void SetOwner(Entity anEntity)
	{
		base.SetOwner(anEntity);
	}

	protected override void UpdateDamage()
	{
		_damageAmount = _statManager.Dexterity.TotalAmount + Random.Range(_bowToUse.WeaponInfo.MinDamage, _bowToUse.WeaponInfo.MaxDamage + 1);

		base.UpdateDamage();
	}
}