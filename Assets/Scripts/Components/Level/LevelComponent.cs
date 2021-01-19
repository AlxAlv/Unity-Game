using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ItemRewardType
{
	MagicPowder,
	Sawdust,
	JarDust
};

public class LevelComponent : MonoBehaviour
{

	[Header("Settings")]
	[SerializeField] private Sprite _damagedSprite;
	[SerializeField] private Sprite _brokenSprite;
	[SerializeField] private bool _isDamageable;
	[SerializeField] private bool _isDestroyable;

	public Inventory AttackerInventory;

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
			AttackerInventory = other.GetComponent<Projectile>().Owner.GetComponent<Inventory>();
			TakeDamage(other.GetComponent<Projectile>().DamageAmount, false, other.GetComponent<Projectile>().Owner.GetComponent<Inventory>());
		}
	}

    public void TakeDamage(float damage, bool isCrit, Inventory playerInventory)
    {
	    if (!_isDamageable || _health.m_currentHealth < 0)
			return;

        _health.TakeDamage(damage, StaleMove.NonStaleMove, isCrit);

        if (_health.m_currentHealth > 0)
        {
			if (_isDamageable)
				_spriteRenderer.sprite = _damagedSprite;
        }

        if (_health.m_currentHealth <= 0)
        {
	        Destroy(GetComponent<TargetHelper>());
	        GetComponent<OutlineHelper>().SetOutlineAmount(0.0f);

	        if (_isDestroyable)
		        Destroy(gameObject);
	        else
	        {
				// Jar, etc.
		        _spriteRenderer.sprite = _brokenSprite;
		        _collider2D.enabled = false;

		        if (_randomReward != null)
		        {
					_randomReward.GiveReward();
		        }
	        }
        }
    }
}
