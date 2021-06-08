using Assets.Scripts.Skills.Magic;
using UnityEngine;

public class FireBolt : MagicSkill
{
    private void SetupSkill()
    {
        _skillName = this.GetType().Name;
        _iconName = _skillName + "Icon";
        _spritePath += _iconName;

        // Things To Update
        _projectilePrefabPath += "Firebolt";
        _soundPath += "FireBoltFx";
        _projectileCollisionsoundPath += "BoltHitFx";
        _toolTipInfo = "Burn foes with a 3 bolt blast of fire!";
        _stunTime = 1.2f;
        _knockBackAmount = 60f;
        _loadingTime = 0.75f;
        _loadingMovementSpeedModifier = 0.5f;
        _loadedMovementSpeedModifier = 0.0f;
        _resourceAmount = 5.0f;
        _resourceToUse = Resource.Mana;
        _weaponTypeToUse = WeaponType.Magic;

        /* This Is A Status Projectile Being Shot */
        _isStatusProjectile = true;
        _numberOfTicks = 5;
        _timePerTick = 0.5f;
    }

    public FireBolt() : base() { SetupSkill(); }

    public FireBolt(Weapon staffToUse) : base(staffToUse) { SetupSkill(); }

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
