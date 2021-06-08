using Assets.Scripts.Skills.Magic;
using UnityEngine;

public class BoulderToss : MagicSkill
{
    private void SetupSkill()
    {
        _skillName = this.GetType().Name;
        _iconName = _skillName + "Icon";
        _spritePath += _iconName;

        // Things To Update
        _projectilePrefabPath += "BoulderTossPrefab";
        _soundPath += "BoulderTossFx";
        _projectileCollisionsoundPath += "SkyFallFx";
        _toolTipInfo = "Throw a large boulder for massive AOE damage!";
        _stunTime = 2.0f;
        _knockBackAmount = 100f;
        _loadingTime = 1.0f;
        _loadingMovementSpeedModifier = 0.5f;
        _loadedMovementSpeedModifier = 0.0f;
        _resourceAmount = 10.0f;
        _resourceToUse = Resource.Mana;
        _weaponTypeToUse = WeaponType.Magic;

        /* Skill Specific */
        _isAOEProjectile = true;
        _outlineRadius = 6.5f;
    }

    public BoulderToss() : base() { SetupSkill(); }

    public BoulderToss(Weapon staffToUse) : base(staffToUse) { SetupSkill(); }

    public override void Update()
    {
	    UpdateOutlineRenderer();

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
