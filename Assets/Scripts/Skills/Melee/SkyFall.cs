using System;
using Assets.Scripts.Skills.Melee;
using UnityEngine;
using Random = UnityEngine.Random;

public class SkyFall : MeleeSkill
{
	// SkyFall Specific
	private float _skyFallDistance = 20.0f;
	private float _skyFallSpeedModifier = 4.0f;
	private string _skyFallImpactSoundPath = "Audio/SoundEffects/SkyFallFx";

	public SkyFall() : base() { SetupBaseSkill(this.GetType().Name); }

	public SkyFall(Weapon swordToUse) : base(swordToUse) { SetupBaseSkill(this.GetType().Name); }

	public override void Update()
	{
		UpdateOutlineRenderer();

		if (IsLoaded() && IsInSkyFallReach() && _pendingAttack)
		{
			_entityMovement.SkillMovementModifier = 1.0f;
			_entityMovement.RunMovementModifier = _skyFallSpeedModifier;
		}

		base.Update();
	}

	public override void CancelSkill()
	{
		_entityMovement.RunMovementModifier = 1.0f;

		base.CancelSkill();
	}

	protected override void Execute()
	{
		if (IsInReach() && IsLoaded() && _pendingAttack)
		{
			SoundManager.Instance.Playsound(_skyFallImpactSoundPath);

			_pendingAttack = false;

			TriggerGameJuice();
			ExecuteAOESkill();
		}
	}

	protected override void UpdateDamage()
	{
		_damageAmount = (_statManager.Strength.TotalAmount * 4) + (Random.Range(_swordToUse.WeaponInfo.MinDamage, _swordToUse.WeaponInfo.MaxDamage + 5) * 3);

		base.UpdateDamage();
	}

	protected bool IsInSkyFallReach()
	{
		if (_entityTarget.CurrentTarget != null)
		{
			float distance = Vector3.Distance(_entityTarget.CurrentTarget.transform.position, _entity.transform.position);

			return (distance < _skyFallDistance);
		}

		return false;
	}
}