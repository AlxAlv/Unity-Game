using System.Collections.Generic;
using UnityEngine;

public class EntityWeapon : EntityComponent
{
    [Header("Weapon Settings")]
    [SerializeField] public Weapon MainWeapon;
    [SerializeField] public Weapon MainOffHand;
    [SerializeField] public Weapon AlternateWeapon;
    [SerializeField] public Weapon AlternateOffHand;

    [SerializeField] public List<Weapon> _ownedWeapons;
    [SerializeField] public Transform m_weaponHolderPosition;
    [SerializeField] public Transform _offHandHolderPosition;
    [SerializeField] private GameObject _armObject;
    [SerializeField] private GameObject _offArmObject;

    // Current Weapons
    public Weapon CurrentWeapon { get; set; }
    public Weapon CurrentOffHand { get; set; }

    // Current Alternate Info
    public WeaponInfo AlternateWeaponInfo;
    public WeaponInfo AlternateOffHandInfo;

    public WeaponAim m_weaponAim { get; set; }
    public WeaponAim OffHandWeaponAim { get; set; }

    public Transform WeaponHolderPosition => m_weaponHolderPosition;
    public Transform OffHandHolderPosition => _offHandHolderPosition;

    // Rapid Fire
    public bool RapidFire = false;
    private float _currentTimer = 0.0f;
    private float _doubleClickWaitTime = 0.35f;

