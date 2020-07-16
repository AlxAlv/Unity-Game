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
	private string _collisionSound;

	private void Start()
	{
		_projectile = GetComponent<Projectile>();
	}

	private void Return()
	{
		if (_projectile != null)
		{
			_projectile.ResetProjectile();
		}

		gameObject.SetActive(false);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (CheckLayer(collision.gameObject.layer, _objectMask))
		{
			if (_projectile != null)
			{
				_projectile.DisableProjectile();
				_impactPS.Play();
				SoundManager.Instance.Playsound(_collisionSound);
				Invoke(nameof(Return), _impactPS.main.duration);

				if (collision.GetComponent<Entity>() != null)
				{
					collision.GetComponent<Health>().TakeDamage(_projectile.DamageAmount, _projectile.SkillName);
					collision.GetComponent<Health>().HitStun(_projectile.StunTime, _projectile.KnockBackAmount, _projectile.Owner.transform);
					collision.GetComponent<Health>().Attacker = _projectile.Owner;
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
