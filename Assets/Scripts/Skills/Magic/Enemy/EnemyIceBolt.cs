﻿using Assets.Scripts.Skills.Magic;
using UnityEngine;

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
    }

    protected override void Execute()
    {
        base.Execute();

        _staffToUse.EvaluateProjectileSpawnPosition();
        ShootProjectile(_staffToUse.ProjectileSpawnPosition);
    }

    protected override void UpdateDamage()
    {
        _damageAmount = _statManager.Intelligence.TotalAmount * 1 + Random.Range(_staffToUse.WeaponInfo.MinDamage, _staffToUse.WeaponInfo.MaxDamage + 1);

        base.UpdateDamage();
    }
}
