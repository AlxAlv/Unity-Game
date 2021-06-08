using Assets.Scripts.Skills.Archery;
using UnityEngine;

public class ArrowBarrage : ArcherySkill
{
    // Skill Specific
    private int _arrowsShot = 0;
    private int _arrowsToShoot = 20;
    private float _timeBetweenArrows = 0.05f;
    private float _timer;

    private void SetupSkill()
    {
        _skillName = this.GetType().Name;
        _iconName = _skillName + "Icon";
        _spritePath += _iconName;
        _resourceAmount = ResourceAmount;
        _resourceToUse = ResourceType;

        // Things To Update
        _projectilePrefabPath += "Arrow";
        _soundPath += "RangedAttackFx";
        _projectileCollisionsoundPath += "ArrowHitFx";
        _toolTipInfo = "Unleash a barrage of arrows!";
        _stunTime = 1.0f;
        _knockBackAmount = 50f;
        _loadingTime = 0.0f;
        _loadingMovementSpeedModifier = 1.0f;
        _loadedMovementSpeedModifier = 0.0f;
        _resourceToUse = Resource.Ultimate;
        _weaponTypeToUse = WeaponType.Bow;
        _resourceAmount = 25f;
    }

    public ArrowBarrage() : base() { SetupSkill(); }

    public ArrowBarrage(Weapon bowToUse) : base(bowToUse) { SetupSkill(); }

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
        _arrowsShot++;
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
