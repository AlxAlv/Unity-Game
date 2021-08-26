using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToPool : MonoBehaviour
{
	[Header("Settings")]
	[SerializeField] private LayerMask _objectMask;
	[SerializeField] private float m_lifeTime = 2f;

	[Header("Effects")]
	[SerializeField] private ParticleSystem _impactPS;

	// Projectile Components
	private Projectile _projectile;
	private StatusProjectile _statusProjectile;
	private ProjectileAOEOnImpact _aoeComponent;
	private ChainProjectile _chainComponent;
	private ParticleProjectileHelper _particleHelper;

	private string _collisionSound;
	private bool _alreadyHit = false;

	private void Start()
	{
		_projectile = GetComponent<Projectile>();
		_statusProjectile = GetComponent<StatusProjectile>();
		_aoeComponent = _projectile.GetComponent<ProjectileAOEOnImpact>();
		_chainComponent = _projectile.GetComponent<ChainProjectile>();
		_particleHelper = _projectile.GetComponent<ParticleProjectileHelper>();
	}

	private void Return()
	{
		if (_projectile != null)
		{
			_projectile.ResetProjectile();
		}

		if (_projectile.ProjectileLight)
			_projectile.ProjectileLight.gameObject.SetActive(false);

		if (_particleHelper)
			KeepParticlesAlive();
		else
			Destroy(gameObject);
	}

	private void Update()
	{
		if (_particleHelper && !_particleHelper.ParticlesSystem.IsAlive())
			Destroy(gameObject);
	}

	private void KeepParticlesAlive()
	{
		_particleHelper.ParticlesSystem.Stop();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (_alreadyHit)
			return;

		if (CheckLayer(collision.gameObject.layer, _objectMask))
		{
			if (_projectile != null)
			{
				_projectile.DisableProjectile();
				
				if (_impactPS)
					_impactPS.Play();

				SoundManager.Instance.Playsound(_collisionSound);
				Invoke(nameof(Return), _impactPS.main.duration);


				// Critical Chance
				bool isCriticalHit = (Random.Range(0, 101) < _projectile.CriticalChance);

				if (_aoeComponent)
				{
					float radius = _aoeComponent.AOESize;

					Collider2D[] _targetCollider2D = Physics2D.OverlapCircleAll(collision.transform.position, radius, LayerMask.GetMask("LevelComponents", "Enemies"));

					foreach (Collider2D collider in _targetCollider2D)
					{
						RaycastHit2D hit = Physics2D.Linecast(collision.transform.position, collider.transform.position, LayerMask.GetMask("LevelComponents", "Enemies"));

						if (hit)
						{
							LevelComponent levelComponent = collider.GetComponent<LevelComponent>();
							Health targetHealth = collider.GetComponent<Health>();

							if (levelComponent)
							{
								levelComponent.TakeDamage((isCriticalHit ? (_projectile.DamageAmount * 2) : _projectile.DamageAmount), isCriticalHit, _projectile.Owner.GetComponent<Inventory>());
							}
							else if (targetHealth)
							{
								targetHealth.Attacker = _projectile.Owner;
								targetHealth.TakeDamage((isCriticalHit ? (_projectile.DamageAmount * 2) : _projectile.DamageAmount), _projectile.SkillName, isCriticalHit);
								targetHealth.HitStun(_projectile.StunTime, _projectile.KnockBackAmount, _projectile.Owner.transform);
							}
						}
					}
				}
				else
				{
					if (collision.GetComponent<Entity>() != null)
					{
						Camera2DShake.Instance.Shake();
						ScreenPause.Instance.Freeze();
						collision.GetComponent<Health>().Attacker = _projectile.Owner;
						collision.GetComponent<Health>().TakeDamage((isCriticalHit ? (_projectile.DamageAmount * 2) : _projectile.DamageAmount), _projectile.SkillName, isCriticalHit);
						collision.GetComponent<Health>().HitStun(_projectile.StunTime, _projectile.KnockBackAmount,
							_projectile.Owner.transform);

						if (_statusProjectile != null)
							_statusProjectile.ApplyEffect(collision.GetComponent<EntityStatus>());
					}
				}
				HandleChainProjectile(collision);
				_alreadyHit = true;
			}
		}
	}

	private void HandleChainProjectile(Collider2D collision)
	{
		if (!_chainComponent)
			return;

		bool isCriticalHit = (Random.Range(0, 101) < _projectile.CriticalChance);
		int hits = 0;

		Collider2D entityCollider = collision;
		List<Collider2D> entitiesHit = new List<Collider2D>();
		List<Collider2D> entitesAlreadyDamaged = new List<Collider2D>();

		while (hits < _chainComponent.MaxBounces && entityCollider)
		{
			Collider2D[] targetCollider2D = Physics2D.OverlapCircleAll(entityCollider.transform.position, _chainComponent.AoeRadius, LayerMask.GetMask("LevelComponents", "Enemies"));

			foreach (Collider2D collider in targetCollider2D)
			{
				RaycastHit2D hit = Physics2D.Linecast(collision.transform.position, collider.transform.position, LayerMask.GetMask("LevelComponents", "Enemies"));

				if (hit && !entitesAlreadyDamaged.Contains(collider) && collider != entityCollider)
					entitiesHit.Add(collider);
			}

			entityCollider = null;

			if (entitiesHit.Count > 0 && entitiesHit.Count > entitesAlreadyDamaged.Count)
			{
				int enemyIndex = Random.Range(0, entitiesHit.Count);

				LevelComponent levelComponent = entitiesHit[enemyIndex].GetComponent<LevelComponent>();
				Health targetHealth = entitiesHit[enemyIndex].GetComponent<Health>();

				if (levelComponent)
				{
					levelComponent.TakeDamage((isCriticalHit ? (_projectile.DamageAmount * 2) : _projectile.DamageAmount), isCriticalHit, _projectile.Owner.GetComponent<Inventory>());
				}
				else if (targetHealth)
				{
					targetHealth.Attacker = _projectile.Owner;
					targetHealth.TakeDamage((isCriticalHit ? (_projectile.DamageAmount * 2) : _projectile.DamageAmount), _projectile.SkillName, isCriticalHit);
					targetHealth.HitStun(_projectile.StunTime, _projectile.KnockBackAmount, _projectile.Owner.transform);
				}

				entityCollider = entitiesHit[enemyIndex];
				entitesAlreadyDamaged.Add(entitiesHit[enemyIndex]);
				hits++;
			}
		}
	}

	private bool CheckLayer(int layer, LayerMask objectMask)
	{
		return ((1 << layer) & objectMask) != 0;
	}

	private void OnEnable()
	{
		Invoke(nameof(Return), m_lifeTime);
	}

	private void OnDisable()
	{
		CancelInvoke();
	}

	public void SetLayerMask(LayerMask layerMask)
	{
		_objectMask = layerMask;
	}

	public void SetCollisionSound(string collisionSound)
	{
		_collisionSound = collisionSound;
	}
}
