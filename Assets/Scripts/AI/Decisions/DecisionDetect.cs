﻿using UnityEngine;

[CreateAssetMenu(menuName = "AI/Decisions/DetectTarget", fileName = "DecisionDetect")]
public class DecisionDetect : AIDecision
{
	public float DetectArea = 3.0f;
	public LayerMask TargetMask;

	private Collider2D _targetCollider2D;
	private AIStateController _controller;

	private float _lineWidth = 0.025f;
	private int _numVerticies = 64;

	public override bool Decide(AIStateController controller)
	{
		_controller = controller;
		return CheckTarget(controller);
	}

	private bool CheckTarget(AIStateController controller)
	{
		_targetCollider2D = Physics2D.OverlapCircle(controller.transform.position, DetectArea, TargetMask);
		Color color = Color.green;

		if (_targetCollider2D != null)
		{
			RaycastHit2D hit = Physics2D.Linecast(controller.transform.position, _targetCollider2D.transform.position, LayerMask.GetMask("LevelComponents", "Player"));

			if (hit && hit.transform.tag == "Player")
			{
				color = Color.red;
				controller.Target = _targetCollider2D.transform;

				controller.DrawPolygon(_numVerticies, DetectArea, controller.transform.position, _lineWidth, _lineWidth, color);

				return true;
			}
		}

		controller.DrawPolygon(_numVerticies, DetectArea, controller.transform.position, _lineWidth, _lineWidth, color);
		return false;
	}
}
