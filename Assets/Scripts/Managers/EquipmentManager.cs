using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EquipmentSlots
{
    CurrentMainWeapon,
    CurrentOffWeapon,
    AlternateMainWeapon,
    AlternateOffWeapon,
    Helmet,
    Armor,
    Footwear
}

public class EquipmentManager : MonoBehaviour
{
    [Header("Weapon Panels")]
    [SerializeField] private GameObject _equipmentPanel;
    [SerializeField] private GameObject _currentMainContainer;
    [SerializeField] private GameObject _currentOffContainer;
    [SerializeField] private GameObject _alternateMainContainer;
    [SerializeField] private GameObject _alternateOffContainer;

    [Header("Equip Panels")]
    [SerializeField] private GameObject _helmetContainer;
    [SerializeField] private GameObject _armorContainer;
    [SerializeField] private GameObject _footwearContainer;

    // Private Members
    private Image _currentMainImage;
    private Image _currentOffImage;
    private Image _alternateMainImage;
    private Image _alternateOffImage;
    private Image _helmetImage;
    private Image _armorImage;
    private Image _footwearImage;

    // Private Components
    private EntityWeapon _entityWeapon;
    private EntityEquips _entityEquips;

    // Start is called before the first frame update
    private void Start()
    {
        _equipmentPanel.SetActive(false);

        _entityWeapon = GetComponent<EntityWeapon>();
        _entityEquips = GetComponent<EntityEquips>();

        // Weapons
        _currentMainImage = _currentMainContainer.GetComponent<Image>();
        _currentOffImage = _currentOffContainer.GetComponent<Image>();
        _alternateMainImage = _alternateMainContainer.GetComponent<Image>();
        _alternateOffImage = _alternateOffContainer.GetComponent<Image>();

        // Equips
        _helmetImage = _helmetContainer.GetComponent<Image>();
        _armorImage = _armorContainer.GetComponent<Image>();
        _footwearImage = _footwearContainer.GetComponent<Image>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            SetActive(!_equipmentPanel.activeSelf);
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            SetActive(false);
        }

