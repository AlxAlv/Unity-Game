using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelComponent : MonoBehaviour
{
	[Header("Settings")]
	[SerializeField] private Sprite _damagedSprite;
	[SerializeField] private bool _isDamageable;

	private Health _health;
	private SpriteRenderer _spriteRenderer;
	private RandomReward _randomReward;
	private Collider2D _collider2D;

	private void Start()
    {
	    _health = GetComponent<Health>();
	    _spriteRenderer = GetComponent<SpriteRenderer>();
	    _randomReward = GetComponent<RandomReward>();
		_collider2D = GetComponent<Collider2D>();
    }

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Projectile"))
		{
			TakeDamage(other.GetComponent<Projectile>().DamageAmount);
		}
		else if (other.CompareTag("MeleeWeapon"))
		{
			TakeDamage(other.GetComponentInParent<MeleeWeapon>().SkillDamage);
		}
	}

    private void TakeDamage(float damage)
    {
        _health.TakeDamage(damage, StaleMove.NonStaleMove);

        if (_health.m_currentHealth > 0)
        {
			if (_isDamageable)
				_spriteRenderer.sprite = _damagedSprite;
        }

        if (_health.m_currentHealth <= 0)
        {
	        if (_isDamageable)
		        Destroy(gameObject);
	        else
	        {
				// Jar, etc.
		        _spriteRenderer.sprite = _damagedSprite;
		        _collider2D.enabled = false;

		        if (_randomReward != null)
		        {
					_randomReward.GiveReward();
		        }
	        }
        }
    }
}
