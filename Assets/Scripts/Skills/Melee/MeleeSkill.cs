using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Skills.Melee
{
	public class MeleeSkill : BaseSkill
	{
		// Resource Variables
		protected Sword _swordToUse;
		protected string _meleeFxPath;

		// Distance Variables
		protected float _distanceToAttack = 5.0f;

		// Attack Variables
		protected bool _pendingAttack = false;
		protected string _skillName = "MeleeSkill";

		protected MeleeSkill(Sword swordToUse) : base(swordToUse)
		{
			_swordToUse = swordToUse;
		}

		public override void Update()
		{
			if (_entityTarget.CurrentTarget != null)
			{
				if (!IsInReach() && IsLoadingOrLoaded() && _pendingAttack)
				{
					_entityMovement.SetIsWalking(true, _entityTarget.CurrentTarget);
				}

				if (IsInReach() && CurrentState != SkillState.available && _pendingAttack)
				{
					_entityMovement.RemoveDestination();

					Execute();
				}
			}

			base.Update();
		}

		protected bool IsInReach()
		{
			if (_entityTarget.CurrentTarget != null)
			{
				float distance = Vector3.Distance(_entityTarget.CurrentTarget.transform.position, _entity.transform.position);

				return (distance < _distanceToAttack);
			}

			return false;
		}

		public override void Trigger()
		{
			base.Trigger();

			_pendingAttack = true;

			if (!IsInReach() && _entityTarget.CurrentTarget != null)
				_entityMovement.SetIsWalking(true, _entityTarget.CurrentTarget);
		}

		protected override void Execute()
		{
			if (IsInReach() && IsLoaded() && _pendingAttack)
			{
				_swordToUse.ClearLastHitEnemies();
				_swordToUse.UseWeapon();

				if (!String.IsNullOrEmpty(_meleeFxPath) && (_meleeFxPath.Length > 0))
					SoundManager.Instance.Playsound(_meleeFxPath);

				SoundManager.Instance.Playsound(_soundPath);

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
					targetHealth.TakeDamage((isCriticalHit ? (_damageAmount * 2) : _damageAmount), _skillName, isCriticalHit);
					targetHealth.HitStun(_stunTime, _knockBackAmount, _entity.transform);
					targetHealth.Attacker = _entity.gameObject;
				}

				CancelSkill();
			}
		}

		protected override void UpdateDamage()
		{
			_swordToUse.SkillDamage = _damageAmount;
			_swordToUse.StunTime = _stunTime;
			_swordToUse.KnockbackAmount = _knockBackAmount;
			base.UpdateDamage();
		}

		protected void TriggerGameJuice()
		{
			Camera2DShake.Instance.Shake();
			ScreenPause.Instance.Freeze();
		}

		public override bool IsBase()
		{
			return false;
		}
	}
}
