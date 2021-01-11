using Assets.Scripts.Skills.Archery;

public class EnemyRangedAttack : ArcherySkill
{
    public EnemyRangedAttack(Bow bowToUse) : base(bowToUse)
    {
        _stunTime = 0.6f;
        _knockBackAmount = 40f;
        _loadingTime = 1.0f;
        _spritePath = "SkillIcons/RangedAttackIcon";
        _projectilePrefabPath = "Prefabs/Projectiles/Arrow";
        _soundPath = "Audio/SoundEffects/RangedAttackFx";
        _projectileCollisionsoundPath = "Audio/SoundEffects/ArrowHitFx";
        _skillName = "RangedAttack";


        _resourceAmount = 1.0f;
        _resourceToUse = Resource.Stamina;

        SetProjectileGameObject(_projectilePrefabPath);
    }

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
        _bowToUse.SpawnProjectile(_bowToUse.ProjectileSpawnPosition, _pooler);
    }

    public override void SetOwner(Entity anEntity)
    {
        base.SetOwner(anEntity);
    }

    protected override void UpdateDamage()
    {
        _damageAmount = _statManager.Dexterity.TotalAmount * 2 + _bowToUse.WeaponInfo.Damage;

        base.UpdateDamage();
    }
}
