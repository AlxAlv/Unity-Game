using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityDash : EntityComponent
{
    [SerializeField] private float m_dashDistance = 5f;
    [SerializeField] private float m_dashDuration = 0.5f;

    private bool m_isDashing;
    private float m_dashTimer;
    private Vector2 m_dashOrigin;
    private Vector2 m_dashDestination;
    private Vector2 m_currentDashPosition;

    protected override void HandleInput()
    {
		//if (Input.GetKeyDown(KeyCode.Space))
		//{
		//	Dash();
		//}
	}

	protected override void HandleComponent()
    {
        base.HandleComponent();

        if (m_isDashing)
        {
            if (m_dashTimer < m_dashDuration)
            {
                m_currentDashPosition = Vector2.Lerp(m_dashOrigin, m_dashDestination, (m_dashTimer / m_dashDuration));
                m_controller.MovePosition(m_currentDashPosition);

                m_dashTimer += Time.deltaTime;
            }
            else
            {
                StopDashing();
            }
        }
    }

    private void Dash()
    {
        m_isDashing = true;
        m_dashTimer = 0f;
        m_dashOrigin = transform.position;

        m_dashDestination = transform.position + (Vector3) m_controller.CurrentMovement.normalized * m_dashDistance;
        m_controller.IsNormalMovement = false;
    }

    private void StopDashing()
    {
        m_isDashing = false;
        m_controller.IsNormalMovement = true;
    }
}
