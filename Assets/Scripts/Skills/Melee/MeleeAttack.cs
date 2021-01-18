using System;
using System.Collections.Generic;
using Assets.Scripts.Skills.Melee;
using UnityEngine;
using Random = UnityEngine.Random;

public class MeleeAttack : MeleeSkill
{
    // Skillbar Helper Static
    public static float ResourceAmount = 0.5f;
    public static Resource ResourceType = Resource.Stamina;

    private const int NUM_OF_SWING = 3;
    private int _currentNumOfSwings = 0;

    private float _timer = 0.0f;
    private float _timerDuration = .05f;

    private List<String> _swingEffects = new List<string>
    {
	    "Audio/SoundEffects/SwordSwing3Fx",
	    "Audio/SoundEffects/SwordSwing2Fx",
	    "Audio/SoundEffects/SwordSwing1Fx"
    };

    public MeleeAttack(Sword swordToUse) : base(swordToUse)
    {
	    _skillName = "MeleeAttack";
        _distanceToAttack = 2.0f;
        _stunTime = 2.0f;
        _knockBackAmount = 40f;
        _loadingTime = 0.4f;
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

		        SoundManager.Instance.Playsound(_swingEffects[(_currentNumOfSwings - 1)]);
		        SoundManager.Instance.Playsound(_soundPath);

		        _currentNumOfSwings--;
		        _pendingAttack = false;

		        LevelComponent levelComponent = _entityTarget.CurrentTarget.GetComponent<LevelComponent>();
                Health targetHealth = _entityTarget.CurrentTarget.GetComponent<Health>();

                // Critical Chance
                bool isCriticalHit = (Random.Range(0, 101) < _weaponToUse.CriticalChance);

                if (levelComponent)
                {
	                TriggerGameJuice();
                    levelComponent.TakeDamage((isCriticalHit ? (_damageAmount * 2) : _damageAmount));
                }
                else if (targetHealth)
		        {
			        TriggerGameJuice();
                    targetHealth.TakeDamage((isCriticalHit ? (_damageAmount * 2) : _damageAmount), "MeleeAttack");
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
        _damageAmount = (_statManager.Strength.TotalAmount * 2) + (Random.Range(_swordToUse.WeaponInfo.MinDamage, _swordToUse.WeaponInfo.MaxDamage + 1)  * 2);

        base.UpdateDamage();
    }
}
