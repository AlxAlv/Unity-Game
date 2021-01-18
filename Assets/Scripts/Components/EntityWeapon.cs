using System.Collections.Generic;
using UnityEngine;

public class EntityWeapon : EntityComponent
{
    [Header("Weapon Settings")]
    [SerializeField] private Weapon _mainWeaponToStart;
    [SerializeField] private Weapon _mainOffHandToStart;
    [SerializeField] private Weapon _alternateWeaponToStart;
    [SerializeField] private Weapon _alternateOffHandToStart;

    [SerializeField] public List<Weapon> _ownedWeapons;

    // Current Weapons
    public Weapon CurrentWeapon { get; set; }
    public Weapon CurrentOffHandWeapon { get; set; }

    // Prefabs
    public Weapon CurrentWeaponPrefab;
    public Weapon CurrentOffHandWeaponPrefab;
    public Weapon CurrentAlternateWeapon { get; set; }
    public Weapon CurrentAlternateOffHandWeapon { get; set; }

    // Current Alternate Info
    public WeaponInfo AlternateWeaponInfo;
    public WeaponInfo AlternateOffHandInfo;

    public WeaponAim m_weaponAim { get; set; }
    public WeaponAim OffHandWeaponAim { get; set; }

    protected override void Start()
    {
        base.Start();

        if (_mainWeaponToStart != null)
        {
	        EquipWeapon(_mainWeaponToStart);
	        AddWeaponToInventory(_mainWeaponToStart, CurrentWeapon.WeaponInfo.MinDamage, CurrentWeapon.WeaponInfo.MaxDamage, CurrentWeapon.WeaponInfo.CriticalChance, CurrentWeapon.WeaponInfo.SkillHaste, CurrentWeapon.WeaponInfo.PrefixEnchant, CurrentWeapon.WeaponInfo.SuffixEnchant, CurrentWeapon.WeaponInfo.Color);
        }

        if (_mainOffHandToStart != null)
        {
	        EquipOffhandWeapon(_mainOffHandToStart);
            AddWeaponToInventory(_mainOffHandToStart, CurrentOffHandWeapon.WeaponInfo.MinDamage, CurrentOffHandWeapon.WeaponInfo.MaxDamage, CurrentOffHandWeapon.WeaponInfo.CriticalChance, CurrentOffHandWeapon.WeaponInfo.SkillHaste, CurrentOffHandWeapon.WeaponInfo.PrefixEnchant, CurrentOffHandWeapon.WeaponInfo.SuffixEnchant, CurrentOffHandWeapon.WeaponInfo.Color);
        }

        if (_alternateWeaponToStart != null)
        {
	        EquipAlternateWeapon(_alternateWeaponToStart);
	        AddWeaponToInventory(_alternateWeaponToStart, AlternateWeaponInfo.MinDamage, AlternateWeaponInfo.MaxDamage, AlternateWeaponInfo.CriticalChance, AlternateWeaponInfo.SkillHaste, AlternateWeaponInfo.PrefixEnchant, AlternateWeaponInfo.SuffixEnchant, AlternateWeaponInfo.Color);
        }

        if (_alternateOffHandToStart != null)
        {
	        EquipAlternateOffhandWeapon(_alternateOffHandToStart);
	        AddWeaponToInventory(_alternateOffHandToStart, AlternateOffHandInfo.MinDamage, AlternateOffHandInfo.MaxDamage, AlternateOffHandInfo.CriticalChance, AlternateOffHandInfo.SkillHaste, AlternateOffHandInfo.PrefixEnchant, AlternateOffHandInfo.SuffixEnchant, AlternateOffHandInfo.Color);
        }
    }

    private bool IsTargettingSomething()
    {
        GameObject enemyUnderCursor = RaycastHelper.Instance.GetEnemyUnderCursor();
        return (_entityTarget.CurrentTarget != null && (enemyUnderCursor != null) && (RaycastHelper.Instance.CheckObjectType(enemyUnderCursor) != TypeOfObject.GameObject)) || _entityTarget.IsTargettingEnemy() || (_entityTarget.PossibleTargetHelper.CurrentTarget != null);
    }