    protected override void Start()
    {
        base.Start();

        EquipWeapon(MainWeapon, m_weaponHolderPosition);
        AddWeaponToInventory(MainWeapon, CurrentWeapon.WeaponInfo.Damage, CurrentWeapon.WeaponInfo.Color);

        if (MainOffHand != null)
        {
            EquipOffhandWeapon(MainOffHand, _offHandHolderPosition);
            AddWeaponToInventory(MainOffHand, CurrentOffHand.WeaponInfo.Damage, CurrentOffHand.WeaponInfo.Color);
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
            UpdateRapidFire();

            if ((Input.GetKeyDown(KeyCode.Tab)) && (AlternateWeapon != null))
            {
                SwitchWeapons();
            }

            if (IsTargettingSomething())
            {
                if ((Input.GetMouseButtonDown(0) || RapidFire) && (!IsAnySkillLoaded() || !_entityStunGuage.Stunned))
                {
                    // If a hotkey was pressed go for that otherwise do the normal main ability
                    if (!CurrentWeapon.SkillToUse.IsBase() && CurrentWeapon.SkillToUse.IsLoadingOrLoaded())
                        CurrentWeapon.SkillToUse.Trigger();
                }
                else if ((Input.GetMouseButtonDown(1) || RapidFire) && (!IsAnySkillLoaded() || !_entityStunGuage.Stunned))
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

    private void UpdateRapidFire()
    {
        if ((Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(0)) && IsTargettingSomething())
        {
            if (_currentTimer == 0.0f)
            {
                _currentTimer = Time.time + _doubleClickWaitTime;
                //Debug.Log("Rapid Fire - OFF - First Click");
                RapidFire = false;
            }
            else if (Time.time < _currentTimer)
            {
                //Debug.Log("Rapid Fire - ON");
                RapidFire = true;
            }
        }
        else if ((Input.GetMouseButtonUp(1) || Input.GetMouseButtonUp(0)) && RapidFire)
        {
            RapidFire = false;
            _currentTimer = 0.0f;
            //Debug.Log("Rapid Fire - OFF - Button Up");
        }
        else if (Time.time > _currentTimer)
        {
            _currentTimer = 0.0f;
        }
    }

    public bool IsMainWeaponEquipped()
    {
        return (CurrentWeapon.WeaponName == MainWeapon.WeaponName);
    }

    public void SwitchWeapons()
    {
        WeaponInfo tempInfo = CurrentWeapon.WeaponInfo;
        WeaponInfo tempInfoTwo = null;
        bool currentOffHandExists = (CurrentOffHand != null);

        if (currentOffHandExists)
            tempInfoTwo = CurrentOffHand.WeaponInfo;

        EquipWeapon(AlternateWeapon, m_weaponHolderPosition, AlternateWeaponInfo);
        EquipOffhandWeapon(AlternateOffHand, _offHandHolderPosition, AlternateOffHandInfo);
        SwapWeapons();

        AlternateWeaponInfo = tempInfo;

        if (currentOffHandExists)
            AlternateOffHandInfo = tempInfoTwo;
    }

    protected override void Update()
    {
        base.Update();
    }

    private void SwapWeapons()
    {
        Weapon temp = AlternateWeapon;
        AlternateWeapon = MainWeapon;
        MainWeapon = temp;

        Weapon tempOff = AlternateOffHand;
        AlternateOffHand = MainOffHand;
        MainOffHand = tempOff;
    }

    public void EquipWeapon(Weapon weapon, Transform weaponPosition, WeaponInfo weaponInfo = null)
    {
        if (m_controller.CanSwitchWeapons())
        {
            DestroyMainHand();

            CurrentWeapon = Instantiate(weapon, weaponPosition.position, weaponPosition.rotation);
            CurrentWeapon.transform.parent = weaponPosition;

            foreach (Transform child in _armObject.transform)
            {
                CurrentWeapon.transform.localScale = child.localScale;
                CurrentWeapon.transform.localRotation = child.localRotation;
            }

            if (weaponInfo != null)
                CurrentWeapon.SetWeaponInfo(weaponInfo);
            CurrentWeapon.SetOwner(m_entity);
            CurrentWeapon.SetArm(_armObject);
            m_weaponAim = CurrentWeapon.GetComponent<WeaponAim>();
        }
    }

    public void EquipOffhandWeapon(Weapon weapon, Transform weaponPosition, WeaponInfo weaponInfo = null)
    {
        if (m_controller.CanSwitchWeapons())
        {
            DestroyOffHand();

            if (weapon == null)
                return;

            CurrentOffHand = Instantiate(weapon, weaponPosition.position, weaponPosition.rotation);
            CurrentOffHand.transform.parent = weaponPosition;

            foreach (Transform child in _offArmObject.transform)
            {
                CurrentOffHand.transform.localScale = child.localScale;
                CurrentOffHand.transform.localRotation = child.localRotation;
            }

            CurrentOffHand.SetOwner(m_entity);
            CurrentOffHand.SetArm(_offArmObject);
            OffHandWeaponAim = CurrentOffHand.GetComponent<WeaponAim>();

            if (weaponInfo != null)
                CurrentOffHand.SetWeaponInfo(weaponInfo);
        }
    }

    public void CancelAllSkills()
    {
        if (CurrentWeapon != null)
        {
            // Cancel the main hand skills
            CurrentWeapon.CancelSkills();
        }

        // Cancel the off hand skills
        if (CurrentOffHand != null)
        {
            CurrentOffHand.CancelSkills();
        }
    }

    public bool IsAnySkillLoading()
    {
        bool isAnySkillLoading = false;

        if (CurrentWeapon != null)
            isAnySkillLoading = (CurrentWeapon.SkillToUse.IsLoading());

        if (CurrentOffHand != null)
            isAnySkillLoading |= (CurrentOffHand.SkillToUse.IsLoading());

        return isAnySkillLoading;
    }

    public bool IsAnySkillLoaded()
    {
        bool isAnySkillLoaded = false;

        if (CurrentWeapon != null)
            isAnySkillLoaded = (CurrentWeapon.SkillToUse.IsLoaded());

        if (CurrentOffHand != null)
            isAnySkillLoaded |= (CurrentOffHand.SkillToUse.IsLoaded());

        if (m_entity.EntityType == Entity.EntityTypes.AI)
            isAnySkillLoaded |= (CurrentWeapon.CurrentEnemySkill.IsLoaded());

        return isAnySkillLoaded;
    }

    public bool IsAnySkillOccupied()
    {
        bool isAnySkillLoading = false;

        if (CurrentWeapon != null)
            isAnySkillLoading = (CurrentWeapon.SkillToUse.IsLoadingOrLoaded());

        if (CurrentOffHand != null)
            isAnySkillLoading |= (CurrentOffHand.SkillToUse.IsLoadingOrLoaded());

        return isAnySkillLoading;
    }

    private void DestroyOffHand()
    {
        if (CurrentOffHand != null)
        {
            CurrentOffHand.DeletePooledObjects();
            Destroy(CurrentOffHand.gameObject);
            OffHandWeaponAim.DestroyReticle();
        }
    }

    private void DestroyMainHand()
    {
        if (CurrentWeapon != null)
        {
            m_weaponAim.DestroyReticle();
            CurrentWeapon.DeletePooledObjects();
            Destroy(CurrentWeapon.gameObject);
        }
    }

    private void DestroyAllWeapons()
    {
        AlternateWeapon = null;
        AlternateOffHand = null;

        CurrentOffHand = null;

        _ownedWeapons.Clear();
    }

    public void RemoveAllWeapons()
    {
        DestroyOffHand();
        DestroyAllWeapons();
    }

    public void AddWeaponToInventory(Weapon weaponToAdd, int damage, Color weaponColor)
    {
        if (m_entity.EntityType == Entity.EntityTypes.Player)
            GetComponent<Inventory>().AddWeaponToInventory(weaponToAdd, new WeaponInfo(damage, weaponColor));
    }

    public bool IsMeleeWeaponAndBusy()
    {
        bool isMeleeAndBusy = false;

        if (CurrentWeapon != null)
            isMeleeAndBusy = (CurrentWeapon.IsMeleeWeaponAndBusy());

        if (CurrentOffHand != null)
            isMeleeAndBusy |= (CurrentOffHand.IsMeleeWeaponAndBusy());

        return isMeleeAndBusy;
    }
}
