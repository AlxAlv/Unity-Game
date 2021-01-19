using System;
using Assets.Scripts.Skills.Melee;
using UnityEngine;
using Random = UnityEngine.Random;

public class SkyFall : MeleeSkill
{
	// Skillbar Helper Static
	public static float ResourceAmount = 8.0f;
	public static Resource ResourceType = Resource.Stamina;

	// SkyFall Specific
	private float _skyFallDistance = 20.0f;
	private float _skyFallSpeedModifier = 4.0f;
	private string _skyFallImpactSoundPath = "Audio/SoundEffects/SkyFallFx";

	public SkyFall(Sword swordToUse) : base(swordToUse)
	{
		_isSkillShot = true;
		_skillName = "SkyFall";
		_distanceToAttack = 1.5f;
		_stunTime = 2.0f;
		_knockBackAmount = 100f;
		_loadingTime = 0.5f;
		_loadingMovementSpeedModifier = 0.20f;
		_loadedMovementSpeedModifier = 0.35f;

		/* AOE Outliner */
		_outlineRadius = 6.5f;

		_spritePath = "SkillIcons/SkyFallIcon";
		_soundPath = "Audio/SoundEffects/ChargeFx";

		_resourceAmount = ResourceAmount;
		_resourceToUse = ResourceType;
	}

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