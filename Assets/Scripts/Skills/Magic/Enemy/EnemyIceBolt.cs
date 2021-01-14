using Assets.Scripts.Skills.Magic;

public class EnemyIceBolt : MagicSkill
{
    // Skillbar Helper Static
    public static float ResourceAmount = 1.0f;
    public static Resource ResourceType = Resource.Mana;

    public EnemyIceBolt(Staff staffToUse) : base(staffToUse)
    {
        _stunTime = 0.5f;
        _staffToUse = staffToUse;
        _loadingTime = .8f;
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

    protected override void Execute()
    {
        base.Execute();

        _staffToUse.EvaluateProjectileSpawnPosition();
        _staffToUse.SpawnProjectile(_staffToUse.ProjectileSpawnPosition, _pooler);
    }

    protected override void UpdateDamage()
    {
        _damageAmount = _statManager.Intelligence.TotalAmount * 1 + _staffToUse.WeaponInfo.Damage;

        base.UpdateDamage();
    }
}
