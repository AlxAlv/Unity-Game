using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public struct WeaponInstance
{
    public Weapon Weapon;
    public WeaponInfo WeaponInfo;

    public WeaponInstance(Weapon weapon, WeaponInfo weaponInfo)
    {
        Weapon = weapon;
        WeaponInfo = weaponInfo;
    }
}

public struct EquipInstance
{
    public Equip Equip;
    public EquipInfo EquipInfo;

    public EquipInstance(Equip equip, EquipInfo equipInfo)
    {
        Equip = equip;
        EquipInfo = equipInfo;
    }
}

public class Inventory : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject _inventoryPanel;
    [SerializeField] private GameObject _inventoryContainer;
    [SerializeField] private int _maxNumberOfEquips = 15;

    public List<WeaponInstance> WeaponsOwned { get; set; }
    public List<EquipInstance> EquipsOwned { get; set; }

    private int _numberOfItems;

    static private int _currentWeaponNumber = 0;
    static private int _currentEquipNumber = 0;

    private void Awake()
    {
        _inventoryPanel.SetActive(false);
        WeaponsOwned = new List<WeaponInstance>();
        EquipsOwned = new List<EquipInstance>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            _inventoryPanel.SetActive(!_inventoryPanel.activeSelf);
            _inventoryContainer.SetActive(_inventoryPanel.activeSelf);
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            _inventoryPanel.SetActive(false);
            _inventoryContainer.SetActive(false);

        }

        if (_inventoryPanel.activeSelf)
        {
            UpdateInventory();
        }
        else
        {
            _numberOfItems = 0;
        }
    }

    public void AddWeaponToInventory(Weapon weapon, WeaponInfo weaponInfo)
    {
        weaponInfo.InternalName = weapon.name + _currentWeaponNumber.ToString();
        _currentWeaponNumber++;

        if (WeaponsOwned.Count + EquipsOwned.Count >= _maxNumberOfEquips)
            DialogManager.Instance.InstantSystemMessage("Out of inventory space!");
        else
            WeaponsOwned.Add(new WeaponInstance(weapon, weaponInfo));
    }

    public void AddEquipToInventory(Equip equip, EquipInfo equipInfo)
    {
        equipInfo.InternalName = equip.equipName + _currentEquipNumber.ToString();
        _currentEquipNumber++;

        if (WeaponsOwned.Count + EquipsOwned.Count >= _maxNumberOfEquips)
            DialogManager.Instance.InstantSystemMessage("Out of inventory space!");
        else
            EquipsOwned.Add(new EquipInstance(equip, equipInfo));
    }

    void UpdateInventory()
    {
        if (_numberOfItems != (WeaponsOwned.Count + EquipsOwned.Count))
        {
            foreach (Transform child in _inventoryContainer.transform)
            {
                GameObject.Destroy(child.gameObject);
            }

            AddWeapons();
            AddEquips();

            _numberOfItems = WeaponsOwned.Count + EquipsOwned.Count;
        }
    }

    public void RemoveFromInventory(string internalName)
    {
        // Look through Weapons
        foreach (WeaponInstance weapon in WeaponsOwned.ToList())
        {
            if (weapon.WeaponInfo.InternalName == internalName)
            {
                DialogManager.Instance.InstantSystemMessage("Deleted " + weapon.Weapon.WeaponName);
                WeaponsOwned.Remove(weapon);
            }
        }

        // Look through Equips
        foreach (EquipInstance equip in EquipsOwned.ToList())
        {
            if (equip.EquipInfo.InternalName == internalName)
            {
                DialogManager.Instance.InstantSystemMessage("Deleted " + equip.Equip.equipName);
                EquipsOwned.Remove(equip);
            }
        }
    }

    // Show Weapons On GUI
    private void AddWeapons()
    {
        for (int i = 0; i < WeaponsOwned.Count; ++i)
        {
            GameObject slotPrefab = (GameObject)Resources.Load("Prefabs/Inventory/ItemSlot");

            GameObject itemSlot = Instantiate(slotPrefab);
            GameObject itemObject = null;

            foreach (Transform child in itemSlot.transform)
            {
                if (child.name == "Item")
                {
                    itemObject = child.gameObject;
                }
            }

            Image itemImage = itemObject.GetComponent<Image>();
            TooltipUIHelper uiHelper = itemObject.GetComponent<TooltipUIHelper>();
            InventoryItem itemScript = itemObject.GetComponent<InventoryItem>();

            uiHelper.SetText(WeaponsOwned[i].Weapon.WeaponName + " ( " + WeaponsOwned[i].WeaponInfo.Damage + " DMG )\nRight-Click To Delete");

            Transform weaponTransform = WeaponsOwned[i].Weapon.transform;
            Transform modelTransform = weaponTransform.Find("Model");
            GameObject modelObject = modelTransform.gameObject;

            itemImage.sprite = modelObject.GetComponent<SpriteRenderer>().sprite;
            itemImage.color = WeaponsOwned[i].WeaponInfo.Color;

            itemSlot.GetComponent<RectTransform>().SetParent(_inventoryContainer.transform);
            itemSlot.SetActive(true);
            itemObject.name = WeaponsOwned[i].WeaponInfo.InternalName;

            itemScript.ItemName = WeaponsOwned[i].WeaponInfo.InternalName;
            itemScript.InventoryInstance = this;

            itemSlot.transform.localScale = new Vector3(.5f, .5f, 0);
            itemObject.transform.localScale = new Vector3(.9f, .9f, 0);
            itemSlot.transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0);
        }
    }

    private void AddEquips()
    {
        for (int i = 0; i < EquipsOwned.Count; ++i)
        {
            GameObject slotPrefab = (GameObject)Resources.Load("Prefabs/Inventory/ItemSlot");

            GameObject itemSlot = Instantiate(slotPrefab);
            GameObject itemObject = null;

            foreach (Transform child in itemSlot.transform)
            {
                if (child.name == "Item")
                {
                    itemObject = child.gameObject;
                }
            }

            Image itemImage = itemObject.GetComponent<Image>();
            TooltipUIHelper uiHelper = itemObject.GetComponent<TooltipUIHelper>();
            InventoryItem itemScript = itemObject.GetComponent<InventoryItem>();

            uiHelper.SetText(EquipsOwned[i].Equip.equipName + " ( " + EquipsOwned[i].EquipInfo.StrBonus + " STR)" + " ( " + EquipsOwned[i].EquipInfo.IntBonus + " INT)" + " ( " + EquipsOwned[i].EquipInfo.DexBonus + " DEX)" + "\nRight-Click To Delete");

            GameObject modelObject = EquipsOwned[i].Equip.transform.gameObject;

            itemImage.sprite = modelObject.GetComponent<SpriteRenderer>().sprite;
            itemImage.color = EquipsOwned[i].EquipInfo.Color;

            itemSlot.GetComponent<RectTransform>().SetParent(_inventoryContainer.transform);
            itemSlot.SetActive(true);
            itemObject.name = EquipsOwned[i].EquipInfo.InternalName;

            itemScript.ItemName = EquipsOwned[i].EquipInfo.InternalName;
            itemScript.InventoryInstance = this;

            itemSlot.transform.localScale = new Vector3(.5f, .5f, 0);
            itemObject.transform.localScale = new Vector3(.9f, .9f, 0);
            itemSlot.transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0);
        }
    }
}
