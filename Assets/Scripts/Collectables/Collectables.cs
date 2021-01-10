using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Collectables : MonoBehaviour
{
	[Header("Settings")]
	[SerializeField] private bool _canDestroyItem = true;
	[SerializeField] private int _rotationSpeed = 160;

	protected Entity _entity;
	protected GameObject _objectCollided;
	protected SpriteRenderer _spriteRenderer;
	protected Collider2D _collider2D;

	private void Start()
	{
		_spriteRenderer = GetComponent<SpriteRenderer>();
		_collider2D = GetComponent<Collider2D>();
	}

	private void Update()
	{
		transform.Rotate(0, _rotationSpeed * Time.deltaTime, 0);
	}

	private void OnTriggerEnter2D(Collider2D other)
    {
	    _objectCollided = other.gameObject;

	    if (IsPickable())
	    {
			Pick();
			PlayEffects();

			if (_canDestroyItem)
			{
				Destroy(gameObject);
			}
			else
			{
				_spriteRenderer.enabled = false;
				_collider2D.enabled = false;
			}
	    }
    }

    protected virtual bool IsPickable()
    {
	    _entity = _objectCollided.GetComponent<Entity>();

	    if (_entity == null)
	    {
		    return false;
	    }

	    return _entity.EntityType == Entity.EntityTypes.Player;
    }

    protected virtual void Pick()
    {
		
    }

    protected virtual void PlayEffects()
    {

    }
}
