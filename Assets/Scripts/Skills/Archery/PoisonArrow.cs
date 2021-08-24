using Assets.Scripts.Skills.Archery;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonArrow : ArcherySkill
{
	public PoisonArrow() : base() { SetupBaseSkill(this.GetType().Name); }

	public PoisonArrow(Weapon bowToUse) : base(bowToUse) { SetupBaseSkill(this.GetType().Name); }

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