using UnityEngine;

public class EntityFlip : EntityComponent
{
    public enum FlipMode
    {
        MovementDirection,
        WeaponDirection
    }

    [SerializeField] private FlipMode m_flipMode = FlipMode.MovementDirection;
    [SerializeField] private float m_threshold = 0.1f;

    public bool m_FacingLeft { get; set; }

    protected override void HandleComponent()
    {
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
            float weaponAngle = _entityArm.m_armAim.m_currentAimAngleAbsolute;

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
            m_entity.CharacterSprite.transform.localScale = new Vector3(x: 1, y: 1, z: 1);
        }
        else if (newDirection == -1)
        {
            m_FacingLeft = false;
            m_entity.CharacterSprite.transform.localScale = new Vector3(x: -1, y: 1, z: 1);
        }
    }
}
