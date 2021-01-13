using System;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(menuName = "AI/Actions/Shoot", fileName = "ActionShoot")]
public class ActionShoot : AIAction
{
    private Vector2 _aimDirection;
    private float _timeToAttack = 1.0f;

    public override void Act(AIStateController controller)
    {
        DetermineAim(controller);
        Combat(controller);
        UpdateAttackLine(controller);

        ThoughtPopups.Create(controller.SkillLoadingTransform.position, controller.gameObject.GetInstanceID(), ThoughtTypes.ExclamationMark);
        controller.EntityMovement.RemoveFollowTarget();
    }

    private void DetermineAim(AIStateController controller)
    {
	    _aimDirection = controller.transform.position - controller.Target.position;
    }

    private void SetWeaponAim(AIStateController controller, Vector2 aim)
    {
	    WeaponAim[] weaponAims = controller.GetWeaponAims();

	    foreach (var weaponAim in weaponAims)
		    weaponAim.SetAim(aim);
    }

    private void UpdateAttackLine(AIStateController controller)
    {
	    SkillState skillState = controller.EntityWeapon.CurrentWeapon.GetEnemySkillState();

        if (skillState == SkillState.loaded)
	    {
		    controller.AttackLineRenderer.SetPosition(0, controller.transform.position);
		    controller.AttackLineRenderer.SetPosition(1, controller.Target.transform.position);

		    controller.AttackLineRenderer.startColor = Color.Lerp(Color.red, Color.green, ((controller.AttackTimer - Time.time) / _timeToAttack));
            controller.AttackLineRenderer.endColor = Color.Lerp(Color.red, Color.green, ((controller.AttackTimer - Time.time) / _timeToAttack));
        }
	    else
	    {
		    controller.AttackLineRenderer.SetPosition(0, new Vector3(0, 0, 0));
		    controller.AttackLineRenderer.SetPosition(1, new Vector3(0, 0, 0));
	    }
    }

    private void Combat(AIStateController controller)
    {
        SoundManager.Instance.UpdateCombatTimer();
        controller.EntityMovement.StopAIMoving();

        if (controller.EntityWeapon.CurrentWeapon != null)
        {
            SetWeaponAim(controller, _aimDirection);

            //Determine CombatState
            switch (controller.CurrentCombatState)
            {
                case AIStateController.CombatState.ImmediateAttack:
	                Attack(controller, false);
	                break;

                case AIStateController.CombatState.Standby:
					Standby(controller);
	                break;

                case AIStateController.CombatState.Berserker:
	                Attack(controller, true);
	                break;
            }
        }
    }

    // Try To Attack Immediately
    private void Attack(AIStateController controller, bool isBerserker)
    {
	    SkillState skillState = controller.EntityWeapon.CurrentWeapon.GetEnemySkillState();
	    bool isStunned = controller.EntityWeapon.StunGuage.Stunned;
	    bool isKnockedBack = controller.EntityWeapon.StunGuage.KnockedBack;

	    if (skillState != SkillState.loaded)
	    {
		    controller.EntityWeapon.CurrentWeapon.TriggerEnemySkill();
		    controller.ResetAttackTime(_timeToAttack);
        }
	    else if (!isStunned && !isKnockedBack && controller.IsAttackTimePassed())
	    {
		    controller.EntityWeapon.CurrentWeapon.TriggerEnemySkill();

		    if (!isBerserker)
			    ChangeToStandbyState(controller);
		    else if ((++controller.NumberOfAttacksCompleted) == 3)
			    ChangeToStandbyState(controller);
        }
    }

    private void Standby(AIStateController controller)
    {
	    if (controller.IsTimePassed())
		    ChangeToImmediateAttackState(controller);

	    if (controller.SavedHealth != controller.Health.m_currentHealth)
		    ChangeToBerserkerState(controller);
    }

    private void ChangeToStandbyState(AIStateController controller)
    {
	    CreateThought(controller, ThoughtTypes.StandbyMark);

        controller.CurrentCombatState = AIStateController.CombatState.Standby;
        controller.SavedHealth = controller.Health.m_currentHealth;

        controller.ResetTime(Random.Range(5.0f, 8.0f));
    }

    private void ChangeToImmediateAttackState(AIStateController controller)
    {
	    controller.CurrentCombatState = AIStateController.CombatState.ImmediateAttack;
    }

    private void ChangeToBerserkerState(AIStateController controller)
    {
	    CreateThought(controller, ThoughtTypes.BerserkerMark);

        controller.NumberOfAttacksCompleted = 0;
	    controller.CurrentCombatState = AIStateController.CombatState.Berserker;
    }

    private void CreateThought(AIStateController controller, ThoughtTypes type)
    {
	    ThoughtPopups.RemoveInstanceFromList(controller.gameObject.GetInstanceID());
	    ThoughtPopups.Create(controller.SkillLoadingTransform.position, controller.gameObject.GetInstanceID(), type);
    }
}
