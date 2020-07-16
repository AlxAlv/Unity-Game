using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityController : MonoBehaviour
{
    // Properties
    public bool IsNormalMovement { get; set; }
    public Vector2 CurrentMovement { get; set; }

    // Animator
    private Animator _animator = null;

    // Animator Parameters
    private readonly int _movingParameter = Animator.StringToHash(name: "IsMoving");
    private readonly int _skillLoadedParameter = Animator.StringToHash(name: "IsSkillLoaded");
    private readonly int _isHurtParameter = Animator.StringToHash(name: "IsHurt");

    // States
    private MovementState _movementState = MovementState.Idle;
    private SkillState _mainSkillState = SkillState.available;
    private SkillState _alternateSkillState = SkillState.available;
    private SkillState _skillToUseState = SkillState.available;

    // Scripts
    private Rigidbody2D _rigidBody = null;
    private EntityWeapon _entityWeapon = null;
    private EntityMovement _entityMovement = null;
    private EntityStunGuage _entityStun = null;

    // Questions
    public bool CanSwitchWeapons() { return !((_mainSkillState == SkillState.loaded) || (_alternateSkillState == SkillState.loaded) || (_skillToUseState == SkillState.loaded)); }

    void Start()
    {
        IsNormalMovement = true;

        _animator = GetComponent<Entity>().EntityAnimator;
        _entityMovement = GetComponent<EntityMovement>();
        _rigidBody = GetComponent<Rigidbody2D>();
        _entityWeapon = GetComponent<EntityWeapon>();
        _entityStun = GetComponent<EntityStunGuage>();
    }

    public bool IsSkillLoaded()
    {
        return ((_mainSkillState == SkillState.loaded) || (_alternateSkillState == SkillState.loaded) || (_skillToUseState == SkillState.loaded));
    }

    private void FixedUpdate()
    {
        UpdateStates();
        if (_animator != null)
            UpdateAnimator();
    }

    public void MovePosition(Vector2 newPosition)
    {
        _rigidBody.MovePosition(newPosition);
    }

    public void SetMovement(Vector2 newMovement)
    {
        CurrentMovement = newMovement;
    }

    private void UpdateStates()
    {
        if (_entityWeapon != null)
        {
            _skillToUseState = _entityWeapon.CurrentWeapon.SkillToUse.CurrentState;
        }

        if (_entityMovement != null)
            _movementState = _entityMovement.State;
    }

    private void UpdateAnimator()
    {
        _animator.SetBool(_movingParameter, (_movementState == MovementState.Walking));

        _animator.SetBool(_skillLoadedParameter, (IsSkillLoaded() && _entityMovement.SkillMovementModifier == 0));

        if (_entityStun != null)
            _animator.SetBool(_isHurtParameter, _entityStun.Stunned);
    }
}
