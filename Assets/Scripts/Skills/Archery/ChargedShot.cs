using Assets.Scripts.Skills.Archery;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargedShot : ArcherySkill
{
    // Skillbar Helper Static
    public static float ResourceAmount = 10.0f;
    public static Resource ResourceType = Resource.Stamina;

    public ChargedShot(Bow bowToUse) : base(bowToUse)
    {
        _stunTime = 1.0f;
        _knockBackAmount = 100f;
        _loadingTime = (1.0f);
        _spritePath = "SkillIcons/ChargedShotIcon";
        _projectilePrefabPath = "Prefabs/Projectiles/Arrow";
        _soundPath = "Audio/SoundEffects/RangedAttackFx";
        _projectileCollisionsoundPath = "Audio/SoundEffects/ArrowHitFx";
        _skillName = "ChargedShot";

        _resourceAmount = ResourceAmount;
        _resourceToUse = ResourceType;
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
        ShootProjectile(_bowToUse.ProjectileSpawnPosition);
    }

    public override void SetOwner(Entity anEntity)
    {
        base.SetOwner(anEntity);
    }

    protected override void UpdateDamage()
    {
        _damageAmount = _statManager.Dexterity.TotalAmount * 5 + Random.Range(_bowToUse.WeaponInfo.MinDamage, _bowToUse.WeaponInfo.MaxDamage + 1) * 5;

        base.UpdateDamage();
    }
}