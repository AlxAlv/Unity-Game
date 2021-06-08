using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Skills.Melee;
using UnityEngine;

public class Backstab : MeleeSkill
{
    // Backstab Specific
    private float _backstabDistance = 15.0f;

    private void SetupSkill()
    {
        _skillName = this.GetType().Name;
        _iconName = _skillName + "Icon";
        _spritePath += _iconName;

        // Things To Update
        _soundPath += "StabFx";
        _toolTipInfo = "Teleport behind an enemy and stab them!";
        _stunTime = 1.0f;
        _knockBackAmount = 10f;
        _loadingTime = 0.05f;
        _loadingMovementSpeedModifier = 1.0f;
        _loadedMovementSpeedModifier = 1.0f;
        _resourceAmount = 10.0f;
        _resourceToUse = Resource.Stamina;
        _weaponTypeToUse = WeaponType.Melee;

        // Skill Specific
        _distanceToAttack = 1.5f;
    }

    public Backstab() : base() { SetupSkill(); }

    public Backstab(Weapon swordToUse) : base(swordToUse) { SetupSkill(); }

    public override void Update()
    {
        if (IsLoaded() && IsBackstabInReach() && _pendingAttack)
        {
            _entity.gameObject.transform.position = _entityTarget.CurrentTarget.transform.position;
        }

        base.Update();
    }

    public override void CancelSkill()
    {
        base.CancelSkill();
    }

    protected override void UpdateDamage()
    {
        _damageAmount = _statManager.Strength.TotalAmount * 10;

        base.UpdateDamage();
    }

    protected bool IsBackstabInReach()
    {
        if (_entityTarget.CurrentTarget != null)
        {
            float distance = Vector3.Distance(_entityTarget.CurrentTarget.transform.position, _entity.transform.position);

            return (distance < _backstabDistance);
        }

        return false;
    }
}
