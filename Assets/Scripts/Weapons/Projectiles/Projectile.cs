using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;

public class Projectile : MonoBehaviour
{
	[SerializeField] private float m_speed = 35f;
	[SerializeField] private float m_acceleration = 0f;

    public Vector2 Direction { get; set; }
    public bool FacingLeft { get; set; }
    public float Speed { get; set; }
    public int DamageAmount { get; set; }
    public int CriticalChance { get; set; }
    public string SkillName { get; set; }
    public float StunTime { get; set; }
    public float KnockBackAmount { get; set; }
    public GameObject Owner { get; set; }

    private Rigidbody2D m_rigidBody2D;
    private SpriteRenderer m_spriteRenderer;
    private Collider2D _collider;
    private Vector2 m_movement;

    private bool _canMove = true;

    private void Awake()
    {
	    Speed = m_speed;
	    FacingLeft = true;

	    Direction = Vector2.left;
	    m_rigidBody2D = GetComponent<Rigidbody2D>();
	    m_spriteRenderer = GetComponent<SpriteRenderer>();
	    _collider = GetComponent<Collider2D>();
        DamageAmount = 1;
        CriticalChance = 0;
        SkillName = "BaseSkill";
        StunTime = .3f;
    }

    private void FixedUpdate()
    {
	    if (_canMove)
	    {
		    MoveProjectile();
	    }
    }

    public void MoveProjectile()
    {
	    m_movement = Direction * Speed * Time.fixedDeltaTime;
        m_rigidBody2D.MovePosition(m_rigidBody2D.position + m_movement);

        Speed += m_acceleration * Time.deltaTime;
    }

    public void FlipProjectile()
    {
	    if (m_spriteRenderer != null)
	    {
		    //m_spriteRenderer.flipX = !m_spriteRenderer.flipX;
	    }
    }

    public void SetDirection(Vector2 newDirection, Quaternion rotation, bool isFacingLeft = true)
    {
	    Direction = newDirection;

	    if (isFacingLeft != FacingLeft)
	    {
		    FlipProjectile();
	    }

	    transform.rotation = rotation;
    }

    public void ResetProjectile()
    {
	    m_spriteRenderer.flipX = false;
    }

    public void DisableProjectile()
    {
	    _canMove = false;
	    m_spriteRenderer.enabled = false;
	    _collider.enabled = false;
    }

    public void EnableProjectile()
    {
	    _canMove = true;
	    m_spriteRenderer.enabled = true;
	    _collider.enabled = true;
    }
}
