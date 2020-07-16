using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charge : ShieldSkill
{
    // Skillbar Helper Static
    public static float ResourceAmount = 1.0f;
    public static Resource ResourceType = Resource.Stamina;

    public Charge(Shield shieldToUse) : base(shieldToUse)
    {
        _stunTime = 2.0f;
        _knockBackAmount = 100f;
        _loadingTime = 0.05f;
        _loadingMovementSpeedModifier = 1.0f;
        _loadedMovementSpeedModifier = 1.0f;
        _spritePath = "SkillIcons/ChargeIcon";
        _soundPath = "Audio/SoundEffects/ChargeFx";
        _skillName = "ChargeAttack";

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

        // Alx TODO: Implement Charge Logic Here
        DialogManager.Instance.InstantSystemMessage("Charge skill used but not implemented");
    }

    public override void SetOwner(Entity anEntity)
    {
        base.SetOwner(anEntity);
    }

    protected override void UpdateDamage()
    {
        _damageAmount = _statManager.Dexterity.StatAmount * 2;

        base.UpdateDamage();
    }
}
