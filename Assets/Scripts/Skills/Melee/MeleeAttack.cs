using System;
using System.Collections.Generic;
using Assets.Scripts.Skills.Melee;
using UnityEngine;
using Random = UnityEngine.Random;

public class MeleeAttack : MeleeSkill
{
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

    public MeleeAttack() : base() { SetupBaseSkill(this.GetType().Name); }
    
    public MeleeAttack(Weapon swordToUse) : base(swordToUse) { SetupBaseSkill(this.GetType().Name); }

    public override void Update()
    {
        if (IsLoaded())
        {
            _entitySkill.SkillUses.text = _currentNumOfSwings.ToString();
            Execute();
        }

        base.Update();
    }

    protected override void SkillLoaded()
    {
	    base.SkillLoaded();
	    _currentNumOfSwings = NUM_OF_SWING;
    }

    protected override void Execute()
    {
        if (IsInReach() && IsLoaded() /*&& _pendingAttack*/)
        {
	        if ((_timer == 0.0f) || (Time.time > _timer))
	        {
                _ultimatePoints.GainUltimatePoints(5);
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
                    levelComponent.TakeDamage((isCriticalHit ? (_damageAmount * 2) : _damageAmount), isCriticalHit, _weaponToUse.m_weaponOwner.GetComponent<Inventory>());
                }
                else if (targetHealth)
		        {
			        TriggerGameJuice();
			        targetHealth.Attacker = _weaponToUse.m_weaponOwner.gameObject;
                    targetHealth.TakeDamage((isCriticalHit ? (_damageAmount * 2) : _damageAmount), "MeleeAttack", isCriticalHit);
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
