using System.Collections.Generic;
using UnityEngine;

public partial class Weapon : MonoBehaviour
{
	[Header("Settings")]
	[SerializeField] private string _weaponName = "";
	[SerializeField] private Sprite _weaponSprite;
	[SerializeField] public SpriteRenderer WeaponRenderer;

	[Header("Weapon")]
	[SerializeField] private bool _singleHanded = false;
	[SerializeField] private int _damage;

	[Header("Effects")]
	[SerializeField] private ParticleSystem _weaponPs;

	[Header("Projectile Settings")]
	[SerializeField] private Vector3 m_projectileSpawnPosition;
	[SerializeField] private Vector3 m_projectileSpread;

	public Vector3 ProjectileSpawnPosition { get; set; }
	private Vector3 m_projectileSpawnOffset;
	private Vector3 m_randomProjectileSpread;

	public WeaponAim WeaponAim { get; set; }

	protected Animator _armAnimator;
	public BaseSkill _skillToUse;

	public float m_currentSpeedModifier { get; set; }
	private bool m_facingLeft;

	public Entity m_weaponOwner { get; set; }
	public int m_currentUses { get; set; }
	public List<BaseSkill> EnemySkill { get; set; }
	private BaseSkill _currentEnemySkill;

	public string WeaponName => _weaponName;
	public Sprite WeaponSprite => _weaponSprite;
	public BaseSkill SkillToUse => _skillToUse;
	public BaseSkill CurrentEnemySkill => _currentEnemySkill;
	public Animator ArmAnimator => _armAnimator;
	private readonly int _weaponUseParamter = Animator.StringToHash("WeaponUse");
	protected readonly int _armSwing = Animator.StringToHash("ArmSwing");
	private Animator _animator;
	private Animator _spriteAnimator;
	public bool IsBusy { get; set; }

	// Weapon Information
	public WeaponInfo WeaponInfo { get; set; }

	protected virtual void Awake()
	{
		EnemySkill = new List<BaseSkill>();
		_currentEnemySkill = new BaseSkill(this);

		_animator = GetComponent<Animator>();
		if (_animator == null)
			_animator = GetComponentInChildren<Animator>();

		_spriteAnimator = GetComponentInChildren<Animator>();
		WeaponAim = GetComponent<WeaponAim>();

		WeaponInfo = new WeaponInfo(_damage, Color.white);

		IsBusy = false;
		_skillToUse = new BaseSkill(this);
	}

	protected virtual void Start()
	{
		m_projectileSpawnOffset = m_projectileSpawnPosition;
		m_projectileSpawnPosition.y = -m_projectileSpawnPosition.y;

		if (m_weaponOwner == null && transform.parent != null && transform.parent.parent != null)
			m_weaponOwner = transform.parent.parent.GetComponent<Entity>(); //gameObject.GetComponent<Entity>();

		if (m_weaponOwner != null)
		m_facingLeft = m_weaponOwner.GetComponent<EntityFlip>().m_FacingLeft;

		if (!m_facingLeft)
		{
			transform.localPosition = new Vector3((transform.localPosition.x * -1), transform.localPosition.y, transform.localPosition.z);
		}
	}

	protected virtual void Update()
	{
		if (m_weaponOwner != null)
		{
			RotateWeapon();
			_skillToUse.Update();

			if (m_weaponOwner.EntityType == Entity.EntityTypes.AI)
				_currentEnemySkill.Update();
		}
	}


	public virtual void SetOwner(Entity owner)
	{
		m_weaponOwner = owner;

		if (EnemySkill != null)
		{
			foreach (var skill in EnemySkill)
				skill.SetOwner(owner);
		}
	}

	public virtual void PlayAnimation()
	{
		if (_weaponPs != null)
			_weaponPs.Play();

		if (_animator != null)
			_animator.SetTrigger(_weaponUseParamter);

		if (_spriteAnimator != null)
			_spriteAnimator.SetTrigger(_weaponUseParamter);
	}

	protected virtual void RotateWeapon()
	{
		if ((m_weaponOwner.GetComponent<EntityFlip>().m_FacingLeft) && !m_facingLeft)
		{
			m_facingLeft = true;
			transform.localScale = new Vector3(1, 1, 1);
		}
		else if ((!m_weaponOwner.GetComponent<EntityFlip>().m_FacingLeft) && m_facingLeft)
		{
			m_facingLeft = false;
			transform.localScale = new Vector3(1, 1, 1);
		}
	}


	public void SpawnProjectile(Vector2 spawnPosition, ObjectPooler poolOfObjects)
	{
		GameObject projectilePooled = poolOfObjects.GetObjectFromPool();

		projectilePooled.transform.position = spawnPosition;
		projectilePooled.SetActive(true);

		// Spread logic
		m_randomProjectileSpread.z = Random.Range(-m_projectileSpread.z, m_projectileSpread.z);
		Quaternion spread = Quaternion.Euler(m_randomProjectileSpread);

		Projectile projectile = projectilePooled.GetComponent<Projectile>();
		projectile.EnableProjectile();
		Vector2 newDirection = m_weaponOwner.GetComponent<EntityFlip>().m_FacingLeft ? (spread * transform.right * -1) : (spread * transform.right * -1);

		projectile.SetDirection(newDirection, transform.rotation, m_weaponOwner.GetComponent<EntityFlip>().m_FacingLeft);
	}

	public void EvaluateProjectileSpawnPosition()
	{
		if (!m_weaponOwner.GetComponent<EntityFlip>().m_FacingLeft)
		{
			ProjectileSpawnPosition = transform.position - transform.rotation * m_projectileSpawnOffset;
		}
		ProjectileSpawnPosition = transform.position + transform.rotation * m_projectileSpawnPosition;
	}

	protected void OnDrawGizmosSelected()
	{
		EvaluateProjectileSpawnPosition();

		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(ProjectileSpawnPosition, 0.1f);
	}

	public void DeletePooledObjects()
	{
		_skillToUse.DeletePooledObjects();
	}

	public virtual bool IsMeleeWeapon()
	{
		return false;
	}

	public virtual bool IsBowWeapon()
	{
		return false;
	}

	public virtual bool IsMagicWeapon()
	{
		return false;
	}

	public virtual bool IsShield()
	{
		return false;
	}

	public void SetArm(GameObject arm)
	{
		_armAnimator = arm.GetComponentInChildren<Animator>();
		if (_armAnimator != null)
			gameObject.transform.parent = _armAnimator.transform;
	}

	public void UseSkill(BaseSkill skill)
	{
		_skillToUse = skill;
		_skillToUse.SetOwner(m_weaponOwner);
		_skillToUse.Trigger();
	}

	public void SetWeaponInfo(WeaponInfo weaponInfo)
	{
		WeaponInfo = weaponInfo;

		_damage = weaponInfo.Damage;
		WeaponRenderer.color = weaponInfo.Color;
	}

	public void CancelSkills()
	{
		_skillToUse.CancelSkill();

		if (m_weaponOwner.EntityType == Entity.EntityTypes.AI)
			_currentEnemySkill.CancelSkill();
	}

	public void TriggerEnemySkill()
	{
		if (!_currentEnemySkill.IsLoadingOrLoaded())
		{
			int index = Random.Range(0, 100);
			int remainder = (index % EnemySkill.Count);
			_currentEnemySkill = EnemySkill[remainder];
		}

		_currentEnemySkill.Trigger();
	}

	public virtual bool IsMeleeWeaponAndBusy()
	{
		return false;
	}
}
