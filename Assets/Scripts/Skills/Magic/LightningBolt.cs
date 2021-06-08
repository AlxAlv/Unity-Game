using Assets.Scripts.Skills.Magic;

public class LightningBolt : MagicSkill
{
    private void SetupSkill()
    {
        _skillName = this.GetType().Name;
        _iconName = _skillName + "Icon";
        _spritePath += _iconName;

        // Things To Update
        _projectilePrefabPath += "Lightningbolt";
        _soundPath += "LightningBoltFx";
        _projectileCollisionsoundPath += "BoltHitFx";
        _toolTipInfo = "Shoot a ball of lightning to stun an enemy!";
        _stunTime = 2.0f;
        _knockBackAmount = 10f;
        _loadingTime = 0.15f;
        _loadingMovementSpeedModifier = 0.5f;
        _loadedMovementSpeedModifier = 0.0f;
        _resourceAmount = 0.5f;
        _resourceToUse = Resource.Mana;
        _weaponTypeToUse = WeaponType.Magic;
    }

    public LightningBolt() : base() { SetupSkill(); }

    public LightningBolt(Weapon staffToUse) : base(staffToUse) { SetupSkill(); }

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
        _damageAmount = _statManager.Intelligence.TotalAmount * 2;

        base.UpdateDamage();
    }
}
