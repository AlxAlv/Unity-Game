using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentSlot : MonoBehaviour, IDropHandler
{
    [SerializeField] private EquipmentManager _equipmentManager;

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            SoundManager.Instance.Playsound("Audio/SoundEffects/UI_Click");
            GameObject gameObject = eventData.pointerDrag.gameObject;

            string weaponName = gameObject.name;
            string containerName = this.gameObject.name;

            if (containerName == "CMContainer")
                _equipmentManager.WeaponOnEquip(weaponName, EquipmentSlots.CurrentMainWeapon);
            if (containerName == "COContainer")
                _equipmentManager.WeaponOnEquip(weaponName, EquipmentSlots.CurrentOffWeapon);
            if (containerName == "AMContainer")
                _equipmentManager.WeaponOnEquip(weaponName, EquipmentSlots.AlternateMainWeapon);
            if (containerName == "AOContainer")
                _equipmentManager.WeaponOnEquip(weaponName, EquipmentSlots.AlternateOffWeapon);
            if (containerName == "HelmetContainer")
                _equipmentManager.EquipOnEquip(weaponName, EquipmentSlots.Helmet);
            if (containerName == "ArmorContainer")
                _equipmentManager.EquipOnEquip(weaponName, EquipmentSlots.Armor);
            if (containerName == "FootwearContainer")
                _equipmentManager.EquipOnEquip(weaponName, EquipmentSlots.Footwear);
        }
    }
}
