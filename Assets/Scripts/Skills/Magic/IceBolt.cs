using Assets.Scripts.Skills.Magic;
using UnityEngine;

public class IceBolt : MagicSkill
{
    private void SetupSkill()
    {
        _skillName = this.GetType().Name;
        _iconName = _skillName + "Icon";
        _spritePath += _iconName;

        // Things To Update
        _projectilePrefabPath += "Icebolt";
        _soundPath += "IceBoltFx";
        _projectileCollisionsoundPath += "BoltHitFx";
        _toolTipInfo = "Shoot a ball made of ice!";
        _stunTime = 0.8f;
        _knockBackAmount = 35f;
        _loadingTime = 0.25f;
        _loadingMovementSpeedModifier = 0.5f;
        _loadedMovementSpeedModifier = 0.0f;
        _resourceAmount = 1.0f;
        _resourceToUse = Resource.Mana;
        _weaponTypeToUse = WeaponType.Magic;
    }

    public IceBolt() : base() { SetupSkill(); }
    
    public IceBolt(Weapon staffToUse) : base(staffToUse){ SetupSkill(); }

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
