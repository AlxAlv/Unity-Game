using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Skills.Melee;
using UnityEngine;

public class Charge : MeleeSkill
{
    // Skillbar Helper Static
    public static float ResourceAmount = 3.0f;
    public static Resource ResourceType = Resource.Stamina;

    // Charge Specific
    private float _chargeDistance = 15.0f;
    private float _chargeSpeedModifier = 6.0f;

    public Charge(Sword swordToUse) : base(swordToUse)
    {
	    _skillName = "ChargeAttack";
	    _distanceToAttack = 1.5f;
        _stunTime = 2.0f;
        _knockBackAmount = 100f;
        _loadingTime = 0.15f;
        _loadingMovementSpeedModifier = 1.0f;
        _loadedMovementSpeedModifier = 1.0f;
        _spritePath = "SkillIcons/ChargeIcon";
        _soundPath = "Audio/SoundEffects/ChargeFx";

        _resourceAmount = ResourceAmount;
        _resourceToUse = ResourceType;
    }

    public override void Update()
    {
	    if (IsLoaded() && IsInChargeReach() && _pendingAttack)
		    _entityMovement.RunMovementModifier = _chargeSpeedModifier;

	    base.Update();
    }

    public override void CancelSkill()
    {
	    _entityMovement.RunMovementModifier = 1.0f;

	    base.CancelSkill();
    }

    protected override void UpdateDamage()
    {
        _damageAmount = _statManager.Strength.TotalAmount * 1;

        base.UpdateDamage();
    }

    protected bool IsInChargeReach()
    {
	    if (_entityTarget.CurrentTarget != null)
	    {
		    float distance = Vector3.Distance(_entityTarget.CurrentTarget.transform.position, _entity.transform.position);

		    return (distance < _chargeDistance);
	    }

	    return false;
    }
}
