using Assets.Scripts.Skills.Magic;
using UnityEngine;

public class FireBolt : MagicSkill
{
    public FireBolt() : base() { SetupBaseSkill(this.GetType().Name); }

    public FireBolt(Weapon staffToUse) : base(staffToUse) { SetupBaseSkill(this.GetType().Name); }

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

        Vector3 firstBoltSpawn = _staffToUse.ProjectileSpawnPosition;
        Vector3 secondBoltSpawn = _staffToUse.ProjectileSpawnPosition;

        firstBoltSpawn.y += 0.3f;
        secondBoltSpawn.y -= 0.3f;

        ShootProjectile(firstBoltSpawn);
        ShootProjectile(_staffToUse.ProjectileSpawnPosition);
        ShootProjectile(secondBoltSpawn);
    }

    public override void SetOwner(Entity anEntity)
    {
        base.SetOwner(anEntity);
    }

    protected override void UpdateDamage()
    {
	    _amountPerTick = _statManager.Intelligence.TotalAmount * 1 + (Random.Range(_staffToUse.WeaponInfo.MinDamage, _staffToUse.WeaponInfo.MaxDamage + 1) * 2);
	    _damageAmount = (int)(_amountPerTick);

        base.UpdateDamage();
    }
}
