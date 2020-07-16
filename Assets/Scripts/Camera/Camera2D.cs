using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera2D : MonoBehaviour
{
	private enum CameraMode
	{
		Update,
		FixedUpdate,
		LateUpdate
	}

	[Header("Target")]
	[SerializeField] private Transform m_targetTransform;

	[Header("Offset")] 
	[SerializeField] private Vector2 m_offset;

	[Header("Mode")] 
	[SerializeField] private CameraMode m_cameraMode = CameraMode.Update;

	private void FollowTarget()
	{
		Vector3 desiredPosition = new Vector3(m_targetTransform.position.x + m_offset.x, m_targetTransform.position.y + m_offset.y, transform.position.z);
		transform.position = desiredPosition;
	}

	private void Update()
	{
		if (m_cameraMode == CameraMode.Update)
		{
			FollowTarget();
		}

		HandleInput();
	}

	private void FixedUpdate()
	{
		if (m_cameraMode == CameraMode.FixedUpdate)
		{
			FollowTarget();
		}
	}

	private void LateUpdate()
	{
		if (m_cameraMode == CameraMode.LateUpdate)
		{
			FollowTarget();
		}
	}

	private void HandleInput()
	{
		if (Input.mouseScrollDelta.y > 0 && Camera.main.orthographicSize > 12)
			Camera.main.orthographicSize = Camera.main.orthographicSize - 1;

		else if (Input.mouseScrollDelta.y < 0 && Camera.main.orthographicSize < 20)
			Camera.main.orthographicSize = Camera.main.orthographicSize + 1;
	}
}
