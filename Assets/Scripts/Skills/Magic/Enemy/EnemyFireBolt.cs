﻿using Assets.Scripts.Skills.Magic;
using UnityEngine;

public class EnemyFireBolt : MagicSkill
{
    // Skillbar Helper Static
    public static float ResourceAmount = 4.0f;
    public static Resource ResourceType = Resource.Mana;

    public EnemyFireBolt(Staff staffToUse) : base(staffToUse)
    {
        _stunTime = .8f;
        _knockBackAmount = 60f;
        _staffToUse = staffToUse;
        _loadingTime = 1.2f;
        _spritePath = "SkillIcons/FireboltIcon";
        _projectilePrefabPath = "Prefabs/Projectiles/Firebolt";
        _soundPath = "Audio/SoundEffects/FireBoltFx";
        _projectileCollisionsoundPath = "Audio/SoundEffects/BoltHitFx";
        _skillName = "Firebolt";

        _resourceAmount = ResourceAmount;
        _resourceToUse = ResourceType;

        /* This Is A Status Projectile Being Shot */
        _isStatusProjectile = true;
        _numberOfTicks = 5;
        _timePerTick = 0.5f;
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

        Vector3 firstBoltSpawn = _staffToUse.ProjectileSpawnPosition;
        Vector3 secondBoltSpawn = _staffToUse.ProjectileSpawnPosition;

        firstBoltSpawn.y += 0.3f;
        secondBoltSpawn.y -= 0.3f;

        ShootProjectile(firstBoltSpawn);
        ShootProjectile(_staffToUse.ProjectileSpawnPosition);
        ShootProjectile(secondBoltSpawn);
    }

    public override void SetOwner(Entity anEntity)
    {
        base.SetOwner(anEntity);
    }

    protected override void UpdateDamage()
    {
        _damageAmount = _statManager.Intelligence.TotalAmount * 3;

        base.UpdateDamage();
    }
}
