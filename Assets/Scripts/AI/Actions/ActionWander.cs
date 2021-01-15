using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/Wander", fileName = "ActionWander")]
public class ActionWander : AIAction
{
    [Header("Wander Settings")]
    public float WanderArea = 10.0f;
    public float WanderTime = 2.0f;

    [Header("Obstacles Settings")]
    public Vector2 ObstacleBoxCheckSize = new Vector2(2, 2);
    public LayerMask ObstacleMask;

    private Vector2 _wanderDirection;
    private float _currentWanderTimer = 0.0f;
    private float _wanderCheckTime = 3.0f;

    private void OnEnable()
    {
        _currentWanderTimer = 0.0f;
    }

    public override void Act(AIStateController controller)
    {
        //EvaluateObstacles(controller);
        Wander(controller);
        controller.EntityMovement.RemoveFollowTarget();

        if (!(controller.LineRenderer.startColor == Color.green))
			controller.EntityMovement.StopAIMoving();
    }

    private void EvaluateObstacles(AIStateController controller)
    {
        //RaycastHit2D hit = Physics2D.BoxCast(controller.Collider2D.bounds.center, ObstacleBoxCheckSize, 0.0f, _wanderDirection, _wanderDirection.magnitude, ObstacleMask);

        //if (hit)
        //{
        //    // Pick random location
        //    _wanderDirection.x = Random.Range(-WanderArea, WanderArea);
        //    _wanderDirection.y = Random.Range(-WanderArea, WanderArea);

        //    // Update wander time
        //    _wanderCheckTime = Time.time;
        //}
    }

    private void Wander(AIStateController controller)
    {
        if (_wanderCheckTime < _currentWanderTimer && (controller.LineRenderer.startColor == Color.green))
        {
            Vector3 destination = 
            new Vector3(controller.transform.position.x + Random.Range(-WanderArea, WanderArea), 
                        controller.transform.position.y + Random.Range(-WanderArea, WanderArea),
                        0.0f);

            controller.EntityMovement.SetAIDestination(destination);

            // Update wander time
            _currentWanderTimer = 0.0f;
        }
        else
            _currentWanderTimer += Time.deltaTime;
    }
}
