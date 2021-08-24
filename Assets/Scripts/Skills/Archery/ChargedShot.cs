using Assets.Scripts.Skills.Archery;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargedShot : ArcherySkill
{
    public ChargedShot() : base() { _skillName = this.GetType().Name; }

    public ChargedShot(Weapon bowToUse) : base(bowToUse) { _skillName = this.GetType().Name; }

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

        _bowToUse.EvaluateProjectileSpawnPosition();
        ShootProjectile(_bowToUse.ProjectileSpawnPosition);
    }

    public override void SetOwner(Entity anEntity)
    {
        base.SetOwner(anEntity);
    }

    protected override void UpdateDamage()
    {
        _damageAmount = _statManager.Dexterity.TotalAmount * 5 + Random.Range(_bowToUse.WeaponInfo.MinDamage, _bowToUse.WeaponInfo.MaxDamage + 1) * 15;

        base.UpdateDamage();
    }
}