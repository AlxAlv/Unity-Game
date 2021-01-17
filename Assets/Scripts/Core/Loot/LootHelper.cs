using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootHelper : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] public int EnemyLevel = 1;

    // Start is called before the first frame update
    void Start()
    {
	    EnemyLevel = LevelManager.Instance.CurrentLevel;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RandomizeLoot()
    {
	    RandomizeEquipLoot();
	    DropMoney();
    }

    private void RandomizeEquipLoot()
    {
        Object weaponReward = null;

        int rewardType = Random.Range(0, 100);

        bool rewardWeapon = false;
        bool rewardEquip = false;

        EquipType equipType = EquipType.Armor;

        // Randomize Weapon Type
        if (rewardType > 0 && rewardType < 3)
        {
            weaponReward = Resources.Load("Prefabs/Rewards/SwordReward");
            rewardWeapon = true;
        }
        if (rewardType > 25 && rewardType < 35)
        {
            weaponReward = Resources.Load("Prefabs/Rewards/BowReward");
            rewardWeapon = true;
        }
        else if (rewardType > 6 && rewardType < 9)
        {
            weaponReward = Resources.Load("Prefabs/Rewards/StaffReward");
            rewardWeapon = true;
        }

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
        if (rewardWeapon || rewardEquip)
            newObj = (GameObject)Instantiate(weaponReward, transform.position, Quaternion.identity);
        else
            return;

        if (rewardWeapon)
        {
            // Randomize The Weapon
            Color randomColor = new Color(Random.Range(0F, 1F), Random.Range(0, 1F), Random.Range(0, 1F));
            newObj.GetComponent<SpriteRenderer>().color = randomColor;
            newObj.GetComponent<WeaponReward>().SetWeaponColor(randomColor);
            newObj.GetComponent<WeaponReward>().SetDamage(Random.Range(EnemyLevel * 1, EnemyLevel * 3));
        }
        else if (rewardEquip)
        {
            // Randomize The Equip
            Color randomColor = new Color(Random.Range(0F, 1F), Random.Range(0, 1F), Random.Range(0, 1F));
            newObj.GetComponent<SpriteRenderer>().color = randomColor;
            newObj.GetComponent<EquipReward>().SetEquipColor(randomColor);
            newObj.GetComponent<EquipReward>().SetBonusValues(Random.Range(EnemyLevel * 1, EnemyLevel * 2), Random.Range(EnemyLevel * 1, EnemyLevel * 2), Random.Range(EnemyLevel * 1, EnemyLevel * 2));
            newObj.GetComponent<EquipReward>().SetEquipType(equipType);
        }

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
	    Rigidbody2D rigidBody = gameObject.GetComponent<Rigidbody2D>();

	    float speed = (24.0f);

	    rigidBody.AddForce(new Vector2(Random.Range(-speed, speed), Random.Range(-speed, speed)), ForceMode2D.Impulse);
        rigidBody.AddTorque(Random.Range(-speed * (5.0f), speed * (5.0f)), ForceMode2D.Impulse);
    }
}