    protected override void HandleInput()
    {
        if (m_entity.EntityType == Entity.EntityTypes.Player)
        {
	        if ((Input.GetKeyDown(KeyCode.Tab)) && ((CurrentAlternateWeapon != null) || (CurrentAlternateOffHandWeapon != null)))
            {
                SwitchWeapons();
            }

            if (IsTargettingSomething())
            {
                if ((Input.GetMouseButtonDown(0)) && (!IsAnySkillLoaded() || !_entityStunGuage.Stunned))
                {
                    // If a hotkey was pressed go for that otherwise do the normal main ability
                    if (!CurrentWeapon.SkillToUse.IsBase() && CurrentWeapon.SkillToUse.IsLoadingOrLoaded())
                        CurrentWeapon.SkillToUse.Trigger();
                }
                else if ((Input.GetMouseButtonDown(1)) && (!IsAnySkillLoaded() || !_entityStunGuage.Stunned))
                {
                    // Do we have an offhand weapon?
                    if (!CurrentWeapon.SkillToUse.IsBase() && CurrentWeapon.SkillToUse.IsLoadingOrLoaded())
                        CurrentWeapon.SkillToUse.Trigger();
                }
            }
            else if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(2))
            {
                CancelAllSkills();
            }
        }
    }

    public void SwitchWeapons()
    {
        WeaponInfo tempInfo = CurrentWeapon.WeaponInfo;
        WeaponInfo tempInfoTwo = null;
        bool currentOffHandExists = (CurrentOffHandWeapon != null);

        if (currentOffHandExists)
            tempInfoTwo = CurrentOffHandWeapon.WeaponInfo;

        Weapon tempWeapon = CurrentWeaponPrefab;
        Weapon tempWeapon2 = CurrentOffHandWeaponPrefab;

        CurrentWeaponPrefab = CurrentAlternateWeapon;
        CurrentOffHandWeaponPrefab = CurrentAlternateOffHandWeapon;

        CurrentAlternateWeapon = tempWeapon;
        CurrentAlternateOffHandWeapon = tempWeapon2;

        EquipWeapon(CurrentWeaponPrefab, AlternateWeaponInfo);
        EquipOffhandWeapon(CurrentOffHandWeaponPrefab, AlternateOffHandInfo);

        AlternateWeaponInfo = tempInfo;

        if (currentOffHandExists)
            AlternateOffHandInfo = tempInfoTwo;
    }

    protected override void Update()
    {
        base.Update();
    }

    public void EquipWeapon(Weapon weapon, WeaponInfo weaponInfo = null)
    {
        if (GetComponent<EntityController>().CanSwitchWeapons())
        {
            DestroyMainHand();

            CurrentWeaponPrefab = weapon;

            CurrentWeapon = Instantiate(weapon, transform.position, transform.rotation);
            CurrentWeapon.transform.parent = transform;

            if (weaponInfo != null)
                CurrentWeapon.SetWeaponInfo(weaponInfo);

            CurrentWeapon.SetOwner((m_entity == null ? GetComponent<Entity>() : m_entity));
            m_weaponAim = CurrentWeapon.GetComponent<WeaponAim>();
        }

        if (GetComponent<WeaponsHeld>())
	        GetComponent<WeaponsHeld>().UpdateWeapons(this);
    }

    public void EquipAlternateWeapon(Weapon weapon, WeaponInfo weaponInfo = null)
    {
	    if (GetComponent<EntityController>().CanSwitchWeapons())
	    {
		    CurrentAlternateWeapon = weapon;

		    Weapon tempWeapon = Instantiate(weapon, transform.position, transform.rotation);

		    AlternateWeaponInfo = new WeaponInfo(tempWeapon.WeaponInfo.MinDamage, tempWeapon.WeaponInfo.MaxDamage, tempWeapon.WeaponInfo.CriticalChance, tempWeapon.WeaponInfo.SkillHaste, tempWeapon.WeaponInfo.PrefixEnchant, tempWeapon.WeaponInfo.SuffixEnchant, (weaponInfo == null? tempWeapon.WeaponInfo.Color : weaponInfo.Color));

		    Destroy(tempWeapon.gameObject);
        }

	    if (GetComponent<WeaponsHeld>())
		    GetComponent<WeaponsHeld>().UpdateWeapons(this);
    }

    public void EquipAlternateOffhandWeapon(Weapon weapon, WeaponInfo weaponInfo = null)
    {
	    if (GetComponent<EntityController>().CanSwitchWeapons())
	    {
		    CurrentAlternateOffHandWeapon = weapon;

		    Weapon tempWeapon = Instantiate(weapon, transform.position, transform.rotation);

		    AlternateOffHandInfo = new WeaponInfo(tempWeapon.WeaponInfo.MinDamage, tempWeapon.WeaponInfo.MaxDamage, tempWeapon.WeaponInfo.CriticalChance, tempWeapon.WeaponInfo.SkillHaste, tempWeapon.WeaponInfo.PrefixEnchant, tempWeapon.WeaponInfo.SuffixEnchant, (weaponInfo == null ? tempWeapon.WeaponInfo.Color : weaponInfo.Color));

            Destroy(tempWeapon.gameObject);
        }
    }

    public void EquipOffhandWeapon(Weapon weapon, WeaponInfo weaponInfo = null)
    {
        if (m_controller.CanSwitchWeapons())
        {
	        CurrentOffHandWeaponPrefab = weapon;

            DestroyOffHand();

            if (weapon == null)
            {
	            if (GetComponent<WeaponsHeld>())
		            GetComponent<WeaponsHeld>().UpdateWeapons(this);

                return;
            }

            CurrentOffHandWeapon = Instantiate(weapon, transform.position, transform.rotation);
            CurrentOffHandWeapon.transform.parent = transform;

            CurrentOffHandWeapon.SetOwner(m_entity);
            OffHandWeaponAim = CurrentOffHandWeapon.GetComponent<WeaponAim>();

            if (weaponInfo != null)
	            CurrentOffHandWeapon.SetWeaponInfo(weaponInfo);
        }

        if (GetComponent<WeaponsHeld>())
	        GetComponent<WeaponsHeld>().UpdateWeapons(this);
    }

    public void CancelAllSkills()
    {
        if (CurrentWeapon != null)
        {
            // Cancel the main hand skills
            CurrentWeapon.CancelSkills();
        }

        // Cancel the off hand skills
        if (CurrentOffHandWeapon != null)
        {
	        CurrentOffHandWeapon.CancelSkills();
        }
    }

    public bool IsAnySkillLoading()
    {
        bool isAnySkillLoading = false;

        if (CurrentWeapon != null)
            isAnySkillLoading = (CurrentWeapon.SkillToUse.IsLoading());

        if (CurrentOffHandWeapon != null)
            isAnySkillLoading |= (CurrentOffHandWeapon.SkillToUse.IsLoading());

        return isAnySkillLoading;
    }

    public bool IsAnySkillLoaded()
    {
        bool isAnySkillLoaded = false;

        if (CurrentWeapon != null)
            isAnySkillLoaded = (CurrentWeapon.SkillToUse.IsLoaded());

        if (CurrentOffHandWeapon != null)
            isAnySkillLoaded |= (CurrentOffHandWeapon.SkillToUse.IsLoaded());

        if (m_entity.EntityType == Entity.EntityTypes.AI)
            isAnySkillLoaded |= (CurrentWeapon.CurrentEnemySkill.IsLoaded());

        return isAnySkillLoaded;
    }

    public bool IsAnySkillOccupied()
    {
        bool isAnySkillLoading = false;

        if (CurrentWeapon != null)
            isAnySkillLoading = (CurrentWeapon.SkillToUse.IsLoadingOrLoaded());

        if (CurrentOffHandWeapon != null)
            isAnySkillLoading |= (CurrentOffHandWeapon.SkillToUse.IsLoadingOrLoaded());

        return isAnySkillLoading;
    }

    private void DestroyOffHand()
    {
        if (CurrentOffHandWeapon != null)
        {
            Destroy(CurrentOffHandWeapon.gameObject);

            CurrentOffHandWeapon = null;
        }
    }

    private void DestroyMainHand()
    {
        if (CurrentWeapon != null)
        {
            Destroy(CurrentWeapon.gameObject);

            CurrentWeapon = null;
        }
    }

    private void DestroyAllWeapons()
    {
	    CurrentAlternateWeapon = null;
        CurrentAlternateOffHandWeapon = null;
        CurrentOffHandWeapon = null;

        _ownedWeapons.Clear();
    }

    public void RemoveAllWeapons()
    {
        DestroyOffHand();
        DestroyAllWeapons();
    }

    public bool AddWeaponToInventory(Weapon weaponToAdd, int minDamage, int maxDamage, int criticalChance, int skillHaste, string prefixEnchant, string suffixEnchant, Color weaponColor)
    {
	    if (m_entity.EntityType == Entity.EntityTypes.Player)
		    return GetComponent<Inventory>().AddWeaponToInventory(weaponToAdd, new WeaponInfo(minDamage, maxDamage, criticalChance, skillHaste, prefixEnchant, suffixEnchant, weaponColor));
	    else
		    return false;
    }

    public bool IsMeleeWeaponAndBusy()
    {
        bool isMeleeAndBusy = false;

        if (CurrentWeapon != null)
            isMeleeAndBusy = (CurrentWeapon.IsMeleeWeaponAndBusy());

        if (CurrentOffHandWeapon != null)
            isMeleeAndBusy |= (CurrentOffHandWeapon.IsMeleeWeaponAndBusy());

        return isMeleeAndBusy;
    }
}
