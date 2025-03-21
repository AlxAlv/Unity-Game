﻿using Pathfinding;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum MovementState : ushort
{
    Idle = 0,
    Walking = 2,
    Running = 3
}

public class EntityMovement : EntityComponent
{
    [SerializeField] private PossibleTargetHelper _possibleTargetHelper;
    [SerializeField] private float _baseWalkSpeed = 6f;
    [SerializeField] private Transform _movementArrowTransform;
    [SerializeField] private Image _movementArrowImage;
    [SerializeField] private bool _isDummy = false;
    [SerializeField] private Transform _feetLocation;

    public float m_moveSpeed { get; set; }
    public float SkillMovementModifier { get; set; }
    public float RunMovementModifier { get; set; }
    public float StunMovementModifier { get; set; }

    public MovementState State = MovementState.Idle;

    private Vector3 _destination;
    private GameObject _currentEnemy;
    private AudioSource _movementAudio;

    public Vector3 Destination => _destination;

    // AI Movement
    private Seeker _seeker;
    private Pathfinding.Path _currentPath;
    private AIPath _aiPath;

    private bool _isMoving = false;
    private bool _enemyTargeted = false;
    private bool _canMove = true;
    private bool IsAI = false;

    private bool _isCollidingWithWall = false;
    public bool WallCollisionDetected => _isCollidingWithWall;

    protected override void Start()
    {
        base.Start();

        _movementArrowTransform = GameObject.FindWithTag("MovementArrow").transform;
        _movementArrowImage = _movementArrowTransform.GetComponentInChildren<Image>();
        _possibleTargetHelper = GameObject.FindWithTag("UIManager").GetComponent<PossibleTargetHelper>();

        m_moveSpeed = _baseWalkSpeed;
        RunMovementModifier = 1.0f;
        SkillMovementModifier = 1.0f;
        StunMovementModifier = 1.0f;

        _movementAudio = gameObject.AddComponent<AudioSource>();
        AudioClip musicToPlay = Resources.Load<AudioClip>("Audio/SoundEffects/WalkingFx");
        _movementAudio.clip = musicToPlay;
        _movementAudio.loop = true;

        if (m_entity.EntityType == Entity.EntityTypes.AI)
        {
            IsAI = true;
            _seeker = GetComponent<Seeker>();
            _aiPath = GetComponent<AIPath>();
        }

    }

    private void FixedUpdate()
    {
        if (IsAI && !_isDummy)
            UpdateAIFollowDestination();

        MoveEntity();
        UpdateMovementArrow();
    }

    private void UpdateAIFollowDestination()
    {
        _aiPath.maxSpeed = m_moveSpeed;
        _currentPath = _seeker.GetCurrentPath();

        if (_currentPath != null && _currentPath.vectorPath != null && (GetComponent<AIDestinationSetter>().target != null) && (!_entityStunGuage.KnockedBack && !_entityStunGuage.Stunned) && (!m_entityWeapon.IsAnySkillOccupied()))
        {
            _aiPath.canMove = true;
        }
        else
            _aiPath.canMove = false;
    }

    private void UpdateMovementArrow()
    {
        if (_movementArrowImage != null)
        {
            if (_destination != transform.position && _destination != Vector3.zero)
            {
                _movementArrowTransform.position = _destination;
                _movementArrowImage.enabled = true;
                _movementArrowImage.transform.position = Vector3.zero;
            }
            else
                _movementArrowImage.enabled = false;
        }
    }

    protected override void HandleComponent()
    {
        base.HandleComponent();
        UpdateAnimations();
        UpdateSounds();

        //if (m_controller.IsSkillLoaded() && !m_entityWeapon.CurrentWeapon.IsMeleeWeapon())
        //    _isMoving = false;

        m_moveSpeed = _baseWalkSpeed * RunMovementModifier * StunMovementModifier;

        if (GameSettingsManager.Instance.IsMovementModifableBySkill)
            m_moveSpeed *= SkillMovementModifier;
    }

    private void MoveEntity()
    {
        Vector2 movement = new Vector2(x: m_horizontalInput, y: m_veritcalInput);

        Vector2 moveInput = movement;

        Vector2 normalizedMovement = moveInput.normalized;
        Vector2 movementSpeed = normalizedMovement * m_moveSpeed;
        m_controller.SetMovement(movementSpeed);

        AIDestinationSetter destinationSetter = GetComponent<AIDestinationSetter>();

        if (_isMoving && !_entityStunGuage.KnockedBack && (destinationSetter == null || destinationSetter.target == null))
        {
             _rigidBody.MovePosition(Vector3.MoveTowards(transform.position, _destination, m_moveSpeed * Time.deltaTime));
        }

        if (IsAtDestination())
            _isMoving = false;
    }

    private void UpdateAnimations()
    {
        if ((((m_horizontalInput != 0f) || (m_veritcalInput != 0f)) || _isMoving) && (m_moveSpeed != 0))
        {
            State = MovementState.Walking;
        }
        else
        {
            State = MovementState.Idle;
        }
    }

