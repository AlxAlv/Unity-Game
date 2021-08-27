using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Skills.Melee;
using UnityEngine;

public class Charge : MeleeSkill
{
    // Charge Specific
    private float _chargeDistance = 15.0f;
    private float _chargeSpeedModifier = 6.0f;

    private void SetupSkill()
    {
        _skillName = this.GetType().Name;
        _iconName = _skillName + "Icon";
        _spritePath += _iconName;

        // Things To Update
        _soundPath += "ChargeFx";
        _toolTipInfo = "Charge towards an enemy and knock them back!";
        _stunTime = 2.0f;
        _knockBackAmount = 100f;
        _loadingTime = 0.15f;
        _loadingMovementSpeedModifier = 1.0f;
        _loadedMovementSpeedModifier = 1.0f;
        _resourceAmount = 3.0f;
        _resourceToUse = Resource.Stamina;
        _weaponTypeToUse = WeaponType.Melee;

        // Skill Specific
        _distanceToAttack = 1.5f;
    }

    public Charge() : base() { SetupSkill(); }

    public Charge(Weapon swordToUse) : base(swordToUse) { SetupSkill(); }

	public override void Trigger()
	{
        _pendingAttack = true;
        base.Trigger();
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
