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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RandomizeLoot()
    {
        Object weaponReward = null;

        int rewardType = Random.Range(0, 100);

        bool rewardWeapon = false;
        bool rewardEquip = false;

        EquipType equipType = EquipType.Armor;

        // Randomize Weapon Type
        if (rewardType > 0 && rewardType < 100)
        {
            weaponReward = Resources.Load("Prefabs/Rewards/SwordReward");
            rewardWeapon = true;
        }
        //if (rewardType > 0 && rewardType < 10)
        //{
        //    weaponReward = Resources.Load("Prefabs/Rewards/BowReward");
        //    rewardWeapon = true;
        //}
        //else if (rewardType > 10 && rewardType < 20)
        //{
        //    weaponReward = Resources.Load("Prefabs/Rewards/StaffReward");
        //    rewardWeapon = true;
        //}

        //// Randomize Equip Type
        //if (rewardType > 20 && rewardType < 30)
        //{
        //    weaponReward = Resources.Load("Prefabs/Rewards/ArmorReward");
        //    rewardEquip = true;
        //    equipType = EquipType.Armor;
        //}
        //else if (rewardType > 30 && rewardType < 40)
        //{
        //    weaponReward = Resources.Load("Prefabs/Rewards/HelmetReward");
        //    rewardEquip = true;
        //    equipType = EquipType.Helmet;
        //}
        //else if (rewardType > 40 && rewardType < 50)
        //{
        //    weaponReward = Resources.Load("Prefabs/Rewards/FootwearReward");
        //    rewardEquip = true;
        //    equipType = EquipType.Footwear;
        //}

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
            newObj.GetComponent<WeaponReward>().SetDamage(Random.Range(EnemyLevel * 1, EnemyLevel * 10));
        }
        else if (rewardEquip)
        {
            // Randomize The Equip
            Color randomColor = new Color(Random.Range(0F, 1F), Random.Range(0, 1F), Random.Range(0, 1F));
            newObj.GetComponent<SpriteRenderer>().color = randomColor;
            newObj.GetComponent<EquipReward>().SetEquipColor(randomColor);
            newObj.GetComponent<EquipReward>().SetBonusValues(Random.Range(EnemyLevel * 1, EnemyLevel * 3), Random.Range(EnemyLevel * 1, EnemyLevel * 3), Random.Range(EnemyLevel * 1, EnemyLevel * 3));
            newObj.GetComponent<EquipReward>().SetEquipType(equipType);
        }

        // GameObject
        newObj.name = "Loot";
    }
}