    private void UpdateSounds()
    {
        //if (State == MovementState.Walking && !_movementAudio.isPlaying)
        //{
        //    _movementAudio.Play();
        //}
        //else
        //    _movementAudio.Pause();
    }

    public void SetWalkSpeed(float movementSpeed)
    {
        _baseWalkSpeed = movementSpeed;
    }

    public void ResetSpeed()
    {
        m_moveSpeed = _baseWalkSpeed;
        SkillMovementModifier = 1.0f;
        StunMovementModifier = 1.0f;
        RunMovementModifier = 1.0f;
    }

    protected override void HandleInput()
    {
        if (!MouseInputBlocker.BlockedByUI)
        {
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
            {
                _enemyTargeted = false;

                GameObject enemyUnderCursor = RaycastHelper.Instance.GetEnemyUnderCursor();

                if (enemyUnderCursor != null && GameSettingsManager.Instance.IsTargettingNeeded)
                {
                    TargetEnemy(enemyUnderCursor);
                }
                else if (_possibleTargetHelper.CurrentTarget != null)
                {
                    TargetEnemy(_possibleTargetHelper.CurrentTarget);
                }
            }

            if (Input.GetKeyDown(KeyCode.LeftControl))
                RemoveDestination();

            if ((((!_entityTarget.IsTargettingEnemy() || _entityTarget.CurrentTarget == null) && !_enemyTargeted) || !GameSettingsManager.Instance.IsPlayerMovementStoppedWhenAiming)
              && (!Input.GetKey(KeyCode.LeftShift))
              && (!Input.GetKey(KeyCode.LeftControl)))
            {
                if ((Input.GetMouseButton(0) || (GameSettingsManager.Instance.IsRightClickAlsoMove && Input.GetMouseButton(1))) && _canMove && !RaycastHelper.Instance.IsPlayerUnderCursor() && (!DialogueManager.Instance.Animator.GetBool("IsOpen")))
                {
                    SetMouseDestination();
                    HideCursorHelper.Instance.MovementFlag = false;
                }
                else
                    HideCursorHelper.Instance.MovementFlag = true;
            }
        }
        else if (Input.GetMouseButtonDown(0))
            _canMove = false;

        if (Input.GetMouseButtonUp(0))
            _canMove = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
	    _isCollidingWithWall = true;
        _isMoving = false;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        _isCollidingWithWall = false;
    }

    private void SetMouseDestination()
    {
        _destination = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _destination.z = transform.position.z;

        _isMoving = true;

        if (Input.GetMouseButtonDown(0))
        {
	        GameObject dustObject = Instantiate((Resources.Load("Prefabs/Effects/WalkingDustPS") as GameObject), _feetLocation.transform.position, Quaternion.identity);
	        dustObject.transform.localScale = new Vector3((_entityFlip.m_FacingLeft ? 1 : -1), dustObject.transform.localScale.y, dustObject.transform.localScale.z);

	        dustObject.GetComponent<ParticleSystem>().Play();
        }
    }

    public void SetAIDestination(Vector3 destination)
    {
        _destination = destination;
        _destination.z = transform.position.z;

        if (!_isMoving)
        {
	        GameObject dustObject = Instantiate((Resources.Load("Prefabs/Effects/WalkingDustPS") as GameObject), _feetLocation.transform.position, Quaternion.identity);
	        dustObject.transform.localScale = new Vector3((_entityFlip.m_FacingLeft ? 1 : -1), dustObject.transform.localScale.y, dustObject.transform.localScale.z);

	        dustObject.GetComponent<ParticleSystem>().Play();
        }

        _isMoving = true;

        if (destination == transform.position)
            _isMoving = false;
    }

    public void SetAIFollow(Transform transformToFollow)
    {
        if (GetComponent<AIDestinationSetter>() != null)
            GetComponent<AIDestinationSetter>().target = transformToFollow;
        else
        {
            SetAIDestination(transformToFollow.position);
            return;
        }

        _isMoving = true;

        if (transformToFollow.position == transform.position)
            _isMoving = false;
    }

    public void StopAIMoving()
    {
        RemoveFollowTarget();
        _isMoving = false;
    }

    public void RemoveFollowTarget()
    {
        if (GetComponent<AIDestinationSetter>() != null)
        {
            GetComponent<AIDestinationSetter>().target = null;
        }

        _seeker.CancelCurrentPathRequest();
        _aiPath.canMove = false;
    }

    public void SetIsWalking(bool isWalking, GameObject enemyObject)
    {
        if (!_entityStunGuage.KnockedBack && !_entityStunGuage.Stunned)
        {
            _isMoving = isWalking;
            _currentEnemy = enemyObject;

            _destination = enemyObject.transform.position;
            _destination.z = transform.position.z;
        }
    }

    private void TargetEnemy(GameObject enemy)
    {
        _enemyTargeted = true;
        //_isMoving = false;

        _entityTarget.CurrentTarget = enemy;
    }

    public void RemoveDestination()
    {
        _destination = transform.position;

        _isMoving = false;
    }

    public bool IsAtDestination()
    {
	    return (transform.position == _destination);
    }
}
