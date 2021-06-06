using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;


public class EntityShield : EntityComponent
{
	[SerializeField] private float _maxDodgeDistance = 5.0f;
	[SerializeField] private float _dodgeDuration = (0.10f);
	[SerializeField] private float _staminaAmount = 3.0f;
	[SerializeField] private GameObject _shieldImGameObject;
	[SerializeField] private LineRenderer _radiusLineRenderer;
	[SerializeField] private LineRenderer _lineRenderer;

	private bool _isShielding = false;
	private bool _dodging = false;
	private bool _resumeWalking = false;

	private float _dodgeTimer = 0.0f;

	private Vector3 _originalDestination;
	private Vector2 _dodgeOrigin;
	private Vector2 _dodgeDestination;
	private Vector2 _currentDodgePosition;

	const float DEGREES_IN_CIRCLE = 360;
	float _rotationLeft = DEGREES_IN_CIRCLE;
	float _rotationSpeed = 10;

	public bool IsShielding => _isShielding;
	public bool IsRolling => _dodging;

	protected override void HandleInput()
	{
		if (Input.GetKeyDown(KeyCode.LeftShift) && !_dodging && !_isShielding && !_entityStunGuage.Stunned && !_entityStunGuage.KnockedBack)
		{
			StartShielding();
		}
		else if (Input.GetKeyUp(KeyCode.LeftShift) && _isShielding)
		{
			_isShielding = false;
			_health.ShieldModifier = 1.0f;

			if (!CameraFilter.Instance.IsTransitioning && _stamina.UseStamina(_staminaAmount))
				Dodge();
		}
	}

	protected override void HandleComponent()
	{
		base.HandleComponent();

		_lineRenderer.gameObject.SetActive(_isShielding);
		_radiusLineRenderer.gameObject.SetActive(_isShielding);
		_shieldImGameObject.SetActive(_isShielding);

		if (_isShielding)
		{
			CalcualteAngle();
		}

		if (_dodging)
		{
			float rotation = _rotationSpeed * Time.deltaTime;
			if (_rotationLeft > rotation)
			{
				_rotationLeft -= rotation;
			}
			else
			{
				rotation = _rotationLeft;
				_rotationLeft = 0;
			}

			transform.Rotate(0, 0, rotation);

			if (_dodgeTimer < _dodgeDuration)
			{
				_currentDodgePosition = Vector2.Lerp(_dodgeOrigin, _dodgeDestination, (_dodgeTimer / _dodgeDuration));
				m_controller.MovePosition(_currentDodgePosition);

				_dodgeTimer += Time.deltaTime;
			}
			else
			{
				StopDodging();
			}

			// Check if we are colliding with a wall
			if (m_movement.WallCollisionDetected)
			{
				StopDodging();
			}
		}
	}

	private void Dodge()
	{
		_rotationSpeed = DEGREES_IN_CIRCLE / _dodgeDuration;
		_rotationLeft = DEGREES_IN_CIRCLE;

		_dodging = true;
		_dodgeTimer = 0f;
		_dodgeOrigin = transform.position;

		_dodgeDestination = CalcualteAngle();
		m_controller.IsNormalMovement = false;
		SetDodgeInvincibility();
	}

	private void StartShielding()
	{
		_resumeWalking = !(m_movement.IsAtDestination());
		_originalDestination = m_movement.Destination;

		m_movement.RemoveDestination();
		m_entityWeapon.CancelAllSkills();

		_isShielding = true;

		_health.ShieldModifier = 0.5f;
	}

	public void StopDodging()
	{
		_dodging = false;
		m_controller.IsNormalMovement = true;

		if (_resumeWalking)
			m_movement.SetAIDestination(_originalDestination);
		else
			m_movement.RemoveDestination();

		RemoveDodgeInvincibility();

		// Reset Rotation
		transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
	}

	Vector3 CalcualteAngle()
	{
		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousePos.z = transform.position.z;

		float radius = _maxDodgeDistance;
		Vector3 centerPosition = transform.localPosition;
		float distance = Vector3.Distance(mousePos, centerPosition);

		if (distance > radius)
		{
			Vector3 fromOriginToObject = mousePos - centerPosition;
			fromOriginToObject *= radius / distance;
			mousePos = centerPosition + fromOriginToObject;
		}

		// Radius you can dodge in
		DrawPolygon(32, _maxDodgeDistance, centerPosition, 0.05f, 0.05f, Color.white, _radiusLineRenderer);
		
		// Dodge location
		DrawPolygon(32, 0.5f, mousePos, 0.05f, 0.05f, Color.white, _lineRenderer);

		return mousePos;
	}

	private void SetDodgeInvincibility()
	{
		_health.DodgeDamageModifier = 0.0f;
	}

	private void RemoveDodgeInvincibility()
	{
		_health.DodgeDamageModifier = 1.0f;
	}

	public void DrawPolygon(int vertexNumber, float radius, Vector3 centerPos, float startWidth, float endWidth, Color color, LineRenderer renderer)
	{
		renderer.startWidth = startWidth;
		renderer.endWidth = endWidth;
		renderer.startColor = color;
		renderer.endColor = color;
		renderer.loop = true;
		float angle = 2 * Mathf.PI / vertexNumber;
		renderer.positionCount = vertexNumber;

		for (int i = 0; i < vertexNumber; i++)
		{
			Matrix4x4 rotationMatrix = new Matrix4x4(new Vector4(Mathf.Cos(angle * i), Mathf.Sin(angle * i), 0, 0),
				new Vector4(-1 * Mathf.Sin(angle * i), Mathf.Cos(angle * i), 0, 0),
				new Vector4(0, 0, 1, 0),
				new Vector4(0, 0, 0, 1));
			Vector3 initialRelativePosition = new Vector3(0, radius, 0);
			renderer.SetPosition(i, centerPos + rotationMatrix.MultiplyPoint(initialRelativePosition));
		}
	}
}
