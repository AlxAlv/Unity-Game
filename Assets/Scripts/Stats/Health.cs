﻿using System.Collections;
using JetBrains.Annotations;
using UnityEngine;

public class Health : MonoBehaviour
{

	[Header("Health")]
	[SerializeField] private float m_initialHealth = 20f;
	[SerializeField] public float m_maxHealth = 20f;

	[Header("Shield")]
	[SerializeField] private float m_initialShield = 0f;
	[SerializeField] private float m_maxShield = 0f;

	[Header("Settings")]
	[SerializeField] private bool m_destroyableObject;
	[SerializeField] private HealthBar _healthBar;
	[SerializeField] public Transform _healthBarPosition;
	[SerializeField] private SpriteRenderer m_spriteRenderer;
	[SerializeField] private LootHelper _lootHelper;
	[SerializeField] private Camera2DShake _cameraShake;
	[SerializeField] private Transform _revivePosition;
	[SerializeField] private Transform _bottomOfSpritePosition;
	[SerializeField] private GameObject _deathParticles;

	private Entity m_entity;
	private StatManager _statManager;
	private EntityController m_controller;
	private EntityStunGuage _entityStun;
	private Collider2D m_collider2D;
	private EntityFlip _entityFlip;
	private bool _isPlayer;
	private HealthBar _healthBarInstance;
	private TintHelper _tintHelper;
	private StaleMove _staleMove = new StaleMove();

	public GameObject Attacker { get; set; }
	public float m_currentHealth { get; set; }
	public float m_currentShield { get; set; }

	public float StunDamageModifier = 1.0f;
	public float DodgeDamageModifier = 1.0f;
	public float ShieldModifier = 1.0f;

	private GameObject _player;
	private bool _maxHealthSet = false;

	public void TakeDamage(float damage, string attackName, bool isCriticalHit)
	{
		if (m_entity != null)
		{
			if (m_entity.EntityType == Entity.EntityTypes.Player)
			{
				UIManager.Instance.BounceHealthText();
				CameraFilter.Instance.Flash(Color.white, (1.75f), (0.45f));
			}
			else if (m_entity.EntityType == Entity.EntityTypes.AI)
			{
				ComboManager.Instance.IncrementCounter();
			}
		}

		if (m_entity != null && (m_entity.EntityType == Entity.EntityTypes.AI ||
		                         m_entity.EntityType == Entity.EntityTypes.Player))
		{
			if (GetComponent<EntitySounds>())
				GetComponent<EntitySounds>().PlayHitSound();

			BossHealthBar.Instance.Bounce();

			damage = (damage * StunDamageModifier * DodgeDamageModifier * ShieldModifier);
		}

		if (damage > 0 && _tintHelper != null)
			_tintHelper.SetTintColor(Color.white);

		if (m_currentHealth <= 0)
		{
			return;
		}

		if (!isCriticalHit)
			DamageNumbers.Create(transform.position, (int)damage);
		else
		{
			// New Numbers
			CriticalNumbers.Create(transform.position, (int)damage);

			// Play Sound
			SoundManager.Instance.Playsound("Audio/SoundEffects/CriticalHitFx");
		}

		m_currentHealth -= damage;
		UpdateEntityHealth();

		if (m_currentHealth <= 0)
		{
			if (GetComponent<ItemLootHelper>())
			{
				if (GetComponent<LevelComponent>() && GetComponent<LevelComponent>().AttackerInventory != null)
					GetComponent<ItemLootHelper>().GiveItems(GetComponent<LevelComponent>().AttackerInventory);
				else if (Attacker != null)
 					GetComponent<ItemLootHelper>().GiveItems(Attacker.GetComponent<Inventory>());
			}

			if (GetComponent<EntitySounds>())
				GetComponent<EntitySounds>().PlayDeathSound();

			Die();
		}
	}

	public void HitStun(float stunTimer, float knockbackAmount = 0.0f, Transform lastPlaceHitFrom = null)
	{
		if (_entityStun != null)
		{
			_entityStun.HitStun(stunTimer, knockbackAmount, lastPlaceHitFrom);
		}
	}

	private void Start()
	{
		_entityFlip = GetComponent<EntityFlip>();
		m_entity = GetComponent<Entity>();
		m_collider2D = GetComponent<Collider2D>();

		if (m_spriteRenderer == null)
			m_spriteRenderer = GetComponent<SpriteRenderer>();

		m_controller = GetComponent<EntityController>();
		_entityStun = GetComponent<EntityStunGuage>();
		_tintHelper = GetComponent<TintHelper>();
		_lootHelper = GetComponent<LootHelper>();

		if (m_entity != null)
			_statManager = m_entity.GetComponent<StatManager>();

		m_currentHealth = CalculateMaxHealth();
		m_currentShield = m_initialShield;

		if (m_entity != null)
		{
			_isPlayer = (m_entity.EntityType == Entity.EntityTypes.Player);
		}

		UpdateEntityHealth();
		Attacker = null;

		if (_healthBar != null && _healthBarPosition != null)
		{
			_healthBarInstance = Instantiate(_healthBar, _healthBarPosition.position, _healthBarPosition.rotation);
			_healthBarInstance.transform.parent = _healthBarPosition;
		}

		ShieldModifier = 1.0f;

		_player = GameObject.Find("Player");
	}

