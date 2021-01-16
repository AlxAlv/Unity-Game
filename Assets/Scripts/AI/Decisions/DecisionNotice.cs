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

	private float _lineWidth = 0.05f;
	private int _numVerticies = 64;

	public override bool Decide(AIStateController controller)
	{
		_controller = controller;
		return CheckTarget(controller);
	}

	private bool CheckTarget(AIStateController controller)
	{
		// _targetCollider2D contains collisions found
		_targetCollider2D = Physics2D.OverlapCircle(controller.transform.position, DetectArea, TargetMask);
		Color color = Color.green;

		// Were there any collisions detected?
		if (_targetCollider2D != null)
		{
			// Is there a wall between the player and me?
 			RaycastHit2D hit = Physics2D.Linecast(controller.transform.position, _targetCollider2D.transform.position, LayerMask.GetMask("LevelComponents", "Player"));

			// If there's no wall in between us...
            if (hit && hit.transform.tag == "Player")
            {
				// If this is the first loop where the player is noticed, create a question mark
				if (controller.LineRenderer.startColor == Color.green)
					ThoughtPopups.Create(controller.SkillLoadingTransform.position, controller.gameObject.GetInstanceID(), ThoughtTypes.QuestionMark);

				// And we see the player
				if (controller.IsTimePassed())
				{
					controller.Target = _targetCollider2D.transform;
					return true;
				}
				else if (controller.Timer == float.MaxValue)
				{
					controller.ResetTime(TimeUntilNotice);
				}
            }
            else
            {
				ThoughtPopups.RemoveInstanceFromList(controller.gameObject.GetInstanceID());
				controller.ResetTime(TimeUntilNotice);
			}
		}
		else if (controller.NoticedAttacker != null)
		{
			ThoughtPopups.Create(controller.SkillLoadingTransform.position, controller.gameObject.GetInstanceID(), ThoughtTypes.ShockedMark);

			if (controller.IsTimePassed())
			{
				controller.Target = controller.NoticedAttacker.transform;
				controller.NoticedAttacker = null;

				return true;
			}
		}
		else
		{
			controller.ResetTime(TimeUntilNotice);
			ThoughtPopups.RemoveInstanceFromList(controller.gameObject.GetInstanceID());
		}

		color = Color.Lerp(Color.red, Color.green, ((controller.Timer - Time.time) / TimeUntilNotice));
		controller.DrawPolygon(_numVerticies, DetectArea, controller.transform.position, _lineWidth, _lineWidth, color);
		return false;
	}
}
