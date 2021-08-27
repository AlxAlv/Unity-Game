using UnityEngine;

public enum SkillState : ushort
{
	available = 0,
	loading = 1,
	loaded = 2,
	unavailable = 3
}

public struct SkillSettings
{
	public string ProjectilePrefabPath;
    public string SoundPath;
    public string ProjectileCollisionsoundPath;
	public float StunTime;
    public float KnockBackAmount;
    public float LoadingTime;
    public float ResourceAmount;
    public float LoadingMovementSpeedModifier;
    public BaseSkill.Resource ResourceToUse;
    public WeaponType WeaponTypeToUse;
}

public class BaseSkill : MonoBehaviour
{
	public enum Resource : ushort
	{
		Stamina = 0,
		Mana = 1,
		Health = 2,
		Ultimate = 3
	}

	[Header("Skill Settings")]
	[SerializeField] protected float _loadingTime = 0.25f;
	[SerializeField] protected float _loadingMovementSpeedModifier = 0.5f;
	[SerializeField] protected float _loadedMovementSpeedModifier = 0.0f;
	[SerializeField] protected float _resourceAmount = 2.0f;
	[SerializeField] protected Resource _resourceToUse = Resource.Mana;
	[SerializeField] protected WeaponType _weaponTypeToUse = WeaponType.Base;

	// Helpers
	public Resource ResourceType => _resourceToUse;
	public WeaponType WeaponType => _weaponTypeToUse;
	public float ResourceAmount => _resourceAmount;
	public string ToolTipInfo => _toolTipInfo;
	public string ChainEffectPrefabPath => _chainPrefabPath;

	// Member Variables
	public SkillState CurrentState { get; set; }

	public string SkillName => _skillName;
	public string IconName => _iconName;
	protected float _distanceToAttack = 5.0f;
	protected float _startTime = 0f;
	protected float _outlineWidth = 0.035f;
	protected float _outlineRadius = 0.0f;
	protected float _skillHaste = -1.0f;
	protected int _numberOfVerticies = 64;
	protected EntitySkill _entitySkill;
	protected Weapon _weaponToUse;
	protected Entity _entity;
	protected EntityMovement _entityMovement;
	protected EntityTarget _entityTarget;
	protected StatManager _statManager;
	protected Mana _mana;
	protected Stamina _stamina;
	protected Health _health;
	protected UltimatePoints _ultimatePoints;
	protected GameObject OutlineRendererObject = null;
	protected bool _isStatusProjectile = false;
	protected bool _isAOEProjectile = false;
	protected bool _isExecutingSkill = false;
	protected string _meleeFxPath;
	protected string _toolTipInfo = "";

	/* Status Info */
	protected int _numberOfTicks = 0;
	protected float _amountPerTick = 0.0f;
	protected float _timePerTick = 0.0f;

	// Resource Paths
	protected string _skillName = "";
	protected string _iconName = "";
	protected string _spritePath = "SkillIcons/";
	protected string _soundPath = "Audio/SoundEffects/";
	protected string _projectilePrefabPath = "Prefabs/Projectiles/";
	protected string _chainPrefabPath = "Sprites/Materials/";
	protected string _projectileCollisionsoundPath = "Audio/SoundEffects/";

	protected int _damageAmount = 0;
	protected int _currentProjectileDamage = -1;
	protected float _stunTime = 0.0f;
	protected float _knockBackAmount = 0.0f;

	public BaseSkill() { }

	public BaseSkill(Weapon weaponToUse)
	{
		_weaponToUse = weaponToUse;
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

		if (_outlineRadius > 0.0f)
			UpdateOutlineRenderer();

		UpdateDamage();
		UpdateSkillHaste();
	}

