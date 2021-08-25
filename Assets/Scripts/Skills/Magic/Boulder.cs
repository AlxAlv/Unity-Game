using Assets.Scripts.Skills.Magic;
using UnityEngine;

public class BoulderToss : MagicSkill
{
    public BoulderToss() : base() { SetupBaseSkill(this.GetType().Name); }

    public BoulderToss(Weapon staffToUse) : base(staffToUse) { SetupBaseSkill(this.GetType().Name); }

    public override void Update()
    {
        base.Update();
    }

    protected override void Execute()
    {
        base.Execute();

        _staffToUse.EvaluateProjectileSpawnPosition();
        ShootProjectile(_staffToUse.ProjectileSpawnPosition);
    }

    public override void SetOwner(Entity anEntity)
    {
        base.SetOwner(anEntity);
    }

    protected override void UpdateDamage()
    {
        _damageAmount = _statManager.Intelligence.TotalAmount * 10 + Random.Range(_staffToUse.WeaponInfo.MinDamage, _staffToUse.WeaponInfo.MaxDamage + 1) * 2;

        base.UpdateDamage();
    }
}
