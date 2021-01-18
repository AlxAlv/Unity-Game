using System;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class EntityFlip : EntityComponent
{
    public enum FlipMode
    {
        MovementDirection,
        WeaponDirection
    }

    [SerializeField] private FlipMode m_flipMode = FlipMode.MovementDirection;
    [SerializeField] private float m_threshold = 0.1f;

    // Rotation
    private readonly float _rotationSpeed = 0.25f;

    public bool m_FacingLeft { get; set; }

    protected override void HandleComponent()
    {
	    UpdateRotation();
        base.HandleComponent();

        if (m_flipMode == FlipMode.MovementDirection)
        {
            FlipToMovementDirection();
        }
        else if (m_flipMode == FlipMode.WeaponDirection)
        {
            FlipToWeaponDirection();
        }
    }

    private void Awake()
    {
        m_FacingLeft = true;
    }

    private void FlipToMovementDirection()
    {
        if (m_controller.CurrentMovement.normalized.magnitude > m_threshold)
        {
            if (m_controller.CurrentMovement.normalized.x > 0)
            {
                FaceDirection(-1);
            }
            else if (m_controller.CurrentMovement.normalized.x < 0)
            {
                FaceDirection(1);
            }
        }
    }

    private void FlipToWeaponDirection()
    {
        if (m_entityWeapon != null)
        {
	        WeaponAim weaponAim = GetComponentInChildren<WeaponAim>();
            float weaponAngle = weaponAim.m_currentAimAngleAbsolute;

            // TODO: ALX - Yo, fix this! The enemy sprites shouldn't need this correction!
            int normalize = 1;
            if ((m_entity.EntityType == Entity.EntityTypes.AI) || _entityTarget.CurrentTarget != null)
                normalize = -1;

            if (weaponAngle > 90 || weaponAngle < -90)
            {
                FaceDirection(1 * normalize);
            }
            else
            {
                FaceDirection(-1 * normalize);
            }
        }
    }

    public void FaceDirection(int newDirection)
    {
	    if (newDirection == 1)
        {
            m_FacingLeft = true;
        }
        else if (newDirection == -1)
        {
            m_FacingLeft = false;
        }
    }

    private void UpdateRotation()
    {
        // If you're supposed to be facing left and you haven't fully rotated
	    if (m_FacingLeft && (m_entity.CharacterSprite.transform.localScale.x != 1.0f))
	    {
		    m_entity.CharacterSprite.transform.localScale = new Vector3( Math.Min((m_entity.CharacterSprite.transform.localScale.x + _rotationSpeed), 1.0f), y: 1, z: 1);
	    }
	    // Else if you're supposed to be facing right and you haven't fully rotated
        else if (!m_FacingLeft && (m_entity.CharacterSprite.transform.localScale.x != -1.0f))
	    {
		    m_entity.CharacterSprite.transform.localScale = new Vector3( Math.Max((m_entity.CharacterSprite.transform.localScale.x - _rotationSpeed), -1.0f), y: 1, z: 1);
        }
    }
}
