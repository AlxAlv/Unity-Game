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

	private void SetupSkill()
	{
		_skillName = this.GetType().Name;
		_iconName = _skillName + "Icon";
		_spritePath += _iconName;

		// Things To Update
		_soundPath += "ChargeFx";
		_toolTipInfo = "Perform a massive leap causing AOE damage on impact!";
		_stunTime = 2.0f;
		_knockBackAmount = 100f;
		_loadingTime = 0.5f;
		_loadingMovementSpeedModifier = 0.2f;
		_loadedMovementSpeedModifier = 0.35f;
		_resourceAmount = 8.0f;
		_resourceToUse = Resource.Stamina;
		_weaponTypeToUse = WeaponType.Melee;

		// Skill Specific
		_outlineRadius = 6.5f;
		_isSkillShot = true;
		_distanceToAttack = 1.5f;
	}

	public SkyFall() : base() { SetupSkill(); }

	public SkyFall(Weapon swordToUse) : base(swordToUse) { SetupSkill(); }

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
		_damageAmount = (_statManager.Strength.TotalAmount * 4) + (Random.Range(_swordToUse.WeaponInfo.MinDamage, _swordToUse.WeaponInfo.MaxDamage + 1) * 3);

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