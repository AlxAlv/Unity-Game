using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAim : MonoBehaviour
{
	public float m_currentAimAngleAbsolute { get; set; }
    public float m_currentAimAngle { get; set; }

    private Camera m_mainCamera;
    private Weapon m_weapon;
    private Entity _entity;
    private EntityTarget _entityTarget = null;

    private Vector3 m_direction;
    private Vector3 m_mousePosition;
    private Vector3 m_currentAim = Vector3.zero;
    private Vector3 m_currentAimAbsolute = Vector3.zero;
    private Quaternion m_initialRotation;
    private Quaternion m_lookRotation;

    private void Start()
    {
        m_mainCamera = Camera.main;
        m_weapon = GetComponent<Weapon>();

        // Check Weapon
	    _entity = m_weapon.m_weaponOwner;

        m_initialRotation = transform.rotation;
        _entityTarget = _entity.GetComponent<EntityTarget>();
    }

    private void Update()
    {
        if (IsPlayer())
        {
            GetMousePosition();
        }
        else
            EnemyAim();

        TargetAim();
        RotateWeapon();

        FlipIfNeeded();
    }

    private void FlipIfNeeded()
    {
        // TODO: ALX - Yo, fix this! The enemy sprites shouldn't need this correction!
        int normalize = 1;
        if ((_entity.EntityType == Entity.EntityTypes.AI) || _entityTarget.CurrentTarget != null)
            normalize = -1;

        if (m_currentAimAngleAbsolute > 90 || m_currentAimAngleAbsolute < -90)
        {
            FaceDirection(1 * normalize);
        }
        else
        {
            FaceDirection(-1 * normalize);
        }
    }

    public void FaceDirection(int newDirection)
    {
        if (newDirection == 1)
        {
            transform.localScale = new Vector3(x: 1, y: 1, z: 1);
        }
        else if (newDirection == -1)
        {
            transform.localScale = new Vector3(x: 1, y: -1, z: 1);
        }
    }

    private void GetMousePosition()
    {
	    Transform parentTransform = transform.root;

        m_mousePosition = Input.mousePosition;
        m_mousePosition.z = 5.0f;

        m_direction = m_mainCamera.ScreenToWorldPoint(m_mousePosition);
        m_direction.z = parentTransform.position.z;

        m_currentAimAbsolute = m_direction - parentTransform.position;
        m_currentAim = parentTransform.position - m_direction;
    }

    public void RotateWeapon()
    {
        if (m_currentAim != Vector3.zero && m_direction != Vector3.zero)
        {
            // Get the angles
            m_currentAimAngle = Mathf.Atan2(m_currentAim.y, m_currentAim.x) * Mathf.Rad2Deg;
            m_currentAimAngleAbsolute = Mathf.Atan2(m_currentAimAbsolute.y, m_currentAimAbsolute.x) * Mathf.Rad2Deg;

            if (m_weapon != null)
            {
	            if (m_weapon.m_weaponOwner.GetComponent<EntityFlip>().m_FacingLeft)
	            {
		            Vector3 lTemp = transform.localScale;
		            lTemp.y = 1;
		            transform.localScale = lTemp;

                    m_currentAimAngle = Mathf.Clamp(m_currentAimAngle, -180, 180);
	            }
	            else
	            {
		            Vector3 lTemp = transform.localScale;
		            lTemp.y = -1;
		            transform.localScale = lTemp;

                    m_currentAimAngle = Mathf.Clamp(m_currentAimAngle, -180, 180);
	            }
            }

            // Apply the calculated angle
            m_lookRotation = Quaternion.Euler(m_currentAimAngle * Vector3.forward);
            transform.rotation = m_lookRotation;
        }
        else
        {
            m_currentAimAngle = 0f;
            transform.rotation = m_initialRotation;
        }
    }

    private void EnemyAim()
    {
        Transform parentTransform = transform.parent;

        m_currentAimAbsolute = m_currentAim;
        m_direction = m_currentAim - parentTransform.position;
    }

    private void TargetAim()
    {
        if (_entityTarget && (_entityTarget.CurrentTarget != null))
        {
            // SetAim equivalent here
            m_currentAim = transform.position - _entityTarget.CurrentTarget.transform.position;

            Transform parentTransform = transform.parent;

            m_currentAimAbsolute = m_currentAim;
            m_direction = m_currentAim - parentTransform.position;
        }
    }

    public void SetAim(Vector2 newAim)
    {
        m_currentAim =  newAim;
    }

    private bool IsPlayer()
    {
        return (_entity.EntityType == Entity.EntityTypes.Player);
    }
}
