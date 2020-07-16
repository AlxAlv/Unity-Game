using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Decisions/Detect Range To Attack", fileName = "DecisionRangeToAttack")]
public class DecisionRangeToAttack : AIDecision
{
    public float MinDistanceToAttack = 1.0f;

    public override bool Decide(AIStateController controller)
    {
        return PlayerInRangeToAttack(controller);
    }

    private bool PlayerInRangeToAttack(AIStateController controller)
    {
        if (controller.Target != null)
        {
            // Get Distance
            float distanceToAttack = (controller.Target.position - controller.transform.position).sqrMagnitude;

            // Returning true of false
            return (distanceToAttack < Mathf.Pow(MinDistanceToAttack, 2));
        }

        return false;
    }
}