        if (_equipmentPanel.activeSelf)
        {
            UpdateEquipment();
        }
    }

    private void UpdateEquipment()
    {
        if (_entityWeapon.CurrentWeapon != null)
        {
            WeaponInstance currentMain = new WeaponInstance(_entityWeapon.CurrentWeapon, _entityWeapon.CurrentWeapon.WeaponInfo);
            UpdateImage(currentMain, _currentMainImage);
        }
        else
            _currentMainImage.sprite = Resources.Load<Sprite>("Sprites/UI/Equipment/Weapon_Icon");

        if (_entityWeapon.CurrentOffHandWeapon != null)
        {
            WeaponInstance currentOff = new WeaponInstance(_entityWeapon.CurrentOffHandWeapon, _entityWeapon.CurrentOffHandWeapon.WeaponInfo);
            UpdateImage(currentOff, _currentOffImage);
        }
        else
            _currentOffImage.sprite = Resources.Load<Sprite>("Sprites/UI/Equipment/Reversed_Weapon_Icon");

        if (_entityWeapon.CurrentAlternateWeapon != null)
        {
            WeaponInstance alternateMain = new WeaponInstance(_entityWeapon.CurrentAlternateWeapon, _entityWeapon.AlternateWeaponInfo);
            UpdateImage(alternateMain, _alternateMainImage);
        }
        else
            _alternateMainImage.sprite = Resources.Load<Sprite>("Sprites/UI/Equipment/Weapon_Icon");

        if (_entityWeapon.CurrentAlternateOffHandWeapon != null)
        {
            WeaponInstance alternateOff = new WeaponInstance(_entityWeapon.CurrentAlternateOffHandWeapon, _entityWeapon.AlternateOffHandInfo);
            UpdateImage(alternateOff, _alternateOffImage);
        }
        else
            _alternateOffImage.sprite = Resources.Load<Sprite>("Sprites/UI/Equipment/Reversed_Weapon_Icon");

        if (_entityEquips.CurrentHelmet != null)
        {
            EquipInstance helm = new EquipInstance(_entityEquips.CurrentHelmet, _entityEquips.HelmetInfo);
            UpdateEquipImage(helm, _helmetImage);
        }
        else
            _helmetImage.sprite = Resources.Load<Sprite>("Sprites/UI/Equipment/Headgear_Icon");

        if (_entityEquips.CurrentArmor != null)
        {
            EquipInstance armor = new EquipInstance(_entityEquips.CurrentArmor, _entityEquips.ArmorInfo);
            UpdateEquipImage(armor, _armorImage);
        }
        else
            _armorImage.sprite = Resources.Load<Sprite>("Sprites/UI/Equipment/Armor_Icon");

        if (_entityEquips.CurrentFootwear != null)
        {
            EquipInstance footwear = new EquipInstance(_entityEquips.CurrentFootwear, _entityEquips.FootwearInfo);
            UpdateEquipImage(footwear, _footwearImage);
        }
        else
            _footwearImage.sprite = Resources.Load<Sprite>("Sprites/UI/Equipment/Footwear_Icon");
    }

    private void UpdateImage(WeaponInstance weapon, Image image)
    {
        Transform weaponTransform = weapon.Weapon.transform;
        Transform modelTransform = weaponTransform.Find("Model");
        GameObject modelObject = modelTransform.gameObject;

        image.sprite = modelObject.GetComponent<SpriteRenderer>().sprite;
        image.color = weapon.WeaponInfo.Color;
		image.gameObject.GetComponent<TooltipUIHelper>().SetText(weapon.Weapon.PrefixEnchant + " " + weapon.Weapon.WeaponName + " " + weapon.Weapon.SuffixEnchant + "\n( " + weapon.WeaponInfo.MinDamage + "~" + weapon.WeaponInfo.MaxDamage + " DMG )\n" + weapon.WeaponInfo.CriticalChance + "% Critical Chance\n" + weapon.WeaponInfo.SkillHaste + "% Skill Haste");
    }

    private void UpdateEquipImage(EquipInstance equip, Image image)
    {
        GameObject modelObject = equip.Equip.transform.gameObject;

        image.sprite = modelObject.GetComponent<SpriteRenderer>().sprite;
        image.color = equip.EquipInfo.Color;
        image.gameObject.GetComponent<TooltipUIHelper>().SetText(equip.Equip.equipName + " ( " + equip.EquipInfo.StrBonus + " STR)" + " ( " + equip.EquipInfo.IntBonus + " INT)" + " ( " + equip.EquipInfo.DexBonus + " DEX)");
    }

    private void SetActive(bool isActive)
    {
        // Weapons
        _equipmentPanel.SetActive(isActive);
        _currentMainContainer.SetActive(isActive);
        _currentOffContainer.SetActive(isActive);
        _alternateMainContainer.SetActive(isActive);
        _alternateOffContainer.SetActive(isActive);

        // Equips
        _helmetContainer.SetActive(isActive);
        _armorContainer.SetActive(isActive);
        _footwearContainer.SetActive(isActive);
    }

    public void WeaponOnEquip(string nameOfWeapon, EquipmentSlots weaponSlot) 
    {
        foreach (var weapon in GetComponent<Inventory>().WeaponsOwned)
        {
            if (weapon.WeaponInfo.InternalName == nameOfWeapon)
            {
                EntityEquipWeapon(weapon, weaponSlot);
            }
        }
    }

    public void EquipOnEquip(string nameOfEquip, EquipmentSlots equipSlot)
    {
        foreach (var equip in GetComponent<Inventory>().EquipsOwned)
        {
            if (equip.EquipInfo.InternalName == nameOfEquip)
            {
                EntityEquip(equip, equipSlot);
            }
        }
    }

    private void EntityEquip(EquipInstance equip, EquipmentSlots equipSlot)
    {
        equip.Equip.SetBonuses(equip.EquipInfo.StrBonus, equip.EquipInfo.IntBonus, equip.EquipInfo.DexBonus);

        if (equipSlot == EquipmentSlots.Helmet && equip.EquipInfo.EquipType == EquipType.Helmet)
        {
            _entityEquips.EquipHelmet(equip.Equip, equip.EquipInfo);
        }
        else if (equipSlot == EquipmentSlots.Armor && equip.EquipInfo.EquipType == EquipType.Armor)
        {
            _entityEquips.EquipArmor(equip.Equip, equip.EquipInfo);
        }
        else if (equipSlot == EquipmentSlots.Footwear && equip.EquipInfo.EquipType == EquipType.Footwear)
        {
            _entityEquips.EquipFootwear(equip.Equip, equip.EquipInfo);
        }
    }

    private void EntityEquipWeapon(WeaponInstance weapon, EquipmentSlots weaponSlot)
    {
        if (weaponSlot == EquipmentSlots.CurrentMainWeapon)
        {
            _entityWeapon.EquipWeapon(weapon.Weapon, weapon.WeaponInfo);
        }
        else if (weaponSlot == EquipmentSlots.AlternateMainWeapon)
        {
            _entityWeapon.EquipAlternateWeapon(weapon.Weapon, weapon.WeaponInfo);
        }
        else if (weaponSlot == EquipmentSlots.CurrentOffWeapon)
        {
            _entityWeapon.EquipOffhandWeapon(weapon.Weapon, weapon.WeaponInfo);
        }
        else if (weaponSlot == EquipmentSlots.AlternateOffWeapon)
        {
	        _entityWeapon.EquipAlternateOffhandWeapon(weapon.Weapon, weapon.WeaponInfo);
        }
    }
}
