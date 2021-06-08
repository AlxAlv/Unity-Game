using Assets.Scripts.Skills.Magic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : MagicSkill
{
    private void SetupSkill()
    {
        _skillName = this.GetType().Name;
        _iconName = _skillName + "Icon";
        _spritePath += _iconName;

        // Things To Update
        _soundPath += "HealFx";
        _toolTipInfo = "Heal yourself!";
        _stunTime = 0.0f;
        _knockBackAmount = 0.0f;
        _loadingTime = 0.25f;
        _loadingMovementSpeedModifier = 0.5f;
        _loadedMovementSpeedModifier = 0.0f;
        _resourceAmount = 3.0f;
        _resourceToUse = Resource.Mana;
        _weaponTypeToUse = WeaponType.Magic;
    }

    public Heal() : base() { SetupSkill(); }

    public Heal(Weapon staffToUse) : base(staffToUse) { SetupSkill(); }

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
        _damageAmount = _statManager.Intelligence.TotalAmount * 5 + Random.Range(_staffToUse.WeaponInfo.MinDamage, _staffToUse.WeaponInfo.MaxDamage + 1);

        base.UpdateDamage();
    }

    protected override void SkillLoaded()
    {
        base.SkillLoaded();
        Execute();
    }
}
