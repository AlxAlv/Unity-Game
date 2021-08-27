using Assets.Scripts.Skills.Magic;

public class LightningBolt : MagicSkill
{
    public LightningBolt() : base() { SetupBaseSkill(this.GetType().Name); }

    public LightningBolt(Weapon staffToUse) : base(staffToUse) { SetupBaseSkill(this.GetType().Name); }

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
        _amountPerTick = _statManager.Intelligence.TotalAmount * 1;
        _damageAmount = _statManager.Intelligence.TotalAmount * 2;

        base.UpdateDamage();
    }
}
