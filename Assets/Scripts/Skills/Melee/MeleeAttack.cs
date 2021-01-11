using Assets.Scripts.Skills.Melee;
using UnityEngine;

public class MeleeAttack : MeleeSkill
{
    // Skillbar Helper Static
    public static float ResourceAmount = 0.5f;
    public static Resource ResourceType = Resource.Stamina;

    public MeleeAttack(Sword swordToUse) : base(swordToUse)
    {
        _stunTime = 2.0f;
        _knockBackAmount = 40f;
        _loadingTime = 0.5f;
        _loadingMovementSpeedModifier = 1.0f;
        _loadedMovementSpeedModifier = 1.0f;
        _spritePath = "SkillIcons/MeleeAttackIcon";
        _meleeFxPath = "Audio/SoundEffects/SwordSwingFx";
        _skillName = "MeleeAttack";

        _resourceAmount = ResourceAmount;
        _resourceToUse = ResourceType;
    }

    protected override void Awake()
    {
        base.Awake();
    }

    public override void Update()
    {
        if (_entityTarget.CurrentTarget != null)
        {
            if (!IsInReach() && IsLoadingOrLoaded())
            {
                _entityMovement.SetIsWalking(true, _entityTarget.CurrentTarget);
            }

            if (IsInReach() && IsLoaded())
            {
                _entityMovement.RemoveDestination();
                Execute();
            }
        }

        base.Update();
    }

    private bool IsInReach()
    {
        if (_entityTarget.CurrentTarget != null)
        {
            float distance = Vector3.Distance(_entityTarget.CurrentTarget.transform.position, _entity.transform.position);

            return (distance < 1.5f);
        }

        return false;
    }

    public override void Trigger()
    {
        base.Trigger();

        if (!IsInReach() && _entityTarget.CurrentTarget != null)
            _entityMovement.SetIsWalking(true, _entityTarget.CurrentTarget);
    }

    protected override void Execute()
    {
        if (IsInReach())
        {
            _swordToUse.ClearLastHitEnemies();
            base.Execute();
        }
    }

    public override void SetOwner(Entity anEntity)
    {
        base.SetOwner(anEntity);
    }

    protected override void UpdateDamage()
    {
        _damageAmount = (_statManager.Strength.TotalAmount * 3) + (_swordToUse.WeaponInfo.Damage * 2);

        base.UpdateDamage();
    }
}
