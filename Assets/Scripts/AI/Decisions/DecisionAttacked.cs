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
		    // If we're the ones who were attacked, notify surrounding forces!
		    if (controller.GetComponent<Health>().Attacker)
		    {
			    Collider2D[] _targetCollider2D =
				    Physics2D.OverlapCircleAll(controller.transform.position, 8.0f, LayerMask.GetMask("Enemies"));

			    foreach (Collider2D collider in _targetCollider2D)
			    {
				    RaycastHit2D hit = Physics2D.Linecast(controller.transform.position, collider.transform.position, LayerMask.GetMask("Enemies"));

				    if (hit)
					    collider.GetComponent<AIStateController>().NoticedAttacker = controller.GetComponent<Health>().Attacker;
			    }
				
			    controller.Target = controller.GetComponent<Health>().Attacker.transform;
		    }

		    controller.GetComponent<Health>().Attacker = null;

			return true;
		}

		return false;
	}
}
