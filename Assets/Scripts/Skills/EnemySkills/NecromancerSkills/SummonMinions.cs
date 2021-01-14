using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonMinions : NecromancerSkill
{
	// Skillbar Helper Static
	public static float ResourceAmount = 1.0f;
	public static Resource ResourceType = Resource.Mana;

	[SerializeField] float _xOffset = 2.0f;
	[SerializeField] float _yOffset = 2.0f;
	[SerializeField] int _maxNumberOfSpawns = 3;

	private int _currentNumberOfSpawns = 0;

	public SummonMinions(NecromancerWeapon weaponToUse) : base(weaponToUse)
	{
		_stunTime = 0.0f;
		_necromancerWeaponToUse = weaponToUse;
		_loadingTime = 0.0f;
		_knockBackAmount = 35f;
		_spritePath = "SkillIcons/MinionSummonIcon";
		_soundPath = "Audio/SoundEffects/MinionSummonFx";
		_skillName = "SummonMinions";

		_resourceAmount = ResourceAmount;
		_resourceToUse = ResourceType;
	}

	protected override void Execute()
	{
		_currentNumberOfSpawns = 0;

		foreach (Transform eachChild in _entity.gameObject.transform)
		{
			if (eachChild.name == (_necromancerWeaponToUse.EnemyObject.name + "(Clone)"))
			{
				_currentNumberOfSpawns++;
			}
		}

		while (_currentNumberOfSpawns < _maxNumberOfSpawns)
		{
			Vector2 whereToSpawn;

			RaycastHit2D hit;

			do
			{
				float randXPos = Random.Range(-_xOffset, _xOffset);
				float randYPos = Random.Range(-_yOffset, _yOffset);
				whereToSpawn = new Vector2(randXPos + _entity.gameObject.transform.position.x,
					randYPos + _entity.gameObject.transform.position.y);

				hit = Physics2D.BoxCast(whereToSpawn,
					_necromancerWeaponToUse.EnemyObject.GetComponent<BoxCollider2D>().size, 0.0f, Vector2.zero, 0,
					LayerMask.GetMask("LevelComponents"));
			} while (hit.collider != null);

			// Summon The Minion
			GameObject objectCreated = Instantiate(_necromancerWeaponToUse.EnemyObject, whereToSpawn, Quaternion.identity);

			// Fix Scaling
			Vector3 scaleTmp = objectCreated.transform.localScale;
			scaleTmp.x /= _entity.gameObject.transform.localScale.x;
			scaleTmp.y /= _entity.gameObject.transform.localScale.y;
			scaleTmp.z /= _entity.gameObject.transform.localScale.z;
			objectCreated.transform.parent = _entity.gameObject.transform;
			objectCreated.transform.localScale = scaleTmp;

			// Give The Minion A Weapon
			objectCreated.GetComponent<EntityWeapon>().MainWeapon = _necromancerWeaponToUse.MinionWeapon;

			// Set Target To Player
			objectCreated.GetComponent<AIStateController>().Target = _entityTarget.CurrentTarget.transform;

			// Increment
			_currentNumberOfSpawns++;
		}

		base.Execute();
	}

	protected override void UpdateDamage()
	{
		_damageAmount = _statManager.Intelligence.TotalAmount * 1 + _necromancerWeaponToUse.WeaponInfo.Damage;

		base.UpdateDamage();
	}
}
