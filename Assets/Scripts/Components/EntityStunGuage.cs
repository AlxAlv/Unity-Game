
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityStunGuage : EntityComponent
{
    [SerializeField] GameObject _stunStars;
    [SerializeField] GameObject _knockBackBar;
    // Stun
    private const float _defaultStunDuration = 0.5f;
    private float _currentStunTimer = _defaultStunDuration;
    private float _currentStunDuration = _defaultStunDuration;

    // Invincibility
    private const float _defaultInvincibleTimer = 3.0f;
    private float _currentInvincibleTimer = _defaultInvincibleTimer;
    private float _currentInvincibleDuration = _defaultInvincibleTimer;

    // Knockback
    private const float _defaultKnockbackDistance = 5f;
    private const float _defaultKnockbackTimer = 0.8f;
    private float _currentKnockbackTimer = _defaultKnockbackTimer;
    private float _currentKnockbackDistance = _defaultKnockbackDistance;
    private float _currentKnockbackDuration = _defaultKnockbackTimer;
    private Vector2 _knockbackOrigin;
    private Vector2 _knockbackDestination;
    private Vector2 _currentKnockbackPosition;
    [SerializeField] private float _armorBreakDuration = 3.0f;

    private Transform _lastPlaceHitFrom;

    public bool Stunned { get; set; }
    public bool KnockedBack { get; set; }
    public bool Invincible { get; set; }

    // Knockback Bar
    [SerializeField] private KnockdownBar _knockdownBar;
    [SerializeField] private Transform _barPosition;
    private KnockdownBar _barInstance;

    private void Awake()
    {
        if (_knockdownBar != null && _barPosition != null)
        {
            _barInstance = Instantiate(_knockdownBar, _barPosition.position, _barPosition.rotation);
            _barInstance.transform.parent = _barPosition;

            _barInstance.SetSunGuage(this);
        }
    }

    private void FixedUpdate()
    {
        if (KnockedBack)
        {
            UpdateStunTimer();
            UpdateKnockback();
            UpdateKnockbackTimer();
        }
        else if (Invincible)
            UpdateInvincibleTimer();
        else
            UpdateStunTimer();

        UpdateStunStars();
        UpdateKnockbackUI();
    }

    private void UpdateKnockbackUI()
    {
        if ((_barInstance.CurrentGuageAmount > 0) && !KnockedBack && !Invincible)
            _knockBackBar.SetActive(true);
        else
            _knockBackBar.SetActive(false);
    }

    private void UpdateStunTimer()
    {
        if (_currentStunTimer < _currentStunDuration)
        {
            m_movement.StunMovementModifier = 0.0f;
            _currentStunTimer += Time.deltaTime;
            Stunned = true;
        }
        else
        {
            m_movement.StunMovementModifier = 1.0f;
            Stunned = false;
        }
    }

    private void UpdateInvincibleTimer()
    {
        Stunned = false;
        _currentInvincibleTimer += Time.deltaTime;
        m_movement.StunMovementModifier = 1.0f;

        if (_currentInvincibleTimer > _currentInvincibleDuration)
        {
            ResetGauges();
        }
    }

    private void UpdateStunStars()
    {
        _stunStars.SetActive(KnockedBack);

        foreach (Transform child in _stunStars.transform)
        {
            child.gameObject.SetActive(_stunStars.activeSelf);
        }
    }

    private void UpdateKnockbackTimer()
    {
        Stunned = true;

        _currentKnockbackTimer += Time.deltaTime;
        m_movement.StunMovementModifier = 0.0f;

        if (_currentStunTimer > _currentStunDuration)
        {
            BecomeInvincible();
        }
    }

    private void UpdateKnockback()
    {
        _currentKnockbackPosition = Vector2.Lerp(_knockbackOrigin, _knockbackDestination, Mathf.SmoothStep(0.0f, 1.0f, (_currentKnockbackTimer / _currentKnockbackDuration)));

        _rigidBody.MovePosition(_currentKnockbackPosition);
    }

    public void HitStun(float stunTime = _defaultStunDuration, float knockBackAmount = 0.0f, Transform projectileLocation = null)
    {
        _lastPlaceHitFrom = projectileLocation;

        if (Invincible)
        {
            SoundManager.Instance.Playsound("Audio/SoundEffects/InvincibleFx");
            return;
        }

        if (!KnockedBack)
            _currentStunTimer = 0.0f;

        _barInstance.AddAmount(knockBackAmount);

        m_entityWeapon.CurrentWeapon.CancelSkills();

        _currentStunDuration = (KnockedBack ? _armorBreakDuration : stunTime);

        if (m_entity.EntityType == Entity.EntityTypes.Player)
            m_movement.RemoveDestination();
    }

    public void BecomeKnockedback()
    {
        if (!KnockedBack)
        {
            // Become Invincible
            _currentKnockbackDuration = _defaultKnockbackTimer;
            _currentKnockbackTimer = 0.0f;

            SoundManager.Instance.Playsound("Audio/SoundEffects/KnockbackFx");
            KnockedBack = true;

            GetComponent<Health>().StunDamageModifier = 2.0f;

            // Find knockback transforms
            _knockbackOrigin = transform.position;

            _knockbackDestination = Vector2.MoveTowards(transform.position, _lastPlaceHitFrom.position, -1 * _currentKnockbackDistance);
        }
    }

    public void BecomeInvincible()
    {
        RemoveKnockbackGuage();

        GetComponent<Health>().StunDamageModifier = 0.5f;

        // Become Invincible
        _currentInvincibleDuration = _defaultInvincibleTimer;
        _currentInvincibleTimer = 0.0f;
        Invincible = true;
    }

    private void ResetGauges()
    {
        RemoveKnockbackGuage();

        Stunned = false;
        Invincible = false;

        m_movement.StunMovementModifier = 1.0f;
        _currentStunDuration = 0.0f;
        _currentInvincibleDuration = 0.0f;
        GetComponent<Health>().StunDamageModifier = 1.0f;
    }

    private void RemoveKnockbackGuage()
    {
        KnockedBack = false;
        _currentKnockbackDuration = 0.0f;
        _barInstance.ResetCurrentGuage();
    }
}
