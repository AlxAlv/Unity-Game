using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityComponent : MonoBehaviour
{
	protected float m_horizontalInput;
	protected float m_veritcalInput;

	protected EntityController m_controller;
	protected EntityWeapon m_entityWeapon;
	protected EntityMovement m_movement;
	protected Animator m_animator;
	protected Stamina _stamina;
	protected Entity m_entity;
	protected EntityArm _entityArm;
	protected EntityTarget _entityTarget;
	protected EntityStunGuage _entityStunGuage;
	protected Rigidbody2D _rigidBody;
	protected EntityFlip _entityFlip;

	public EntityStunGuage StunGuage => _entityStunGuage;

	// Start is called before the first frame update
	protected virtual void Start()
	{
		m_controller = GetComponent<EntityController>();
		m_movement = GetComponent<EntityMovement>();
		m_animator = GetComponent<Animator>();
		m_entity = GetComponent<Entity>();
		m_entityWeapon = GetComponent<EntityWeapon>();
		_stamina = GetComponent<Stamina>();
		_entityArm = GetComponent<EntityArm>();
		_entityTarget = GetComponent<EntityTarget>();
		_entityStunGuage = GetComponent<EntityStunGuage>();
		_rigidBody = GetComponent<Rigidbody2D>();
		_entityFlip = GetComponent<EntityFlip>();
	}

	protected virtual void Update()
	{
		HandleComponent();
	}

	protected virtual void HandleComponent()
	{
		InternalHandleInput();

		if (m_entity.EntityType == Entity.EntityTypes.Player)
			HandleInput();
	}

	protected virtual void HandleInput()
	{
		
	}

	protected virtual void InternalHandleInput()
	{
	}
}
