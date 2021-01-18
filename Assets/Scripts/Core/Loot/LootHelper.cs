using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class LootHelper : MonoBehaviour
{
	struct WeaponBonuses
	{
		public WeaponBonuses(int min, int max, int crit, int haste)
		{
			MinDamage = min;
			MaxDamage = max;
			CritChance = crit;
			SkillHaste = haste;
		}
        
		public int MinDamage;
		public int MaxDamage;
		public int CritChance;
		public int SkillHaste;
	}

    [Header("Settings")]
    [SerializeField] private int _bowDropRate = 4;
    [SerializeField] private int _swordDropRate = 4;
    [SerializeField] private int _staffDropRate = 4;
    [SerializeField] private int _helmetDropRate = 4;
    [SerializeField] private int _armorDropRate = 4;
    [SerializeField] private int _bootsDropRate = 4;

    private Dictionary<string, WeaponBonuses> _weaponsTable;
    private int _enemyLevel = 1;

    // Start is called before the first frame update
    void Start()
    {
	    _enemyLevel = LevelManager.Instance.CurrentLevel;
	    _weaponsTable = new Dictionary<string, WeaponBonuses>();

        SetupLootTable();
    }

    public void RandomizeLoot()
    {
	    GameObject bowReward = RandomizeBowDrop();
	    ThrowItem(bowReward);

	    GameObject swordReward = RandomizeSwordDrop();
	    ThrowItem(swordReward);

	    GameObject staffReward = RandomizeStaffDrop();
	    ThrowItem(staffReward);

        RandomizeEquipLoot();

	    DropMoney();
    }

    public GameObject RandomizeBowDrop()
    {
	    if (Random.Range(0, 101) > _bowDropRate)
		    return null;

	    string prefabPath = "Prefabs/Rewards/Weapon Rewards/Archery Weapons/";
	    string prefabName = "";

        int bowType = Random.Range(0, 101);

        // Base Bow
        if (bowType < 25)
	        prefabName = "BaseBowReward";
        // Damage Bow
        else if (bowType < 50)
	        prefabName = "DamageBowReward";
        // Critical Bow
        else if (bowType < 75)
	        prefabName = "CriticalBowReward";
        // Haste Bow
        else
	        prefabName = "HasteBowReward";

        prefabPath += prefabName;
        GameObject bowReward = RandomizeWeapon(prefabPath, prefabName);

        return bowReward;
    }

    public GameObject RandomizeSwordDrop()
    {
	    if (Random.Range(0, 101) > _swordDropRate)
		    return null;

	    string prefabPath = "Prefabs/Rewards/Weapon Rewards/Melee Weapons/";
	    string prefabName = "";

	    int swordType = Random.Range(0, 101);

	    // Base Sword
	    if (swordType < 25)
		    prefabName = "BaseSwordReward";
        // Damage Sword
        else if (swordType < 50)
		    prefabName = "DamageSwordReward";
        // Critical BSwordow
        else if (swordType < 75)
		    prefabName = "CriticalSwordReward";
        // Haste Sword
        else
            prefabName = "HasteSwordReward";

	    prefabPath += prefabName;
	    GameObject swordReward = RandomizeWeapon(prefabPath, prefabName);

	    return swordReward;
    }

    public GameObject RandomizeStaffDrop()
    {
	    if (Random.Range(0, 101) > _staffDropRate)
		    return null;

	    string prefabPath = "Prefabs/Rewards/Weapon Rewards/Magic Weapons/";
	    string prefabName = "";

	    int staffType = Random.Range(0, 101);

	    // Base Staff
	    if (staffType < 25)
		    prefabName = "BaseStaffReward";
        // Damage Staff
        else if (staffType < 50)
		    prefabName = "DamageStaffReward";
        // Critical Staff
        else if (staffType < 75)
		    prefabName = "CriticalStaffReward";
        // Haste Staff
        else
            prefabName = "HasteStaffReward";

	    prefabPath += prefabName;
	    GameObject staffReward = RandomizeWeapon(prefabPath, prefabName);

	    return staffReward;
    }

    private GameObject RandomizeWeapon(string prefabPath, string prefabName)
    {
	    GameObject weaponPrefab = (GameObject)Resources.Load(prefabPath);
	    GameObject weapon = (GameObject)Instantiate(weaponPrefab, transform.position, Quaternion.identity);

        // Get Multipliers
        WeaponBonuses weaponBonus = _weaponsTable[prefabName];

        // Randomize The Weapon
        Color randomColor = new Color(Random.Range(0F, 1F), Random.Range(0, 1F), Random.Range(0, 1F));
		weapon.GetComponent<SpriteRenderer>().color = randomColor;
		weapon.GetComponent<WeaponReward>().SetWeaponColor(randomColor);

		int minDamage = Random.Range(_enemyLevel * 1 * weaponBonus.MinDamage, _enemyLevel * 2 * weaponBonus.MinDamage);
		int maxDamage = Random.Range(_enemyLevel * 1 * weaponBonus.MaxDamage, _enemyLevel * 1 * weaponBonus.MaxDamage);

        weapon.GetComponent<WeaponReward>().SetDamage
			(Math.Min(minDamage, maxDamage),
	         Math.Max(minDamage, maxDamage), 
			 Random.Range(_enemyLevel * 1 * weaponBonus.CritChance, _enemyLevel * 1 * weaponBonus.CritChance), 
			 Random.Range(_enemyLevel * 1 * weaponBonus.SkillHaste, _enemyLevel * 1 * weaponBonus.SkillHaste)
			 , ""
			 , "");

         weapon.GetComponent<WeaponReward>().SetWeaponData();

        // GameObject
        weapon.name = "Weapon Loot";

        return weapon;
    }

    private void RandomizeEquipLoot()
    {
        Object weaponReward = null;

        int rewardType = Random.Range(0, 100);
        bool rewardEquip = false;

        EquipType equipType = EquipType.Armor;

        // Randomize Equip Type
        if (rewardType > 10 && rewardType < 15)
        {
            weaponReward = Resources.Load("Prefabs/Rewards/ArmorReward");
            rewardEquip = true;
            equipType = EquipType.Armor;
        }
        else if (rewardType > 15 && rewardType < 20)
        {
            weaponReward = Resources.Load("Prefabs/Rewards/HelmetReward");
            rewardEquip = true;
            equipType = EquipType.Helmet;
        }
        else if (rewardType > 20 && rewardType < 25)
        {
            weaponReward = Resources.Load("Prefabs/Rewards/FootwearReward");
            rewardEquip = true;
            equipType = EquipType.Footwear;
        }

        GameObject newObj = null;

        // Create
        if (rewardEquip)
            newObj = (GameObject)Instantiate(weaponReward, transform.position, Quaternion.identity);
        else
            return;
	    
        // Randomize The Equip
        Color randomColor = new Color(Random.Range(0F, 1F), Random.Range(0, 1F), Random.Range(0, 1F));
        newObj.GetComponent<SpriteRenderer>().color = randomColor;
        newObj.GetComponent<EquipReward>().SetEquipColor(randomColor);
        newObj.GetComponent<EquipReward>().SetBonusValues(Random.Range(_enemyLevel * 1, _enemyLevel * 2), Random.Range(_enemyLevel * 1, _enemyLevel * 2), Random.Range(_enemyLevel * 1, _enemyLevel * 2));
        newObj.GetComponent<EquipReward>().SetEquipType(equipType);

        // GameObject
        newObj.name = "Loot";

        ThrowItem(newObj);
    }

    private void DropMoney()
    {
        Object moneyReward = null;

        int rewardType = Random.Range(0, 100);

        // Randomize Money
        if (rewardType > 0 && rewardType < 30)
        {
	        moneyReward = Resources.Load("Prefabs/Rewards/Coin_25G");
        }
        else if (rewardType > 30 && rewardType < 45)
        {
	        moneyReward = Resources.Load("Prefabs/Rewards/Coin_50G");
        }
        else if (rewardType > 45 && rewardType < 50)
        {
	        moneyReward = Resources.Load("Prefabs/Rewards/Coin_100G");
        }

        GameObject newObj = null;

        // Create
        if (moneyReward != null)
            newObj = (GameObject)Instantiate(moneyReward, transform.position, Quaternion.identity);
        else
            return;

        // GameObject
        newObj.name = "Loot";

        ThrowItem(newObj);
    }

    private void ThrowItem(GameObject gameObject)
    {
	    if (gameObject == null)
		    return;

	    Rigidbody2D rigidBody = gameObject.GetComponent<Rigidbody2D>();

	    float speed = (24.0f);

	    rigidBody.AddForce(new Vector2(Random.Range(-speed, speed), Random.Range(-speed, speed)), ForceMode2D.Impulse);
        rigidBody.AddTorque(Random.Range(-speed * (5.0f), speed * (5.0f)), ForceMode2D.Impulse);
    }

    private void SetupLootTable()
    {
        // Bows
	    _weaponsTable.Add("BaseBowReward", new WeaponBonuses(3,3,7,3));
	    _weaponsTable.Add("DamageBowReward", new WeaponBonuses(3, 7, 5, 1));
	    _weaponsTable.Add("CriticalBowReward", new WeaponBonuses(1, 3, 11, 1));
	    _weaponsTable.Add("HasteBowReward", new WeaponBonuses(1, 3, 5, 7));

	    // Staves
	    _weaponsTable.Add("BaseStaffReward", new WeaponBonuses(3, 3, 3, 7));
	    _weaponsTable.Add("DamageStaffReward", new WeaponBonuses(3, 7, 1, 5));
	    _weaponsTable.Add("CriticalStaffReward", new WeaponBonuses(1, 3, 7, 5));
	    _weaponsTable.Add("HasteStaffReward", new WeaponBonuses(1, 3, 1, 11));

	    // Swords
	    _weaponsTable.Add("BaseSwordReward", new WeaponBonuses(3, 7, 3, 3));
	    _weaponsTable.Add("DamageSwordReward", new WeaponBonuses(3, 11, 1, 1));
		_weaponsTable.Add("CriticalSwordReward", new WeaponBonuses(3, 5, 7, 1));
		_weaponsTable.Add("HasteSwordReward", new WeaponBonuses(3, 5, 1, 7));
    }
}
