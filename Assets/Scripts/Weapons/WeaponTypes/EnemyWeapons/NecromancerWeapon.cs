using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NecromancerWeapon : Weapon
{
	// Minions To Spawn And Weapon To Give Them
	[SerializeField] public GameObject EnemyObject;
	[SerializeField] public Weapon MinionWeapon;

	public override void SetOwner(Entity owner)
	{
		if (owner.EntityType == Entity.EntityTypes.AI)
		{
			EnemySkill.Add(new SummonMinions(this));

			foreach (var skill in EnemySkill)
				skill.SetOwner(owner);
		}

		base.SetOwner(owner);
	}

	public override bool IsMagicWeapon()
	{
		return true;
	}
}

