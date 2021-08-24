using Assets.Scripts.Skills.Magic;
using UnityEngine;

public class IceBolt : MagicSkill
{
    public IceBolt() : base() { SetupBaseSkill(this.GetType().Name); }
    
    public IceBolt(Weapon staffToUse) : base(staffToUse){ SetupBaseSkill(this.GetType().Name); }

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

        _staffToUse.EvaluateProjectileSpawnPosition();
        ShootProjectile(_staffToUse.ProjectileSpawnPosition);
    }

    public override void SetOwner(Entity anEntity)
    {
        base.SetOwner(anEntity);
    }

    protected override void UpdateDamage()
    {
        _damageAmount = (_statManager.Intelligence.TotalAmount * 3) + (Random.Range(_staffToUse.WeaponInfo.MinDamage, _staffToUse.WeaponInfo.MaxDamage + 1) * 3);

        base.UpdateDamage();
    }
}
