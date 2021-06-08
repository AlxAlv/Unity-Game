using Assets.Scripts.Skills.Archery;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargedShot : ArcherySkill
{
    private void SetupSkill()
    {
        _skillName = this.GetType().Name;
        _iconName = _skillName + "Icon";
        _spritePath += _iconName;

        // Things To Update
        _projectilePrefabPath += "Arrow";
        _soundPath += "RangedAttackFx";
        _projectileCollisionsoundPath += "ArrowHitFx";
        _toolTipInfo = "Shoot a devastating arrow!";
        _stunTime = 1.0f;
        _knockBackAmount = 100f;
        _loadingTime = 1.0f;
        _loadingMovementSpeedModifier = 0.5f;
        _loadedMovementSpeedModifier = 0.0f;
        _resourceAmount = 10.0f;
        _resourceToUse = Resource.Stamina;
        _weaponTypeToUse = WeaponType.Bow;
    }

    public ChargedShot() : base() { SetupSkill(); }

    public ChargedShot(Weapon bowToUse) : base(bowToUse) { SetupSkill(); }

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