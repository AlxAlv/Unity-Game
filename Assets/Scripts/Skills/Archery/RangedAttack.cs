using Assets.Scripts.Skills.Archery;

public class RangedAttack : ArcherySkill
{
    // Skillbar Helper Static
    public static float ResourceAmount = 1.0f;
    public static Resource ResourceType = Resource.Stamina;

    public RangedAttack(Bow bowToUse) : base(bowToUse)
	{
        _stunTime = 1.0f;
        _knockBackAmount = 50f;
        _loadingTime = 0.4f;
        _loadingMovementSpeedModifier = 1.0f;
        _spritePath = "SkillIcons/RangedAttackIcon";
        _projectilePrefabPath = "Prefabs/Projectiles/Arrow";
        _soundPath = "Audio/SoundEffects/RangedAttackFx";
        _projectileCollisionsoundPath = "Audio/SoundEffects/ArrowHitFx";
        _skillName = "RangedAttack";


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

		_bowToUse.EvaluateProjectileSpawnPosition();
		_bowToUse.SpawnProjectile(_bowToUse.ProjectileSpawnPosition, _pooler);
    } 

    public override void SetOwner(Entity anEntity)
    {
        base.SetOwner(anEntity);
    }

    protected override void UpdateDamage()
    {
        _damageAmount = _statManager.Dexterity.StatAmount * 2 + _bowToUse.WeaponInfo.Damage;

        base.UpdateDamage();
    }
}
