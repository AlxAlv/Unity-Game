using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Skills.Melee;
using UnityEngine;

public class Backstab : MeleeSkill
{
    // Backstab Specific
    private float _backstabDistance = 15.0f;

    public Backstab() : base() { SetupBaseSkill(this.GetType().Name); }

    public Backstab(Weapon swordToUse) : base(swordToUse) { SetupBaseSkill(this.GetType().Name); }

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
