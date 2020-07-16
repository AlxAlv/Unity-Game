using UnityEngine;

[CreateAssetMenu(menuName = "AI/Decisions/DetectAttacker", fileName = "DecisionAttacked")]
public class DecisionAttacked : AIDecision
{
	public override bool Decide(AIStateController controller)
	{
		return CheckIfAttacked(controller);
	}

	private bool CheckIfAttacked(AIStateController controller)
 	{
		if (controller.GetComponent<Health>().Attacker != null)
		{
			controller.Target = controller.GetComponent<Health>().Attacker.transform;
			controller.GetComponent<Health>().Attacker = null;
			return true;
		}

		return false;
	}
}
