using Assets.Scripts.Skills.Magic;

public class LightningBolt : MagicSkill
{
    // Skillbar Helper Static
    public static float ResourceAmount = 0.5f;
    public static Resource ResourceType = Resource.Mana;

    public LightningBolt(Staff staffToUse) : base(staffToUse)
    {
        _stunTime = 2.5f;
        _staffToUse = staffToUse;
        _loadingTime = 0.1f;
        _knockBackAmount = 10f;
        _spritePath = "SkillIcons/LightningboltIcon";
        _projectilePrefabPath = "Prefabs/Projectiles/Lightningbolt";
        _soundPath = "Audio/SoundEffects/LightningBoltFx";
        _projectileCollisionsoundPath = "Audio/SoundEffects/BoltHitFx";
        _skillName = "Lightningbolt";

        _resourceAmount  = ResourceAmount;
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
        _damageAmount = _statManager.Intelligence.StatAmount * 1;

        base.UpdateDamage();
    }
}