	public float CalculateMaxHealth()
	{
		if (_statManager != null)
			return m_initialHealth + (_statManager.Strength.TotalAmount * 3) + (_statManager.HealthPerLevel);

		return m_maxHealth;
	}

	private void Die()
	{
		if (m_entity != null)
		{
			m_collider2D.enabled = false;

			if (m_spriteRenderer != null && (m_entity.EntityType == Entity.EntityTypes.Player))
				m_spriteRenderer.enabled = false;
			else
			{
				m_spriteRenderer = transform.Find("Sprite").GetComponent<SpriteRenderer>();
				m_spriteRenderer.enabled = false;
			}

			m_entity.enabled = false;
			m_controller.enabled = false;
		}

		if (m_entity != null && m_entity.EntityType == Entity.EntityTypes.Player)
		{
			DialogManager.Instance.InstantSystemMessage("You've Died! Press \"M\" To Revive");
			ArenaManager.Instance.PlayerDied();
		}

		if (m_destroyableObject || (m_entity != null && m_entity.EntityType == Entity.EntityTypes.AI))
		{
			if (_lootHelper != null)
				_lootHelper.RandomizeLoot();

			GameObject dustObject = Instantiate(_deathParticles, _bottomOfSpritePosition.transform.position, Quaternion.identity);
			dustObject.transform.localScale = new Vector3((_entityFlip.m_FacingLeft ? 1 : -1), dustObject.transform.localScale.y, dustObject.transform.localScale.z);
			dustObject.GetComponent<ParticleSystem>().Play();

			FadeAwayToDeath.Instance.InitializeFadeAway(m_spriteRenderer);
			DestroyObject();
		}

		if (m_entity != null)
		{
			//if ((m_entity.EntityType == Entity.EntityTypes.AI))
			//{
			//	FadeAwayToDeath.Instance.InitializeFadeAway(m_spriteRenderer);
			//	DestroyObject();
			//}
			//else
			if ((m_entity.EntityType == Entity.EntityTypes.Player))
				gameObject.SetActive(false);
		}

		if (_player != null && _player.GetComponent<Exp>() != null && GetComponent<Exp>() != null && m_entity.EntityType != Entity.EntityTypes.Player)
			_player.GetComponent<Exp>().GainExp(GetComponent<Exp>().ExpToGive);
	}

	private void Update()
	{
		if (!_maxHealthSet)
			m_maxHealth = CalculateMaxHealth();

		if (_healthBarInstance != null)
			_healthBarInstance.UpdateHealth(m_currentHealth, m_maxHealth);

		UpdateEntityHealth();
	}

	public void Revive()
	{
		if (m_entity != null)
		{
			m_collider2D.enabled = true;
			m_spriteRenderer.enabled = true;
			m_entity.enabled = true;
			m_controller.enabled = true;

			m_currentHealth = m_initialHealth;
			m_currentShield = m_initialShield;

			EntityStunGuage stunGuage = gameObject.GetComponent<EntityStunGuage>();
			Exp exp = gameObject.GetComponent<Exp>();
			StatManager statManager = gameObject.GetComponent<StatManager>();

			exp.ResetData();
			statManager.RemoveStats();

			stunGuage.ResetGauges();
			stunGuage.RemoveKnockbackGuage();

			LevelManager.Instance.CurrentLevel = 1;

			CoinManager.Instance.DeleteCoins();

			UpdateEntityHealth();
		}

		gameObject.SetActive(true);
	}

	public void GainHealth(int amount)
	{
		if (amount > 0 && _tintHelper != null)
			_tintHelper.SetTintColor(new Color(0, 1, 0, 1f));

		HealNumbers.Create(transform.position, (int)amount);

		m_currentHealth = Mathf.Min(m_currentHealth + amount, m_maxHealth);
		UpdateEntityHealth();
	}

	public void GainShield(int amount)
	{
		m_currentShield = Mathf.Min(m_currentShield + amount, m_maxShield);
		UpdateEntityHealth();
	}

	private void DestroyObject()
	{
		Destroy(gameObject);
	}

	private void UpdateEntityHealth()
	{
		if (m_entity != null && m_entity.EntityType == Entity.EntityTypes.Player)
			UIManager.Instance.UpdateHealth(m_currentHealth, CalculateMaxHealth(), m_currentShield, m_maxShield, _isPlayer);
	}

	public void RefillHealth()
	{
		m_currentHealth = CalculateMaxHealth();
	}

	public bool IsStaled()
	{
		return _staleMove.IsStale;
	}

	public void SetHealth(float currentHealth, float maxHealth)
	{
		m_currentHealth = currentHealth;
		m_maxHealth = maxHealth;

		_maxHealthSet = true;
	}
}
