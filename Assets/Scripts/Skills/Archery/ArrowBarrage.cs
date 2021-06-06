using Assets.Scripts.Skills.Archery;
using UnityEngine;

public class ArrowBarrage : ArcherySkill
{
    // Skillbar Helper Static
    public static float ResourceAmount = 25.0f;
    public static Resource ResourceType = Resource.Ultimate;

    // Private Members
    private int _arrowsShot = 0;
    private int _arrowsToShoot = 20;

    private float _timeBetweenArrows = 0.05f;
    private float _timer;


    public ArrowBarrage(Bow bowToUse) : base(bowToUse)
    {
        _stunTime = 1.0f;
        _knockBackAmount = 50f;
        _loadingTime = (0.0f);
        _loadingMovementSpeedModifier = 1.0f;
        _spritePath = "SkillIcons/ArrowBarrageIcon";
        _projectilePrefabPath = "Prefabs/Projectiles/Arrow";
        _soundPath = "Audio/SoundEffects/RangedAttackFx";
        _projectileCollisionsoundPath = "Audio/SoundEffects/ArrowHitFx";
        _skillName = "RangedAttack";


        _resourceAmount = ResourceAmount;
        _resourceToUse = ResourceType;
    }

    private bool Timer()
    {
        if (_timer < _timeBetweenArrows)
        {
            _timer += Time.deltaTime;
            return false;
        }
        else
        {
            _timer = 0.0f;
            return true;
        }
    }

    protected override void Awake()
    {
        base.Awake();
    }

    public override void Update()
    {
        base.Update();

        if (_isExecutingSkill && Timer())
            Execute();
    }

    public override void Trigger()
    {
        base.Trigger();
    }

    protected override void Execute()
    {
        if (_arrowsShot < _arrowsToShoot)
        {
            if (!_isExecutingSkill)
                _timer = 0.0f;

            _isExecutingSkill = true;
            SoundManager.Instance.Playsound(_soundPath);
        }
        else
        {
            _isExecutingSkill = false;
            base.Execute(); 
        }

        _bowToUse.EvaluateProjectileSpawnPosition();
        ShootProjectile(_bowToUse.ProjectileSpawnPosition);
        _arrowsShot++;
    }

    protected override void SkillLoaded()
    {
        base.SkillLoaded();

        _arrowsShot = 0;
    }

    public override void SetOwner(Entity anEntity)
    {
        base.SetOwner(anEntity);
    }

    protected override void UpdateDamage()
    {
        _damageAmount = _statManager.Dexterity.TotalAmount * 50 + (Random.Range(_bowToUse.WeaponInfo.MinDamage, _bowToUse.WeaponInfo.MaxDamage + 1)) * 50;

        base.UpdateDamage();
    }
}
