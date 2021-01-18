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

	private Projectile _projectile;
	private StatusProjectile _statusProjectile;

	private string _collisionSound;

	private void Start()
	{
		_projectile = GetComponent<Projectile>();
		_statusProjectile = GetComponent<StatusProjectile>();
	}

	private void Return()
	{
		if (_projectile != null)
		{
			_projectile.ResetProjectile();
		}

		Destroy(gameObject);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (CheckLayer(collision.gameObject.layer, _objectMask))
		{
			if (_projectile != null)
			{
				_projectile.DisableProjectile();
				
				if (_impactPS)
					_impactPS.Play();

				SoundManager.Instance.Playsound(_collisionSound);
				Invoke(nameof(Return), _impactPS.main.duration);

				ProjectileAOEOnImpact aoeComponent = _projectile.GetComponent<ProjectileAOEOnImpact>();

				if (aoeComponent)
				{
					float radius = aoeComponent.AOESize;

					Collider2D[] _targetCollider2D = Physics2D.OverlapCircleAll(collision.transform.position, radius, LayerMask.GetMask("LevelComponents", "Enemies"));

					foreach (Collider2D collider in _targetCollider2D)
					{
						RaycastHit2D hit = Physics2D.Linecast(collision.transform.position, collider.transform.position, LayerMask.GetMask("LevelComponents", "Enemies"));

						if (hit)
						{
							LevelComponent levelComponent = collider.GetComponent<LevelComponent>();
							Health targetHealth = collider.GetComponent<Health>();

							if (levelComponent)
								levelComponent.TakeDamage(_projectile.DamageAmount);
							else if (targetHealth)
							{
								targetHealth.TakeDamage(_projectile.DamageAmount, _projectile.SkillName);
								targetHealth.HitStun(_projectile.StunTime, _projectile.KnockBackAmount, _projectile.Owner.transform);
								targetHealth.Attacker = _projectile.Owner;
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
						collision.GetComponent<Health>().TakeDamage(_projectile.DamageAmount, _projectile.SkillName);
						collision.GetComponent<Health>().HitStun(_projectile.StunTime, _projectile.KnockBackAmount,
							_projectile.Owner.transform);
						collision.GetComponent<Health>().Attacker = _projectile.Owner;

						if (_statusProjectile != null)
							_statusProjectile.ApplyEffect(collision.GetComponent<EntityStatus>());
					}
				}
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
