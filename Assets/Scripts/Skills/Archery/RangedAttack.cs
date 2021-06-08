using Assets.Scripts.Skills.Archery;
using UnityEngine;

public class RangedAttack : ArcherySkill
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
        _toolTipInfo = "Shoot an arrow!";
        _stunTime = 1.0f;
        _knockBackAmount = 50f;
        _loadingTime = 0.3f;
        _loadingMovementSpeedModifier = 0.5f;
        _loadedMovementSpeedModifier = 0.0f;
        _resourceAmount = 1.0f;
        _resourceToUse = Resource.Stamina;
        _weaponTypeToUse = WeaponType.Bow;
    }

    public RangedAttack() : base() { SetupSkill(); }

    public RangedAttack(Weapon bowToUse) : base(bowToUse) { SetupSkill(); }

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
        _damageAmount = _statManager.Dexterity.TotalAmount * 5 + (Random.Range(_bowToUse.WeaponInfo.MinDamage, _bowToUse.WeaponInfo.MaxDamage + 1)) * 5;

        base.UpdateDamage();
    }
}
