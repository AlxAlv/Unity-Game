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

		if (projectileComponent != null)
		{
			projectileComponent.DamageAmount = _damage;
			projectileComponent.SkillName = _skillName;
			projectileComponent.StunTime = _stunTime;
			projectileComponent.KnockBackAmount = _knockbackAmount;
			projectileComponent.Owner = _owner;
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
