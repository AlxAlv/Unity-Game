using Assets.Scripts.Skills.Melee;
using UnityEngine;

public class MeleeAttack : MeleeSkill
{
    // Skillbar Helper Static
    public static float ResourceAmount = 0.5f;
    public static Resource ResourceType = Resource.Stamina;

    private const int NUM_OF_SWING = 3;
    private int _currentNumOfSwings = 0;

    private float _timer = 0.0f;
    private float _timerDuration = .15f;

    public MeleeAttack(Sword swordToUse) : base(swordToUse)
    {
	    _skillName = "MeleeAttack";
        _distanceToAttack = 2.0f;
        _stunTime = 2.0f;
        _knockBackAmount = 40f;
        _loadingTime = 0.75f;
        _loadingMovementSpeedModifier = 1.0f;
        _loadedMovementSpeedModifier = 1.0f;
        _spritePath = "SkillIcons/MeleeAttackIcon";
        _meleeFxPath = "Audio/SoundEffects/SwordSwingFx";
        _skillName = "MeleeAttack";

        _resourceAmount = ResourceAmount;
        _resourceToUse = ResourceType;
    }

    public override void Update()
    {
	    if (IsLoaded())
		    _entitySkill.SkillUses.text = _currentNumOfSwings.ToString();

        base.Update();
    }

    protected override void SkillLoaded()
    {
	    base.SkillLoaded();
	    _currentNumOfSwings = NUM_OF_SWING;
    }

    protected override void Execute()
    {
        if (IsInReach() && IsLoaded() && _pendingAttack)
        {
	        if ((_timer == 0.0f) || (Time.time > _timer))
	        {
		        _timer = Time.time + _timerDuration;
		        _swordToUse.ClearLastHitEnemies();
		        _swordToUse.UseWeapon();

		        if (_meleeFxPath.Length > 0)
			        SoundManager.Instance.Playsound(_meleeFxPath);

		        _weaponToUse.PlayAnimation();
		        SoundManager.Instance.Playsound(_soundPath);

		        _currentNumOfSwings--;
		        _pendingAttack = false;

		        LevelComponent levelComponent = _entityTarget.CurrentTarget.GetComponent<LevelComponent>();
                Health targetHealth = _entityTarget.CurrentTarget.GetComponent<Health>();

                if (levelComponent)
                {
	                levelComponent.TakeDamage(_damageAmount);
                }
                else if (targetHealth)
		        {
			        targetHealth.TakeDamage(_damageAmount, "MeleeAttack");
					targetHealth.HitStun(_stunTime, _knockBackAmount, _entity.transform);
			        targetHealth.Attacker = _entity.gameObject;
                }

		        if (_currentNumOfSwings == 0)
			        CancelSkill();
	        }
        }
    }

    protected override void UpdateDamage()
    {
        _damageAmount = (_statManager.Strength.TotalAmount * 2) + (_swordToUse.WeaponInfo.Damage * 2);

        base.UpdateDamage();
    }
}
