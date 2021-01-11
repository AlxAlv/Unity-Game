using Assets.Scripts.Skills.Magic;

public class BoulderToss : MagicSkill
{
    // Skillbar Helper Static
    public static float ResourceAmount = 1.0f;
    public static Resource ResourceType = Resource.Mana;

    public BoulderToss(Staff staffToUse) : base(staffToUse)
    {
        _loadingMovementSpeedModifier = 0.0f;
        _stunTime = 2.0f;
        _staffToUse = staffToUse;
        _loadingTime = 1.0f;
        _knockBackAmount = 75f;
        _spritePath = "SkillIcons/BoulderTossIcon";
        _projectilePrefabPath = "Prefabs/Projectiles/BoulderTossPrefab";
        _soundPath = "Audio/SoundEffects/BoulderTossFx";
        _projectileCollisionsoundPath = "Audio/SoundEffects/BoltHitFx";
        _skillName = "BoulderToss";

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
        _damageAmount = _statManager.Intelligence.TotalAmount * 5 + _staffToUse.WeaponInfo.Damage;

        base.UpdateDamage();
    }
}
