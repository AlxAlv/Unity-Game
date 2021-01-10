using System;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Decisions/DecisionNotice", fileName = "DecisionNotice")]
public class DecisionNotice : AIDecision
{
	public float DetectArea = 3.0f;
	public float TimeUntilNotice = 3.0f;
	public LayerMask TargetMask;

	private Collider2D _targetCollider2D;
	private AIStateController _controller;

	public override bool Decide(AIStateController controller)
	{
		_controller = controller;
		return CheckTarget(controller);
	}

	private bool CheckTarget(AIStateController controller)
	{
		// _targetCollider2D contains collisions found
		_targetCollider2D = Physics2D.OverlapCircle(controller.transform.position, DetectArea, TargetMask);

		// Were there any collisions detected?
		if (_targetCollider2D != null)
		{
			// Is there a wall between the player and me?
 			RaycastHit2D hit = Physics2D.Linecast(controller.transform.position, _targetCollider2D.transform.position, LayerMask.GetMask("LevelComponents", "Player"));

			// If there's no wall in between us...
            if (hit)
            {
				// And we see the player
				if (hit.transform.tag == "Player" && controller.IsTimePassed())
				{
					controller.Target = _targetCollider2D.transform;
					return true;
				}
				else if (controller.Timer == float.MaxValue)
				{
					controller.ResetTime(TimeUntilNotice);
				}
            }
		}
		else
		{
			controller.ResetTime(TimeUntilNotice);
		}

		return false;
	}
}
