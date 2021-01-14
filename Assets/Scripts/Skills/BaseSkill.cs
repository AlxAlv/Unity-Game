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
	protected int _numberOfVerticies = 64;
	protected float _outlineWidth = 0.035f;
	protected float _outlineRadius = 0.0f;
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
	protected GameObject OutlineRendererObject = null;
	protected bool _isSkillShot = false;
	protected bool _isStatusProjectile = false;
	protected bool _isAOEProjectile = false;

	/* Status Info */
	protected int _numberOfTicks = 0;
	protected float _amountPerTick = 0.0f;
	protected float _timePerTick = 0.0f;

	// Resource Paths
	protected string _spritePath = "";
	protected string _soundPath = "";
	protected string _projectilePrefabPath = "";
	protected string _projectileCollisionsoundPath = "";

	protected int _damageAmount = 0;
	protected string _skillName = "BaseSkill";
	protected int _currentProjectileDamage = -1;
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

		if (_isStatusProjectile)
			_pooler.SetStatusInfo(_amountPerTick, _numberOfTicks, _timePerTick);

		if (_isAOEProjectile)
			_pooler.SetAOEInfo(_outlineRadius);

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

				if (levelComponent)
					levelComponent.TakeDamage(_damageAmount);
				else if (targetHealth)
				{
					targetHealth.TakeDamage(_damageAmount, _skillName);
					targetHealth.HitStun(_stunTime, _knockBackAmount, _entity.transform);
					targetHealth.Attacker = _entity.gameObject;
				}
			}
		}

		CancelSkill();
	}
}
