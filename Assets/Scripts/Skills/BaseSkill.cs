using UnityEngine;

public enum SkillState : ushort
{
	available = 0,
	loading = 1,
	loaded = 2,
	unavailable = 3
}

public class BaseSkill : MonoBehaviour
{
	public enum Resource : ushort
	{
		Stamina = 0,
		Mana = 1,
		Health = 2
	}

	[Header("Skill Settings")]
	[SerializeField] protected float _loadingTime = 0.25f;
	[SerializeField] protected float _loadingMovementSpeedModifier = 0.5f;
	[SerializeField] protected float _loadedMovementSpeedModifier = 0.0f;
	[SerializeField] protected float _resourceAmount = 2.0f;
	[SerializeField] protected Resource _resourceToUse = Resource.Mana;

	// Member Variables
	public SkillState CurrentState { get; set; }
	protected float _startTime = 0f;
	protected EntitySkill _entitySkill;
	protected ObjectPooler _pooler;
	protected Weapon _weaponToUse;
	protected Entity _entity;
	protected EntityMovement _entityMovement;
	protected EntityTarget _entityTarget;
	protected StatManager _statManager;
	protected Mana _mana;
	protected Stamina _stamina;
	protected Health _health;

	// Resource Paths
	protected string _spritePath = "";
	protected string _soundPath = "";
	protected string _projectilePrefabPath = "";
	protected string _projectileCollisionsoundPath = "";

	protected int _damageAmount = 0;
	protected string _skillName = "BaseSkill";
	protected int _currentProjectileDamage = 0;
	protected float _stunTime = 0.0f;
	protected float _knockBackAmount = 0.0f;

	public BaseSkill(Weapon weaponToUse)
	{
		_weaponToUse = weaponToUse;
		_pooler = new ObjectPooler();
	}

	protected virtual void Awake()
	{
		CurrentState = SkillState.available;
	}

	public virtual void Update()
	{
		if (CurrentState == SkillState.loading)
		{
			LoadingUpdate();
		}

		UpdateDamage();
	}

	public virtual void Trigger()
	{
		if (CurrentState == SkillState.available && !_weaponToUse.IsBusy && UseAbilityResource(_resourceAmount))
		{
			_weaponToUse.IsBusy = true;
			_startTime = Time.time;
			CurrentState = SkillState.loading;

			// Play skill loading sound
			SoundManager.Instance.Playsound("Audio/SoundEffects/SkillLoadingFx");

			_entitySkill.m_skillIcon.sprite = Resources.Load<Sprite>(_spritePath);
			_entitySkill.m_skillIcon.gameObject.transform.localScale = new Vector2(0f, 0f);
			SetIsSkillIconEnabled(true);

			_entityMovement.SkillMovementModifier = _loadingMovementSpeedModifier;
		}
		else if (CurrentState == SkillState.loaded)
		{
			Execute();
		}
	}

	protected virtual void LoadingUpdate()
	{
		float timePassed = (Time.time - _startTime);
		if (_loadingTime < timePassed)
		{
			SkillLoaded();
		}
		else
			_entitySkill.m_skillIcon.gameObject.transform.localScale = new Vector2(((timePassed / _loadingTime) * 1.0f), ((timePassed / _loadingTime) * 1.0f));
	}

	protected virtual void SkillLoaded()
	{
		CurrentState = SkillState.loaded;

		_entityMovement.SkillMovementModifier = _loadedMovementSpeedModifier;

		// Play skill loaded sound
		SoundManager.Instance.Playsound("Audio/SoundEffects/SkillLoadedFx");

		_entitySkill.m_skillIcon.gameObject.transform.localScale = new Vector2(1.0f, 1.0f);
	}

	protected virtual void Execute()
	{
		_weaponToUse.PlayAnimation();

		SoundManager.Instance.Playsound(_soundPath);

		CancelSkill();
	}

	public virtual void SetOwner(Entity anEntity)
	{
		_entity = anEntity;
		_entitySkill = _entity.GetComponent<EntitySkill>();
		_entityMovement = _entity.GetComponent<EntityMovement>();
		_entityTarget = _entity.GetComponent<EntityTarget>();
		_statManager = _entity.GetComponent<StatManager>();

		SetIsSkillIconEnabled(false);

		_health = _entity.GetComponent<Health>();
		_mana = _entity.GetComponent<Mana>();
		_stamina = _entity.GetComponent<Stamina>();
	}

	public virtual void CancelSkill()
	{
		_weaponToUse.IsBusy = false;
		CurrentState = SkillState.available;
		SetIsSkillIconEnabled(false);

		if (_entityMovement != null)
			_entityMovement.SkillMovementModifier = 1.0f;
	}

	protected virtual void SetIsSkillIconEnabled(bool isEnabled)
	{
		if (_entitySkill != null && _entitySkill.m_skillIcon != null)
		{
			_entitySkill.m_skillIcon.enabled = isEnabled;

			if (!isEnabled)
				_entitySkill.SkillUses.text = "";
		}
	}

	protected virtual void SetProjectileGameObject(string projectilePrefabPath)
	{
		GameObject prefabToUse = Resources.Load(projectilePrefabPath) as GameObject;

		_pooler.SetDamage(_damageAmount);
		_pooler.SetSkillName(_skillName);
		_pooler.SetStunTime(_stunTime);
		_pooler.SetKnockbackAmount(_knockBackAmount);
		_pooler.SetCollisionSound(_projectileCollisionsoundPath);
		_pooler.SetLayerMask(GetLayerMaskToAssign());

		if (_entity != null)
		{
			_pooler.SetOwner(_entity.transform.root.gameObject);
			_pooler.ChangeProjectile(prefabToUse, (_entity.EntityType == Entity.EntityTypes.Player ? "Pool" : "EnemyPool"));
		}
	}

	protected virtual bool UseAbilityResource(float amount)
	{
		if (_entity.EntityType == Entity.EntityTypes.Player)
		{
			switch (_resourceToUse)
			{
				case Resource.Health:
					break;

				case Resource.Mana:
					return _mana.UseMana(_resourceAmount);

				case Resource.Stamina:
					return _stamina.UseStamina(_resourceAmount);
			}

			return false;
		}
		else
		{
			return true;
		}
	}

	protected virtual void UpdateDamage()
	{
		if (_projectilePrefabPath.Length > 0)
		{
			if (_damageAmount != _currentProjectileDamage)
			{
				SetProjectileGameObject(_projectilePrefabPath);
				_currentProjectileDamage = _damageAmount;
			}
		}
	}

	public void DeletePooledObjects()
	{
		_pooler.DeletePooledObjects();
	}

	protected virtual LayerMask GetLayerMaskToAssign()
	{
		if (_entity != null)
		{
			if (_entity.EntityType == Entity.EntityTypes.Player)
				return LayerMask.GetMask("LevelComponents", "Enemies");
			else
				return LayerMask.GetMask("LevelComponents", "Player");
		}

		return LayerMask.GetMask("Default");
	}

	public bool IsLoadingOrLoaded()
	{
		return ((CurrentState == SkillState.loading) || (CurrentState == SkillState.loaded));
	}

	public bool IsLoaded()
	{
		return (CurrentState == SkillState.loaded);
	}

	public bool IsLoading()
	{
		return (CurrentState == SkillState.loading);
	}

	public virtual bool IsBase()
	{
		return true;
	}

	public string GetSkillIconPath()
	{
		return _spritePath;
	}

	public float GetResourceAmount()
	{
		return _resourceAmount;
	}
}
