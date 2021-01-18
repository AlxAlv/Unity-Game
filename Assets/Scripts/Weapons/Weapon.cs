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

	public WeaponAim WeaponAim { get; set; }
	public BaseSkill _skillToUse;

	private bool m_facingLeft;

	public Entity m_weaponOwner { get; set; }
	public int m_currentUses { get; set; }
	public List<BaseSkill> EnemySkill { get; set; }
	private BaseSkill _currentEnemySkill;

	public string WeaponName => _weaponName;
	public BaseSkill SkillToUse => _skillToUse;
	public BaseSkill CurrentEnemySkill => _currentEnemySkill;
	public bool IsBusy { get; set; }

	// Weapon Information
	public WeaponInfo WeaponInfo { get; set; }

	protected virtual void Awake()
	{
		EnemySkill = new List<BaseSkill>();
		_currentEnemySkill = new BaseSkill(this);

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

	public SkillState GetEnemySkillState()
	{
		return _currentEnemySkill.CurrentState;
	}

	public virtual bool IsMeleeWeaponAndBusy()
	{
		return false;
	}
}
