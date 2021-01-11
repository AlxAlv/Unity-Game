using Assets.Scripts.Skills.Magic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : MagicSkill
{
    // Skillbar Helper Static
    public static float ResourceAmount = 3.0f;
    public static Resource ResourceType = Resource.Mana;

    public Heal(Staff staffToUse) : base(staffToUse)
    {
        _stunTime = 0.0f;
        _staffToUse = staffToUse;
        _loadingTime = 0.2f;
        _knockBackAmount = 0f;
        _spritePath = "SkillIcons/HealIcon";
        _soundPath = "Audio/SoundEffects/HealFx";
        _skillName = "Heal";

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
        _health.GainHealth(_damageAmount);
    }

    public override void SetOwner(Entity anEntity)
    {
        base.SetOwner(anEntity);
    }

    protected override void UpdateDamage()
    {
        _damageAmount = _statManager.Intelligence.TotalAmount * 1 + _staffToUse.WeaponInfo.Damage;

        base.UpdateDamage();
    }

    protected override void SkillLoaded()
    {
        base.SkillLoaded();
        Execute();
    }
}
