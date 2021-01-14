using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
	[SerializeField] private GameObject m_objectPrefab;
	[SerializeField] private int m_poolSize = 10;
	[SerializeField] private bool m_poolCanExpand = true;

	private List<GameObject> m_pooledObjects;
	private GameObject m_parentObject;
	private int _damage;
	private string _skillName;
	private float _stunTime;
	private float _knockbackAmount;
	private string _collisionSound;
	private GameObject _owner;

	/* Status Info */
	private int _numberOfTicks = 0;
	private float _amountPerTick = 0.0f;
	private float _timePerTick = 0.0f;

	/* AOE Info */
	private float _radius = 0.0f;

	public LayerMask ProjectileMask;

	private void Awake()
    {
		ProjectileMask = LayerMask.GetMask("Default");
	}

	public void Refill()
    {
		DeletePooledObjects();

		m_pooledObjects = new List<GameObject>();

		for (int i = 0; i < m_poolSize; i++)
	    {
		    AddObjectToPool();
	    }
	}

    public GameObject GetObjectFromPool()
    {
	    for (int i = 0; i < m_pooledObjects.Count; i++)
	    {
		    if (!m_pooledObjects[i].activeInHierarchy)
		    {
			    return m_pooledObjects[i];
		    }
	    }

	    if (m_poolCanExpand)
	    {
		    return AddObjectToPool();
	    }

	    return null;
    }

    public GameObject AddObjectToPool()
    {
	    GameObject newObject = Instantiate(m_objectPrefab);
        newObject.SetActive(false);
        newObject.transform.parent = m_parentObject.transform;

		var projectileComponent = newObject.transform.GetComponent<Projectile>();
		var poolReturnComponent = newObject.transform.GetComponent<ReturnToPool>();
		var statusComponent = newObject.transform.GetComponent<StatusProjectile>();
		var aoeComponent = newObject.transform.GetComponent<ProjectileAOEOnImpact>();

		if (projectileComponent != null)
		{
			projectileComponent.DamageAmount = _damage;
			projectileComponent.SkillName = _skillName;
			projectileComponent.StunTime = _stunTime;
			projectileComponent.KnockBackAmount = _knockbackAmount;
			projectileComponent.Owner = _owner;
		}

		if (statusComponent != null)
		{
			statusComponent.NumberOfTicks = _numberOfTicks;
			statusComponent.AmountPerTick = _amountPerTick;
			statusComponent.TimePerTick = _timePerTick;
		}

		if (aoeComponent != null)
		{
			aoeComponent.AOESize = _radius;
		}

		if (poolReturnComponent != null)
		{
			poolReturnComponent.SetLayerMask(ProjectileMask);
			poolReturnComponent.SetCollisionSound(_collisionSound);
		}

		m_pooledObjects.Add(newObject);

        return newObject;
    }

	public void ChangeProjectile(GameObject prefabToUse, string poolName)
	{
		m_parentObject = new GameObject(name: poolName);

		m_objectPrefab = prefabToUse;
		Refill();
	}

	public void DeletePooledObjects()
	{
		if (m_pooledObjects != null && m_pooledObjects.Count > 0)
		{
			m_pooledObjects[0].transform.parent.gameObject.name = ObjectPoolCleanup.OprhanedPoolName;
		}
	}

	public void SetStatusInfo(float amountPerTick, int numberOfTicks, float timePerTick)
	{
		_amountPerTick = amountPerTick;
		_numberOfTicks = numberOfTicks;
		_timePerTick = timePerTick;
	}

	public void SetAOEInfo(float radius)
	{
		_radius = radius;
	}

	public void SetDamage(int damage)
	{
		_damage = damage;
	}

	public void SetSkillName(string skillName)
	{
		_skillName = skillName;
	}

	public void SetStunTime(float stunTime)
	{
		_stunTime = stunTime;
	}

	public void SetKnockbackAmount(float knockbackAmount)
	{
		_knockbackAmount = knockbackAmount;
	}

	public void SetCollisionSound(string sound)
	{
		_collisionSound = sound;
	}

	public void SetLayerMask(LayerMask mask)
	{
		ProjectileMask = mask;
	}

	public void SetOwner(GameObject owner)
	{
		_owner = owner;
	}
}
