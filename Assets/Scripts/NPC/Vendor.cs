using UnityEngine;

public class Vendor : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject _popupPanel;
    [SerializeField] private GameObject _shopPanel;

    [Header("Items")]
    [SerializeField] private VendorItem _weaponToPurchase;
    [SerializeField] private VendorItem _healthToPurchase;
    [SerializeField] private VendorItem _shieldToPurchase;

    private bool _canOpenShop;
    private EntityWeapon _entityWeapon;

    void Update()
    {
        if (_canOpenShop && Input.GetKeyDown(KeyCode.J))
        {
            _shopPanel.SetActive(true);
            _popupPanel.SetActive(false);
        }

        if (_shopPanel.activeSelf)
        {
            BuyItems();
        }
    }

    private void BuyItems()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            BuyFirstSelection();
        }
        else if (Input.GetKeyDown(KeyCode.N))
        {
            BuySecondSelection();
        }
        else if (Input.GetKeyDown(KeyCode.M))
        {
            BuyThirdSelection();
        }
    }

    public void BuySelection(int index)
    {
        if (index == 0)
        {
            BuyFirstSelection();
        }
        else if (index == 1)
        {
            BuySecondSelection();
        }
        else if (index == 2)
        {
            BuyThirdSelection();
        }
    }

    private void BuyFirstSelection()
    {
        if (CoinManager.Instance.Coins >= _weaponToPurchase.Cost)
        {
            //_entityWeapon.AddWeaponToInventory(_weaponToPurchase.WeaponToSell, _weaponToPurchase.Damage, Color.white);
            ProductBought(_weaponToPurchase.Cost);
        }
        else
            DialogManager.Instance.InstantSystemMessage("Not enough coins!");
    }

    private void BuySecondSelection()
    {
        if (CoinManager.Instance.Coins >= _shieldToPurchase.Cost)
        {
            _shieldToPurchase.ShieldToSell.AddShield(_entityWeapon.GetComponent<Entity>());
            ProductBought(_shieldToPurchase.Cost);
        }
        else
            DialogManager.Instance.InstantSystemMessage("Not enough coins!");
    }

    private void BuyThirdSelection()
    {
        if (CoinManager.Instance.Coins >= _healthToPurchase.Cost)
        {
            _healthToPurchase.HealthToSell.AddHealth(_entityWeapon.GetComponent<Entity>());
            ProductBought(_healthToPurchase.Cost);
        }
        else
            DialogManager.Instance.InstantSystemMessage("Not enough coins!");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _popupPanel.SetActive(true);
            _canOpenShop = true;

            _entityWeapon = other.GetComponent<EntityWeapon>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _entityWeapon = null;
            _canOpenShop = false;

            _popupPanel.SetActive(false);
            _shopPanel.SetActive(false);
        }
    }

    private void ProductBought(int amount)
    {
        CoinManager.Instance.RemoveCoins(amount);
    }
}
