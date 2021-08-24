using Assets.Scripts.Skills.Magic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : MagicSkill
{
    public Heal() : base() { SetupBaseSkill(this.GetType().Name); }

    public Heal(Weapon staffToUse) : base(staffToUse) { SetupBaseSkill(this.GetType().Name); }

    protected override void Awake()
    {
        base.Awake();
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Trigger()
    {
        base.Trigger();
    }

    protected override void Execute()
    {
        base.Execute();
        _health.GainHealth(_damageAmount);
    }

    public override void SetOwner(Entity anEntity)
    {
        base.SetOwner(anEntity);
    }

    protected override void UpdateDamage()
    {
        _damageAmount = _statManager.Intelligence.TotalAmount * 5 + Random.Range(_staffToUse.WeaponInfo.MinDamage, _staffToUse.WeaponInfo.MaxDamage + 1);

        base.UpdateDamage();
    }

    protected override void SkillLoaded()
    {
        base.SkillLoaded();
        Execute();
    }
}