	protected void SetupBaseSkill(string skillName)
	{
		_skillName = skillName;
		_iconName = _skillName + "Icon";
		_spritePath += _iconName;

		if (SkillInfoRepository.Instance.SkillInfoMap.ContainsKey(_skillName))
		{
			SkillInfo skillInfo = SkillInfoRepository.Instance.SkillInfoMap[_skillName];

			_projectilePrefabPath += skillInfo.ProjectilePrefabPath;
			_soundPath += skillInfo.SoundPath;
			_projectileCollisionsoundPath += skillInfo.ProjectileCollisionsoundPath;
			_chainPrefabPath += skillInfo.ChainMaterialPath;
			_toolTipInfo = skillInfo.ToolTipInfo;
			_stunTime = skillInfo.StunTime;
			_knockBackAmount = skillInfo.KnockBackAmount;
			_loadingTime = skillInfo.LoadingTime;
			_loadingMovementSpeedModifier = skillInfo.LoadingMovementSpeedModifier;
			_loadedMovementSpeedModifier = skillInfo.LoadedMovementSpeedModifier;
			_resourceAmount = skillInfo.ResourceAmount;
			_resourceToUse = skillInfo.TheResourceType;
			_weaponTypeToUse = skillInfo.TheWeaponType;
			_isAOEProjectile = skillInfo.IsAOEProjectile;
			_outlineRadius = skillInfo.OutlineRadius;
			_timePerTick = skillInfo.TimePerTick;
			_numberOfTicks = skillInfo.NumberOfTicks;
			_isStatusProjectile = skillInfo.IsStatusProjectile;
			_distanceToAttack = skillInfo.DistanceToAttack;
			_meleeFxPath = skillInfo.MeleeSoundPath;
		}
		else
			Debug.LogError(_skillName + " does not have its SkillInfoRepository setup!");
	}

