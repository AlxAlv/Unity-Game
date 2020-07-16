using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Decisions/Timer", fileName = "DecisionTimer")]
public class DecisionTimer : AIDecision
{
	private float _timer = 0.0f;
	public float TimerDuration = 5.0f;

	public override bool Decide(AIStateController controller)
	{
		if (_timer == 0.0f)
			_timer = Time.time + TimerDuration;
			
		return IsTimerDone(controller);
	}

	private bool IsTimerDone(AIStateController controller)
	{
		if (Time.time > _timer)
		{
			_timer = 0.0f;
			return true;
		}

		return false;
	}
}
