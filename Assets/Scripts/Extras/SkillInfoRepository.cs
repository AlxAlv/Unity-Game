using System.Collections.Generic;
using static BaseSkill;

public struct SkillInfo
{
    public string ProjectilePrefabPath;
    public string ChainMaterialPath;
    public string SoundPath;
    public string ProjectileCollisionsoundPath;
    public string ToolTipInfo;
    public string MeleeSoundPath;
    public float StunTime;
    public float KnockBackAmount;
    public float LoadingTime;
    public float LoadingMovementSpeedModifier;
    public float LoadedMovementSpeedModifier;
    public float ResourceAmount;
    public float OutlineRadius;
    public float DistanceToAttack;
    public float TimePerTick;
    public int NumberOfTicks;
    public bool IsAOEProjectile;
    public bool IsStatusProjectile;
    public Resource TheResourceType;
    public WeaponType TheWeaponType;
}

public class SkillInfoRepository : Singleton<SkillInfoRepository>
{
    public Dictionary<string, SkillInfo> SkillInfoMap;

    // Setup Info
    void Start()
    {
        SkillInfoMap = new Dictionary<string, SkillInfo>();
        // Player Archery Skills

        // RangedAttack
        SkillInfoMap["RangedAttack"] =
        new SkillInfo() {
            ProjectilePrefabPath = "Arrow",
            SoundPath = "RangedAttackFx",
            ProjectileCollisionsoundPath = "ArrowHitFx",
            ToolTipInfo = "Shoot an arrow!",
            StunTime = 1.0f,
            KnockBackAmount = 50f,
            LoadingTime = 0.3f,
            LoadingMovementSpeedModifier = 0.5f,
            LoadedMovementSpeedModifier = 0.0f,
            ResourceAmount = 1.0f,
            TheResourceType = Resource.Stamina,
            TheWeaponType = WeaponType.Bow
        };

        // ArrowBarrage
        SkillInfoMap["ArrowBarrage"] =
        new SkillInfo()
        {
            ProjectilePrefabPath = "Arrow",
            SoundPath = "RangedAttackFx",
            ProjectileCollisionsoundPath = "ArrowHitFx",
            ToolTipInfo = "Unleash a barrage of arrows!",
            StunTime = 1.0f,
            KnockBackAmount = 50f,
            LoadingTime = 0.0f,
            LoadingMovementSpeedModifier = 1.0f,
            LoadedMovementSpeedModifier = 0.0f,
            ResourceAmount = 25f,
            TheResourceType = Resource.Ultimate,
            TheWeaponType = WeaponType.Bow
        };

		// ArrowRevolver
		SkillInfoMap["ArrowRevolver"] =
		new SkillInfo()
		{
			ProjectilePrefabPath = "Arrow",
			SoundPath = "RangedAttackFx",
			ProjectileCollisionsoundPath = "ArrowHitFx",
			ToolTipInfo = "Load up 5 arrows to shoot!",
			StunTime = 0.8f,
			KnockBackAmount = 25f,
			LoadingTime = 0.8f,
			LoadingMovementSpeedModifier = 0.8f,
			LoadedMovementSpeedModifier = 0.0f,
			ResourceAmount = 5.0f,
			TheResourceType = Resource.Stamina,
			TheWeaponType = WeaponType.Bow
		};

        // ChargedShot
        SkillInfoMap["ChargedShot"] =
        new SkillInfo()
        {
            ProjectilePrefabPath = "Arrow",
            SoundPath = "RangedAttackFx",
            ProjectileCollisionsoundPath = "ArrowHitFx",
            ToolTipInfo = "Shoot a devastating arrow!",
            StunTime = 1.0f,
            KnockBackAmount = 100f,
            LoadingTime = 1.0f,
            LoadingMovementSpeedModifier = 0.5f,
            LoadedMovementSpeedModifier = 0.0f,
            ResourceAmount = 10.0f,
            TheResourceType = Resource.Stamina,
            TheWeaponType = WeaponType.Bow
        };

        // PoisonArrow
        SkillInfoMap["PoisonArrow"] =
        new SkillInfo()
        {
            ProjectilePrefabPath = "PoisonArrow",
            SoundPath = "RangedAttackFx",
            ProjectileCollisionsoundPath = "ArrowHitFx",
            ToolTipInfo = "Shoot an arrow covered in poison!",
            StunTime = 0.35f,
            KnockBackAmount = 20f,
            LoadingTime = 0.5f,
            LoadingMovementSpeedModifier = 0.5f,
            LoadedMovementSpeedModifier = 0.0f,
            ResourceAmount = 5.0f,
            TheResourceType = Resource.Mana,
            TheWeaponType = WeaponType.Bow,
            NumberOfTicks = 5,
            TimePerTick = 1.5f
        };

        // Player Magic Skills

        // BoulderToss
        SkillInfoMap["BoulderToss"] =
        new SkillInfo()
        {
            ProjectilePrefabPath = "BoulderTossPrefab",
            SoundPath = "BoulderTossFx",
            ProjectileCollisionsoundPath = "SkyFallFx",
            ToolTipInfo = "Throw a large boulder for massive AOE damage!",
            StunTime = 2.0f,
            KnockBackAmount = 100f,
            LoadingTime = 1.0f,
            LoadingMovementSpeedModifier = 0.5f,
            LoadedMovementSpeedModifier = 0.0f,
            ResourceAmount = 10.0f,
            TheResourceType = Resource.Mana,
            TheWeaponType = WeaponType.Magic,
            IsAOEProjectile = true,
            OutlineRadius = 6.5f
        };

        // FireBolt
        SkillInfoMap["FireBolt"] =
        new SkillInfo()
        {
            ProjectilePrefabPath = "Firebolt",
            SoundPath = "FireBoltFx",
            ProjectileCollisionsoundPath = "BoltHitFx",
            ToolTipInfo = "Burn foes with a 3 bolt blast of fire!",
            StunTime = 1.2f,
            KnockBackAmount = 60f,
            LoadingTime = 0.75f,
            LoadingMovementSpeedModifier = 0.5f,
            LoadedMovementSpeedModifier = 0.0f,
            ResourceAmount = 5.0f,
            TheResourceType = Resource.Mana,
            TheWeaponType = WeaponType.Magic,
            IsStatusProjectile = true,
            NumberOfTicks = 5,
            TimePerTick = 0.5f
        };

        // FrozenDaggers
        SkillInfoMap["FrozenDaggers"] =
        new SkillInfo()
        {
            ProjectilePrefabPath = "FrozenDaggersPrefab",
            SoundPath = "IceBoltFx",
            ProjectileCollisionsoundPath = "BoltHitFx",
            ToolTipInfo = "Load up and shoot mini daggers of ice!",
            StunTime = 0.25f,
            KnockBackAmount = 10f,
            LoadingTime = 1.0f,
            LoadingMovementSpeedModifier = 1.0f,
            LoadedMovementSpeedModifier = 1.0f,
            ResourceAmount = 0.5f,
            TheResourceType = Resource.Mana,
            TheWeaponType = WeaponType.Magic,
        };

        // Heal
        SkillInfoMap["Heal"] =
        new SkillInfo()
        {
            SoundPath = "HealFx",
            ProjectileCollisionsoundPath = "BoltHitFx",
            ToolTipInfo = "Heal yourself!",
            StunTime = 0.0f,
            KnockBackAmount = 0.0f,
            LoadingTime = 0.25f,
            LoadingMovementSpeedModifier = 0.5f,
            LoadedMovementSpeedModifier = 0.0f,
            ResourceAmount = 3.0f,
            TheResourceType = Resource.Mana,
            TheWeaponType = WeaponType.Magic,
        };

        // IceBolt
        SkillInfoMap["IceBolt"] =
        new SkillInfo()
        {
            ProjectilePrefabPath = "Icebolt",
            SoundPath = "IceBoltFx",
            ProjectileCollisionsoundPath = "BoltHitFx",
            ToolTipInfo = "Shoot a ball made of ice!",
            StunTime = 0.8f,
            KnockBackAmount = 35f,
            LoadingTime = 0.25f,
            LoadingMovementSpeedModifier = 0.5f,
            LoadedMovementSpeedModifier = 0.0f,
            ResourceAmount = 1.0f,
            TheResourceType = Resource.Mana,
            TheWeaponType = WeaponType.Magic
        };

        // LightningBolt
        SkillInfoMap["LightningBolt"] =
        new SkillInfo()
        {
            ProjectilePrefabPath = "Lightningbolt",
            ChainMaterialPath = "LightningParticle",
            SoundPath = "LightningBoltFx",
            ProjectileCollisionsoundPath = "BoltHitFx",
            ToolTipInfo = "Shoot a ball of lightning that bounces off multiple enemies!",
            StunTime = 2.0f,
            KnockBackAmount = 10f,
            LoadingTime = 0.15f,
            LoadingMovementSpeedModifier = 0.5f,
            LoadedMovementSpeedModifier = 0.0f,
            ResourceAmount = 0.5f,
            TheResourceType = Resource.Mana,
            TheWeaponType = WeaponType.Magic,
            OutlineRadius = 10.0f,
            IsStatusProjectile = true,
            NumberOfTicks = 5,
            TimePerTick = 0.10f
        };

        // Player Melee Skills

        // Backstab
        SkillInfoMap["Backstab"] =
        new SkillInfo()
        {
            SoundPath = "StabFx",
            ToolTipInfo = "Teleport behind an enemy and stab them!",
            StunTime = 1.0f,
            KnockBackAmount = 10f,
            LoadingTime = 0.05f,
            LoadingMovementSpeedModifier = 1.0f,
            LoadedMovementSpeedModifier = 1.0f,
            ResourceAmount = 10.0f,
            TheResourceType = Resource.Stamina,
            TheWeaponType = WeaponType.Melee,
            DistanceToAttack = 1.5f
        };

        // MeleeAttack
        SkillInfoMap["MeleeAttack"] =
        new SkillInfo()
        {
            SoundPath = "StabFx",
            MeleeSoundPath = "Audio/SoundEffects/SwordSwingFx",
            ToolTipInfo = "Swing your weapon at the enemies",
            StunTime = 1.0f,
            KnockBackAmount = 40f,
            LoadingTime = 0.3f,
            LoadingMovementSpeedModifier = 1.0f,
            LoadedMovementSpeedModifier = 1.0f,
            ResourceAmount = 1.0f,
            TheResourceType = Resource.Stamina,
            TheWeaponType = WeaponType.Melee,
            DistanceToAttack = 2.0f
        };

        // SkyFall
        SkillInfoMap["SkyFall"] =
        new SkillInfo()
        {
            SoundPath = "ChargeFx",
            ToolTipInfo = "Perform a massive leap causing AOE damage on impact!",
            StunTime = 2.0f,
            KnockBackAmount = 100f,
            LoadingTime = 0.5f,
            LoadingMovementSpeedModifier = 0.2f,
            LoadedMovementSpeedModifier = 0.35f,
            ResourceAmount = 8.0f,
            TheResourceType = Resource.Stamina,
            TheWeaponType = WeaponType.Melee,
            DistanceToAttack = 1.5f,
            OutlineRadius = 6.5f
        };
    }
}
