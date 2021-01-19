using Assets.Scripts.Skills.Magic;
using UnityEngine;

public class BoulderToss : MagicSkill
{
    // Skillbar Helper Static
    public static float ResourceAmount = 10.0f;
    public static Resource ResourceType = Resource.Mana;

    public BoulderToss(Staff staffToUse) : base(staffToUse)
    {
        _loadingMovementSpeedModifier = 0.0f;
        _stunTime = 2.0f;
        _staffToUse = staffToUse;
        _loadingTime = (1.0f);
        _knockBackAmount = 100f;
        _spritePath = "SkillIcons/BoulderTossIcon";
        _projectilePrefabPath = "Prefabs/Projectiles/BoulderTossPrefab";
        _soundPath = "Audio/SoundEffects/BoulderTossFx";
        _projectileCollisionsoundPath = "Audio/SoundEffects/SkyFallFx";
        _skillName = "BoulderToss";

        /* AOE Outliner */
        _isAOEProjectile = true;
        _outlineRadius = 6.5f;

        _resourceAmount = ResourceAmount;
        _resourceToUse = ResourceType;
    }

    public override void Update()
    {
	    UpdateOutlineRenderer();

        base.Update();
    }

    protected override void Execute()
    {
        base.Execute();

        _staffToUse.EvaluateProjectileSpawnPosition();
        ShootProjectile(_staffToUse.ProjectileSpawnPosition);
    }

    public override void SetOwner(Entity anEntity)
    {
        base.SetOwner(anEntity);
    }

    protected override void UpdateDamage()
    {
        _damageAmount = _statManager.Intelligence.TotalAmount * 10 + Random.Range(_staffToUse.WeaponInfo.MinDamage, _staffToUse.WeaponInfo.MaxDamage + 1) * 2;

        base.UpdateDamage();
    }
}
