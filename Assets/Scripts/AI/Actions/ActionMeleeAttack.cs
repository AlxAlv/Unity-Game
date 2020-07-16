using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/MeleeAttack", fileName = "ActionMeleeAttack")]
public class ActionMeleeAttack : AIAction
{
    public override void Act(AIStateController controller)
    {
        Attack(controller);
    }

    private void Attack(AIStateController controller)
    {
        // Stop
        controller.EntityMovement.StopAIMoving();

        // Attack
        if ((!controller.EntityWeapon.IsAnySkillLoaded() || !controller.EntityWeapon.StunGuage.Stunned))
            controller.EntityWeapon.CurrentWeapon.TriggerEnemySkill();
    }
}
