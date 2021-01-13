using Assets.Scripts.Skills.Magic;

public class IceBolt : MagicSkill
{
    // Skillbar Helper Static
    public static float ResourceAmount = 1.0f;
    public static Resource ResourceType = Resource.Mana;

    public IceBolt(Staff staffToUse) : base(staffToUse)
    {
        _stunTime = 0.8f;
        _staffToUse = staffToUse;
        _loadingTime = 0.4f;
        _knockBackAmount = 35f;
        _spritePath = "SkillIcons/IceboltIcon";
        _projectilePrefabPath = "Prefabs/Projectiles/Icebolt";
        _soundPath = "Audio/SoundEffects/IceBoltFx";
        _projectileCollisionsoundPath = "Audio/SoundEffects/BoltHitFx";
        _skillName = "Icebolt";

        _resourceAmount = ResourceAmount;
        _resourceToUse = ResourceType;

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

        _staffToUse.EvaluateProjectileSpawnPosition();
        _staffToUse.SpawnProjectile(_staffToUse.ProjectileSpawnPosition, _pooler);
    }

    public override void SetOwner(Entity anEntity)
    {
        base.SetOwner(anEntity);
    }

    protected override void UpdateDamage()
    {
        _damageAmount = _statManager.Intelligence.TotalAmount * 3 + _staffToUse.WeaponInfo.Damage;

        base.UpdateDamage();
    }
}