	protected virtual void UpdateSkillHaste()
	{
		if (_skillHaste == -1.0f)
		{
			_skillHaste = ((100 - _weaponToUse.SkillHaste) / 100.0f);
			_loadingTime = (_loadingTime * _skillHaste);
		}
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
		if (_loadingTime < timePassed || GameSettingsManager.Instance.IsSkillLoadingInstant)
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
		_ultimatePoints = _entity.GetComponent<UltimatePoints>();
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

	protected virtual void ShootProjectile(Vector2 spawnPosition)
	{
		GameObject projectileCreated = SpawnProjectile();

		projectileCreated.transform.position = spawnPosition;
		projectileCreated.SetActive(true);

		// Spread logic
		Vector3 m_randomProjectileSpread = new Vector3();
		Vector3 m_projectileSpread = new Vector3(0,0,0);
		m_randomProjectileSpread.z = Random.Range(-m_projectileSpread.z, m_projectileSpread.z);
		Quaternion spread = Quaternion.Euler(m_randomProjectileSpread);

		Projectile projectile = projectileCreated.GetComponent<Projectile>();
		projectile.EnableProjectile();
		Vector2 newDirection = _entity.GetComponent<EntityFlip>().m_FacingLeft ? (spread * _weaponToUse.transform.right * -1) : (spread * _weaponToUse.transform.right * -1);

		projectile.SetDirection(newDirection, _weaponToUse.transform.rotation, _entity.GetComponent<EntityFlip>().m_FacingLeft);
	}

	protected virtual GameObject SpawnProjectile()
	{
		GameObject prefabToUse = Resources.Load(_projectilePrefabPath) as GameObject;
		GameObject newObject = Instantiate(prefabToUse);

		var projectileComponent = newObject.transform.GetComponent<Projectile>();
		var poolReturnComponent = newObject.transform.GetComponent<ReturnToPool>();
		var statusComponent = newObject.transform.GetComponent<StatusProjectile>();
		var aoeComponent = newObject.transform.GetComponent<ProjectileAOEOnImpact>();
		var chainComponent = newObject.transform.GetComponent<ChainProjectile>();

		if (projectileComponent != null)
		{
			projectileComponent.DamageAmount = _damageAmount;
			projectileComponent.CriticalChance = _weaponToUse.WeaponInfo.CriticalChance;
			projectileComponent.SkillName = _skillName;
			projectileComponent.StunTime = _stunTime;
			projectileComponent.KnockBackAmount = _knockBackAmount;
			projectileComponent.Owner = _entity.gameObject;
		}

		if (statusComponent != null)
		{
			statusComponent.NumberOfTicks = _numberOfTicks;
			statusComponent.AmountPerTick = _amountPerTick;
			statusComponent.TimePerTick = _timePerTick;
		}

		if (aoeComponent != null)
		{
			aoeComponent.AOESize = _outlineRadius;
		}
		else if (chainComponent != null)
			chainComponent.AoeRadius = _outlineRadius;

		if (poolReturnComponent != null)
		{
			poolReturnComponent.SetLayerMask(GetLayerMaskToAssign());
			poolReturnComponent.SetCollisionSound(_projectileCollisionsoundPath);
		}

		return newObject;
	}

	protected virtual bool UseAbilityResource(float amount)
	{
		if (_entity.EntityType == Entity.EntityTypes.Player)
		{
			_ultimatePoints.GainUltimatePoints(100);

			switch (_resourceToUse)
			{
				case Resource.Health:
					break;

				case Resource.Mana:
					return _mana.UseMana(_resourceAmount);

				case Resource.Stamina:
					return _stamina.UseStamina(_resourceAmount);

				case Resource.Ultimate:
					return _ultimatePoints.UseUltimateBarPoints(_resourceAmount);
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
				_currentProjectileDamage = _damageAmount;
			}
		}
	}

	protected virtual LayerMask GetLayerMaskToAssign()
	{
		if (_entity != null)
		{
			if (_entity.EntityType == Entity.EntityTypes.Player)
				return LayerMask.GetMask("Walls", "LevelComponents", "Enemies");
			else
				return LayerMask.GetMask("Walls", "LevelComponents", "Player");
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

	protected void DrawPolygon(LineRenderer LineRenderer, int vertexNumber, float radius, Vector3 centerPos, float width, Color color)
	{
		LineRenderer.startWidth = width;
		LineRenderer.endWidth = width;
		LineRenderer.startColor = color;
		LineRenderer.endColor = color;
		LineRenderer.loop = true;
		float angle = 2 * Mathf.PI / vertexNumber;
		LineRenderer.positionCount = vertexNumber;

		for (int i = 0; i < vertexNumber; i++)
		{
			Matrix4x4 rotationMatrix = new Matrix4x4(new Vector4(Mathf.Cos(angle * i), Mathf.Sin(angle * i), 0, 0),
				new Vector4(-1 * Mathf.Sin(angle * i), Mathf.Cos(angle * i), 0, 0),
				new Vector4(0, 0, 1, 0),
				new Vector4(0, 0, 0, 1));
			Vector3 initialRelativePosition = new Vector3(0, radius, 0);
			LineRenderer.SetPosition(i, centerPos + rotationMatrix.MultiplyPoint(initialRelativePosition));
		}
	}

	protected void UpdateOutlineRenderer()
	{
		if (_entity.EntityType == Entity.EntityTypes.AI)
			return;

		if (CurrentState == SkillState.loading || CurrentState == SkillState.loaded)
		{
			if (OutlineRendererObject == null)
			{
				Object outlineRenderer = Resources.Load("Prefabs/UI/OutlineLineRenderer");
				OutlineRendererObject = (GameObject)Instantiate(outlineRenderer, _entity.transform.position, Quaternion.identity);
			}

			Transform position = null;
			if ((_entityTarget._potentialTarget != null) && Input.GetKey(KeyCode.S))
				position = _entityTarget._potentialTarget.transform;
			else if (_entityTarget.CurrentTarget != null)
				position = _entityTarget.CurrentTarget.transform;
			else if (RaycastHelper.Instance.GetEnemyUnderCursor() != null)
				position = RaycastHelper.Instance.GetEnemyUnderCursor().transform;

			if (position != null)
			{
				DrawPolygon(OutlineRendererObject.GetComponent<LineRenderer>(), _numberOfVerticies, _outlineRadius, position.position, _outlineWidth, Color.red);
				OutlineRendererObject.transform.position = position.position;
			}
			else
				Destroy(OutlineRendererObject);
		}
		else
			Destroy(OutlineRendererObject);
	}

	protected void ExecuteAOESkill()
	{
		SoundManager.Instance.Playsound(_soundPath);

		Collider2D[] _targetCollider2D = Physics2D.OverlapCircleAll(OutlineRendererObject.transform.position, _outlineRadius, LayerMask.GetMask("LevelComponents", "Enemies"));

		foreach (Collider2D collider in _targetCollider2D)
		{
			RaycastHit2D hit = Physics2D.Linecast(OutlineRendererObject.transform.position, collider.transform.position, LayerMask.GetMask("LevelComponents", "Enemies"));

			if (hit)
			{
				LevelComponent levelComponent = collider.GetComponent<LevelComponent>();
				Health targetHealth = collider.GetComponent<Health>();

				// Critical Chance
				bool isCriticalHit = (Random.Range(0, 101) < _weaponToUse.CriticalChance);

				if (levelComponent)
					levelComponent.TakeDamage((isCriticalHit ? (_damageAmount * 2) : _damageAmount), false, _weaponToUse.m_weaponOwner.GetComponent<Inventory>());
				else if (targetHealth)
				{
					targetHealth.Attacker = _weaponToUse.m_weaponOwner.gameObject;
					targetHealth.TakeDamage((isCriticalHit ? (_damageAmount * 2) : _damageAmount), _skillName, isCriticalHit);
					targetHealth.HitStun(_stunTime, _knockBackAmount, _entity.transform);
					targetHealth.Attacker = _entity.gameObject;
				}
			}
		}

		CancelSkill();
	}
}
