using UnityEngine;

[CreateAssetMenu(menuName ="AI/Actions/Patrol", fileName ="ActionPatrol")]
public class ActionPatrol : AIAction
{
    private Vector2 _newDirection;

    public override void Act(AIStateController controller)
    {
        Patrol(controller);
    }

    private void Patrol (AIStateController controller)
    {
	    if (!(controller.LineRenderer.startColor == Color.green))
	        controller.EntityMovement.StopAIMoving();
        else
			controller.EntityMovement.SetAIDestination(controller.Path.CurrentPoint);
    }
}
