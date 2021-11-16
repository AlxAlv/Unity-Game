using System.Collections.Generic;
using System.Linq;
using TMPro;
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
    [SerializeField] private GameObject _inventoryContainer;
    [SerializeField] private GameObject _weaponsPanel;
    [SerializeField] private GameObject _armorPanel;
    [SerializeField] private GameObject _itemsPanel;
    [SerializeField] private int _maxNumberOfItems = 15;

    [Header("Arrows")]
    [SerializeField] private GameObject _weaponsArrow;
    [SerializeField] private GameObject _armorArrow;
    [SerializeField] private GameObject _itemsArrow;

    [Header("Item Text")]
    [SerializeField] private TextMeshProUGUI _magicPowderText;
    [SerializeField] private TextMeshProUGUI _sawdustText;
    [SerializeField] private TextMeshProUGUI _jarDustText;

    public List<WeaponInstance> WeaponsOwned { get; set; }
    public List<EquipInstance> EquipsOwned { get; set; }

    private int _numberOfItems;

    // Items
    private int _currentMagicPowder = 0;
    private int _currentSawdust = 0;
    private int _currentjarDust = 0;

    static private int _currentWeaponNumber = 0;
    static private int _currentEquipNumber = 0;

    private void Awake()
    {
        _inventoryContainer = GameObject.FindWithTag("InventoryPanel");

        if (_inventoryContainer)
        {
            _weaponsPanel = _inventoryContainer.transform.Find("WeaponsContainer").gameObject;
            _armorPanel = _inventoryContainer.transform.Find("ArmorContainer").gameObject;
            _itemsPanel = _inventoryContainer.transform.Find("ItemsContainer").gameObject;

            _weaponsArrow = _inventoryContainer.transform.Find("CategoryTabs").Find("WeaponsTab").Find("Arrow").gameObject;
            _armorArrow = _inventoryContainer.transform.Find("CategoryTabs").Find("ArmorTab").Find("Arrow").gameObject;
            _itemsArrow = _inventoryContainer.transform.Find("CategoryTabs").Find("ItemsTab").Find("Arrow").gameObject;

            _magicPowderText = _itemsPanel.transform.Find("Magic Powder").Find("Text").GetComponent<TextMeshProUGUI>();
            _sawdustText = _itemsPanel.transform.Find("Sawdust").Find("Text").GetComponent<TextMeshProUGUI>();
            _jarDustText = _itemsPanel.transform.Find("Jar Dust").Find("Text").GetComponent<TextMeshProUGUI>();
        }

        _inventoryContainer.SetActive(false);
        WeaponsOwned = new List<WeaponInstance>();
        EquipsOwned = new List<EquipInstance>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
	        _inventoryContainer.SetActive(!_inventoryContainer.activeSelf);
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
	        _inventoryContainer.SetActive(false);
        }

        if (_inventoryContainer.activeSelf)
        {
            UpdateInventory();
        }
        else
        {
            _numberOfItems = 0;
        }
    }

    public bool AddWeaponToInventory(Weapon weapon, WeaponInfo weaponInfo)
    {
	    if (WeaponsOwned.Count >= _maxNumberOfItems)
        {
	        DialogManager.Instance.InstantSystemMessage("Out of weapon space!");
	        return false;
        }
        else
        {
	        weaponInfo.InternalName = weapon.name + _currentWeaponNumber.ToString();
	        _currentWeaponNumber++;

            WeaponsOwned.Add(new WeaponInstance(weapon, weaponInfo));
	        return true;
        }
    }

    public bool AddEquipToInventory(Equip equip, EquipInfo equipInfo)
    {
	    if (EquipsOwned.Count >= _maxNumberOfItems)
        {
	        DialogManager.Instance.InstantSystemMessage("Out of armor space!");
	        return false;
        }
        else
        {
	        equipInfo.InternalName = equip.equipName + _currentEquipNumber.ToString();
	        _currentEquipNumber++;

            EquipsOwned.Add(new EquipInstance(equip, equipInfo));
            return true;
        }
    }

    void UpdateInventory()
    {
        if (_weaponsPanel.activeSelf && (_numberOfItems != WeaponsOwned.Count))
        {
            foreach (Transform child in _weaponsPanel.transform)
            {
                GameObject.Destroy(child.gameObject);
            }

            AddWeapons();

            _numberOfItems = WeaponsOwned.Count;
        }
        else if (_armorPanel.activeSelf && (_numberOfItems != EquipsOwned.Count))
        {
	        foreach (Transform child in _armorPanel.transform)
	        {
		        GameObject.Destroy(child.gameObject);
	        }

	        AddEquips();

	        _numberOfItems = EquipsOwned.Count;
        }
        else if (_itemsPanel.activeSelf)
        {
	        UpdateItemsPanel();
        }
    }

    private void UpdateItemsPanel()
    {
	    _magicPowderText.text = "Magic Powder x" + _currentMagicPowder;
	    _sawdustText.text = "Sawdust x" + _currentSawdust;
        _jarDustText.text = "Jar Dust x" + _currentjarDust;
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

            uiHelper.SetText(WeaponsOwned[i].Weapon.WeaponName + "\n( " + WeaponsOwned[i].WeaponInfo.MinDamage + "~" + WeaponsOwned[i].WeaponInfo.MaxDamage + " DMG )\n" + WeaponsOwned[i].WeaponInfo.CriticalChance + "% Critical Chance\n" + WeaponsOwned[i].WeaponInfo.SkillHaste + "% Skill Haste\nRight-Click To Delete");

            Transform weaponTransform = WeaponsOwned[i].Weapon.transform;
            Transform modelTransform = weaponTransform.Find("Model");
            GameObject modelObject = modelTransform.gameObject;

            itemImage.sprite = modelObject.GetComponent<SpriteRenderer>().sprite;
            itemImage.color = WeaponsOwned[i].WeaponInfo.Color;

            itemSlot.GetComponent<RectTransform>().SetParent(_weaponsPanel.transform);
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

            itemSlot.GetComponent<RectTransform>().SetParent(_armorPanel.transform);
            itemSlot.SetActive(true);
            itemObject.name = EquipsOwned[i].EquipInfo.InternalName;

            itemScript.ItemName = EquipsOwned[i].EquipInfo.InternalName;
            itemScript.InventoryInstance = this;

            itemSlot.transform.localScale = new Vector3(.5f, .5f, 0);
            itemObject.transform.localScale = new Vector3(.9f, .9f, 0);
            itemSlot.transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0);
        }
    }

    public void ShowWeaponsTab()
    {
	    _numberOfItems = 0;

	    _weaponsPanel.SetActive(true);
	    _armorPanel.SetActive(false);
	    _itemsPanel.SetActive(false);

	    _weaponsArrow.SetActive(true);
	    _armorArrow.SetActive(false);
	    _itemsArrow.SetActive(false);
    }

    public void ShowArmorTab()
    {
	    _numberOfItems = 0;

	    _weaponsPanel.SetActive(false);
	    _armorPanel.SetActive(true);
	    _itemsPanel.SetActive(false);

	    _weaponsArrow.SetActive(false);
	    _armorArrow.SetActive(true);
	    _itemsArrow.SetActive(false);
    }

    public void ShowItemsPanel()
    {
	    _weaponsPanel.SetActive(false);
	    _armorPanel.SetActive(false);
	    _itemsPanel.SetActive(true);

        _weaponsArrow.SetActive(false);
	    _armorArrow.SetActive(false);
	    _itemsArrow.SetActive(true);
    }

    public void AddItems(ItemRewardType rewardType, int amount)
    {
	    switch (rewardType)
	    {
            case ItemRewardType.MagicPowder:
                _currentMagicPowder += amount;
	            break;

            case ItemRewardType.Sawdust:
	            _currentSawdust += amount;
	            break;

            case ItemRewardType.JarDust:
	            _currentjarDust += amount;
	            break;
	    }
	}
}
