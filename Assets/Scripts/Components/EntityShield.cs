using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;


public class EntityShield : EntityComponent
{
	[SerializeField] private float _maxDodgeDistance = 3.0f;
	[SerializeField] private float _dodgeDuration = (0.10f);
	[SerializeField] private float _staminaAmount = 3.0f;
	[SerializeField] private GameObject _shieldImGameObject;
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

			if (_stamina.UseStamina(_staminaAmount))
				Dodge();
		}
	}

	protected override void HandleComponent()
	{
		base.HandleComponent();

		_lineRenderer.gameObject.SetActive(_isShielding);
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

	private void StopDodging()
	{
		_dodging = false;
		m_controller.IsNormalMovement = true;

		if (_resumeWalking)
			m_movement.SetAIDestination(_originalDestination);
		else
			m_movement.RemoveDestination();

		RemoveDodgeInvincibility();

		// Reset Rotation
		transform.Rotate(0, 0, 0);
	}

	Vector3 CalcualteAngle()
	{
		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector3 targetPos = new Vector3(0.0f, 0.0f, 0.0f);
		targetPos.z = transform.position.z;

		// The Mouse Is To The Right
		if ((transform.position.y - mousePos.y) < 0)
			targetPos.y = (Mathf.Min((transform.position.y + _maxDodgeDistance), mousePos.y));
		//Else, The Mouse Is To The Left
		else
			targetPos.y = (Mathf.Max((transform.position.y - _maxDodgeDistance), mousePos.y));

		// The Mouse Is Above
		if ((transform.position.x - mousePos.x) < 0)
			targetPos.x = (Mathf.Min((transform.position.x + _maxDodgeDistance), mousePos.x));
		//Else, The Mouse Is Below
		else
			targetPos.x = (Mathf.Max((transform.position.x - _maxDodgeDistance), mousePos.x));

		DrawPolygon(32, 0.5f, targetPos, 0.05f, 0.05f, Color.white);

		return targetPos;
	}

	private void SetDodgeInvincibility()
	{
		_health.DodgeDamageModifier = 0.0f;
	}

	private void RemoveDodgeInvincibility()
	{
		_health.DodgeDamageModifier = 1.0f;
	}

	public void DrawPolygon(int vertexNumber, float radius, Vector3 centerPos, float startWidth, float endWidth, Color color)
	{
		_lineRenderer.startWidth = startWidth;
		_lineRenderer.endWidth = endWidth;
		_lineRenderer.startColor = color;
		_lineRenderer.endColor = color;
		_lineRenderer.loop = true;
		float angle = 2 * Mathf.PI / vertexNumber;
		_lineRenderer.positionCount = vertexNumber;

		for (int i = 0; i < vertexNumber; i++)
		{
			Matrix4x4 rotationMatrix = new Matrix4x4(new Vector4(Mathf.Cos(angle * i), Mathf.Sin(angle * i), 0, 0),
				new Vector4(-1 * Mathf.Sin(angle * i), Mathf.Cos(angle * i), 0, 0),
				new Vector4(0, 0, 1, 0),
				new Vector4(0, 0, 0, 1));
			Vector3 initialRelativePosition = new Vector3(0, radius, 0);
			_lineRenderer.SetPosition(i, centerPos + rotationMatrix.MultiplyPoint(initialRelativePosition));
		}
	}
}
