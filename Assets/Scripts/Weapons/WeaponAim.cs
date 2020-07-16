using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAim : MonoBehaviour
{
    [SerializeField] private GameObject m_reticlePrefab;

    public float m_currentAimAngleAbsolute { get; set; }
    public float m_currentAimAngle { get; set; }

    private Camera m_mainCamera;
    private GameObject m_reticle;
    private Weapon m_weapon;
    private Arm m_arm;
    private OffArm m_offArm;
    private Entity _entity;
    private EntityTarget _entityTarget = null;

    private Vector3 m_direction;
    private Vector3 m_mousePosition;
    private Vector3 m_reticlePosition;
    private Vector3 m_currentAim = Vector3.zero;
    private Vector3 m_currentAimAbsolute = Vector3.zero;
    private Quaternion m_initialRotation;
    private Quaternion m_lookRotation;

    private void Start()
    {
        m_mainCamera = Camera.main;
        m_weapon = GetComponent<Weapon>();

        // Check Weapon
        if (m_weapon == null)
	        m_arm = GetComponent<Arm>();
        else
            _entity = m_weapon.m_weaponOwner;

        // Check Arm
        if (m_arm == null)
	        m_offArm = GetComponent<OffArm>();
        else
            _entity = m_arm.m_armOwner;

        // Check Off Arm
        if (m_offArm != null)
            _entity = m_offArm.m_armOwner;

        if (IsPlayer())
        {
            InstantiateReticle();
        }

        m_initialRotation = transform.rotation;
        _entityTarget = _entity.GetComponent<EntityTarget>();
    }

    private void Update()
    {
        if (IsPlayer())
        {
            GetMousePosition();
            MoveReticle();
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

        m_reticlePosition = m_direction;
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

            // Clamping our angle
            if (m_arm != null)
            {
	            if (m_arm.m_armOwner.GetComponent<EntityFlip>().m_FacingLeft)
	            {
		            m_currentAimAngle = Mathf.Clamp(m_currentAimAngle, -180, 180);
	            }
	            else
	            {
		            m_currentAimAngle = Mathf.Clamp(m_currentAimAngle, -180, 180);
	            }
            }
            else if (m_weapon != null)
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
            else if (m_offArm != null)
            {
	            if (m_offArm.m_armOwner.GetComponent<EntityFlip>().m_FacingLeft)
	            {
		            m_currentAimAngle = Mathf.Clamp(m_currentAimAngle, -180, 180);
	            }
	            else
	            {
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

    private void MoveReticle()
    {
        m_reticle.transform.rotation = Quaternion.identity;
        m_reticle.transform.position = m_reticlePosition;
    }

    public void DestroyReticle()
    {
        Destroy(m_reticle);
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

    private void InstantiateReticle()
    {
        if (m_reticlePrefab != null)
            m_reticle = Instantiate(m_reticlePrefab);
    }

    private bool IsPlayer()
    {
        return (_entity.EntityType == Entity.EntityTypes.Player);
    }
}
