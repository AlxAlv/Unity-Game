using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;
using System.Linq;
using Assets.Scripts.Skills.Melee;
using Assets.Scripts.Skills.Magic;
using Assets.Scripts.Skills.Archery;

public class SkillBar : MonoBehaviour
{
    // References to the skill bar
    [Header("Player Object")]
    [SerializeField] GameObject Player;
    [Header("Skill Bar Icons")]
    [SerializeField] Image G1_L_Icon;
    [SerializeField] Image G1_R_Icon;
    [SerializeField] Image G2_L_Icon;
    [SerializeField] Image G2_R_Icon;
    [SerializeField] Image G3_L_Icon;
    [SerializeField] Image G3_R_Icon;
    [SerializeField] Image G4_L_Icon;
    [SerializeField] Image G4_R_Icon;
    [SerializeField] Image G5_L_Icon;
    [SerializeField] Image G5_R_Icon;

    [SerializeField] Image GroupOneArrow;
    [SerializeField] Image GroupTwoArrow;
    [SerializeField] Image GroupThreeArrow;
    [SerializeField] Image GroupFourArrow;
    [SerializeField] Image GroupFiveArrow;

    private EntityWeapon _playerWeapons;

    private List<string> _skillNames = new List<string>();
    private List<string> _iconNames = new List<string>();
    private Dictionary<string, string> _iconNameToSkillName = new Dictionary<string, string>();
    private Dictionary<string, float> _iconNameToResourceAmount = new Dictionary<string, float>();
    private Dictionary<string, BaseSkill.Resource> _iconNameToResourceType = new Dictionary<string, BaseSkill.Resource>();
    private Dictionary<string, WeaponType> _iconNameToWeaponType = new Dictionary<string, WeaponType>();

    // Self Skills
    const string HealName = "HealIcon";

    // UI Variables
    private int currentlySelectedGroup = 0;

    // Start is called before the first frame update
    void Start()
    {
        _playerWeapons = Player.GetComponent<EntityWeapon>();

        Setup();
    }

    private void Setup()
    {
        Type[] MeleeSkills = FindMeleeSkills();
        Type[] MagicSkills = FindMagicSkills();
        Type[] ArcherySkills = FindArcherySkills();

        // Setup Melee Skill Names
        foreach (Type meleeSkill in MeleeSkills)
            _skillNames.Add(meleeSkill.Name);

        // Setup Magic Skill Names
        foreach (Type magicSkill in MagicSkills)
            _skillNames.Add(magicSkill.Name);

        // Setup Archery Skill Names
        foreach (Type archerySkill in ArcherySkills)
            _skillNames.Add(archerySkill.Name);

        // Temp Fix
        _skillNames.Remove("MeleeSkill");
        _skillNames.Remove("MagicSkill");
        _skillNames.Remove("ArcherySkill");
        _skillNames.Remove("EnemyFireBolt");
        _skillNames.Remove("EnemyIceBolt");
        _skillNames.Remove("EnemyChargedShot");
        _skillNames.Remove("EnemyRangedAttack");

        // Setup Icon Names
        foreach (string skillName in _skillNames)
        {
            string iconName = skillName + "Icon";
            _iconNames.Add(iconName);
            _iconNameToSkillName[iconName] = skillName;
        }

        // Setup Skill Info
        foreach (string iconName in _iconNames)
        {
            Type type = Type.GetType(_iconNameToSkillName[iconName]);

            if (type == null)
                Debug.LogError("Skill not found!");

            BaseSkill skill = (BaseSkill)Activator.CreateInstance(type);

            _iconNameToResourceAmount[iconName] = skill.ResourceAmount;
            _iconNameToResourceType[iconName] = skill.ResourceType;
            _iconNameToWeaponType[iconName] = skill.WeaponType;
        }
    }

    void Update()
    {
        // Attacking Skills
        if (!Player.GetComponent<EntityWeapon>().IsAnySkillOccupied() && (Player.GetComponent<EntityTarget>().PossibleTargetHelper.CurrentTarget != null || Player.GetComponent<EntityTarget>().IsTargettingEnemy() || (RaycastHelper.Instance.GetEnemyUnderCursor() && (RaycastHelper.Instance.CheckObjectType(RaycastHelper.Instance.GetEnemyUnderCursor()) != TypeOfObject.GameObject))))
        {
            Image skillImage = GetSkillImage();

            if (skillImage != null && !IsSelfSkill(skillImage.sprite.name))
                HandleKeyPress(skillImage);
        }
        // Self Skills
        else if (!Player.GetComponent<EntityWeapon>().IsAnySkillOccupied() && RaycastHelper.Instance.IsPlayerUnderCursor())
        {
            Image skillImage = GetSkillImage();

            if (skillImage != null && IsSelfSkill(skillImage.sprite.name))
            {
                HandleKeyPress(skillImage);
            }
        }

        KeyHandler();
        UpdateGuiArrows();
        UpdateImages();
    }

    private bool IsSelfSkill(string name)
    {
        bool isSelfSkill = false;
        if (name == HealName)
            isSelfSkill = true;

        return isSelfSkill;
    }

    private Image GetSkillImage()
    {
        if (currentlySelectedGroup == 0)
        {
            if (Input.GetMouseButtonDown(0))
                return G1_L_Icon;
            else if (Input.GetMouseButtonDown(1))
                return G1_R_Icon;
        }

        if (currentlySelectedGroup == 1)
        {
            if (Input.GetMouseButtonDown(0))
                return G2_L_Icon;
            else if (Input.GetMouseButtonDown(1))
                return G2_R_Icon;
        }

        if (currentlySelectedGroup == 2)
        {
            if (Input.GetMouseButtonDown(0))
                return G3_L_Icon;
            else if (Input.GetMouseButtonDown(1))
                return G3_R_Icon;
        }

        if (currentlySelectedGroup == 3)
        {
            if (Input.GetMouseButtonDown(0))
                return G4_L_Icon;
            else if (Input.GetMouseButtonDown(1))
                return G4_R_Icon;
        }


        if (currentlySelectedGroup == 4)
        {
            if (Input.GetMouseButtonDown(0))
                return G5_L_Icon;
            else if (Input.GetMouseButtonDown(1))
                return G5_R_Icon;
        }

        return null;
    }

    private void KeyHandler()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            currentlySelectedGroup--;
            if (currentlySelectedGroup < 0)
                currentlySelectedGroup = 4;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            currentlySelectedGroup++;
            if (currentlySelectedGroup > 4)
                currentlySelectedGroup = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
            currentlySelectedGroup = 0;
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            currentlySelectedGroup = 1;
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            currentlySelectedGroup = 2;
        else if (Input.GetKeyDown(KeyCode.Alpha4))
            currentlySelectedGroup = 3;
        else if (Input.GetKeyDown(KeyCode.Alpha5))
            currentlySelectedGroup = 4;
    }

    private void UpdateGuiArrows()
    {
        GroupOneArrow.enabled = (currentlySelectedGroup == 0);
        GroupTwoArrow.enabled = (currentlySelectedGroup == 1);
        GroupThreeArrow.enabled = (currentlySelectedGroup == 2);
        GroupFourArrow.enabled = (currentlySelectedGroup == 3);
        GroupFiveArrow.enabled = (currentlySelectedGroup == 4);
    }

    public bool HandleKeyPress(Image iconPressed)
    {
        if (iconPressed.sprite.name == "IconSlot")
        {
            DialogManager.Instance.InstantSystemMessage("No skill assigned");
            return false;
        }
        else if (Player.GetComponent<EntityWeapon>().IsAnySkillOccupied() || ((!(_iconNameToWeaponType[iconPressed.sprite.name] == WeaponType.Melee)) && Player.GetComponent<EntityWeapon>().IsMeleeWeaponAndBusy()))
            return false;


        WeaponType weaponType = _iconNameToWeaponType[iconPressed.sprite.name];

        bool isWeaponFound = false;
        Weapon mainWeapon = _playerWeapons.CurrentWeapon;
        Weapon primaryOff = _playerWeapons.CurrentOffHandWeapon;
        Weapon secondaryWeapon = _playerWeapons.CurrentAlternateWeapon;
        Weapon alternateOff = _playerWeapons.CurrentAlternateOffHandWeapon;

        bool primaryWeaponUsed = true;
        bool mainHandUsed = true;

        if (mainWeapon != null && CheckAgainstWeaponType(weaponType, mainWeapon))
            isWeaponFound = true;
        else if (!isWeaponFound && primaryOff != null && CheckAgainstWeaponType(weaponType, primaryOff))
        {
            isWeaponFound = true;
            mainHandUsed = false;
        }
        else if (!isWeaponFound && secondaryWeapon != null && CheckAgainstWeaponType(weaponType, secondaryWeapon))
        {
            isWeaponFound = true;
            primaryWeaponUsed = false;
        }
        else if (!isWeaponFound && alternateOff != null && CheckAgainstWeaponType(weaponType, alternateOff))
        {
            isWeaponFound = true;
            primaryWeaponUsed = false;
            mainHandUsed = false;
        }

        if (isWeaponFound)
        {
            if (!primaryWeaponUsed)
                _playerWeapons.SwitchWeapons();

            BaseSkill skillToUse = (SkillToUse(iconPressed.sprite.name, (mainHandUsed ? _playerWeapons.CurrentWeapon : _playerWeapons.CurrentOffHandWeapon)));
            _playerWeapons.CurrentWeapon.UseSkill(skillToUse);

            UIBounce.Instance.BounceUI(iconPressed.gameObject);
            FadeAwayToDeath.Instance.InitializeFadeAway(iconPressed, 1.0005f);

            return true;
        }
        else
            DialogManager.Instance.InstantSystemMessage("You don't have a " + weaponType.ToString() + " weapon");

        return false;
    }

    private bool CheckAgainstWeaponType(WeaponType type, Weapon weapon)
    {
        if (type == WeaponType.Magic)
            return weapon.IsMagicWeapon();
        else if (type == WeaponType.Bow)
            return weapon.IsBowWeapon();
        else if (type == WeaponType.Melee)
            return weapon.IsMeleeWeapon();
        else if (type == WeaponType.Shield)
            return weapon.IsShield();

        return false;
    }

    private BaseSkill SkillToUse(string iconName, Weapon weapon)
    {
        Type type = Type.GetType(_iconNameToSkillName[iconName]);

        if (type == null)
            Debug.LogError("Skill not found!");

        System.Object[] args = { weapon };

        return (BaseSkill)Activator.CreateInstance(type, args);
    }

    public void ResetSkillIcons()
    {
        G1_L_Icon.sprite = Resources.Load<Sprite>("SkillIcons/IconSlot");
        G1_R_Icon.sprite = Resources.Load<Sprite>("SkillIcons/IconSlot");
        G2_L_Icon.sprite = Resources.Load<Sprite>("SkillIcons/IconSlot");
        G2_R_Icon.sprite = Resources.Load<Sprite>("SkillIcons/IconSlot");
        G3_L_Icon.sprite = Resources.Load<Sprite>("SkillIcons/IconSlot");
        G3_R_Icon.sprite = Resources.Load<Sprite>("SkillIcons/IconSlot");
        G4_L_Icon.sprite = Resources.Load<Sprite>("SkillIcons/IconSlot");
        G4_R_Icon.sprite = Resources.Load<Sprite>("SkillIcons/IconSlot");
        G5_L_Icon.sprite = Resources.Load<Sprite>("SkillIcons/IconSlot");
        G5_R_Icon.sprite = Resources.Load<Sprite>("SkillIcons/IconSlot");
    }

    private void UpdateImages()
    {
        if (G1_L_Icon.sprite.name != "IconSlot")
        {
            bool enoughResource = CheckResource(_iconNameToResourceAmount[G1_L_Icon.sprite.name], _iconNameToResourceType[G1_L_Icon.sprite.name]);
            if (enoughResource)
                G1_L_Icon.color = Color.white;
            else
                G1_L_Icon.color = Color.grey;
        }
        if (G1_R_Icon.sprite.name != "IconSlot")
        {
            bool enoughResource = CheckResource(_iconNameToResourceAmount[G1_R_Icon.sprite.name], _iconNameToResourceType[G1_R_Icon.sprite.name]);
            if (enoughResource)
                G1_R_Icon.color = Color.white;
            else
                G1_R_Icon.color = Color.grey;
        }
        if (G2_L_Icon.sprite.name != "IconSlot")
        {
            bool enoughResource = CheckResource(_iconNameToResourceAmount[G2_L_Icon.sprite.name], _iconNameToResourceType[G2_L_Icon.sprite.name]);
            if (enoughResource)
                G2_L_Icon.color = Color.white;
            else
                G2_L_Icon.color = Color.grey;
        }
        if (G2_R_Icon.sprite.name != "IconSlot")
        {
            bool enoughResource = CheckResource(_iconNameToResourceAmount[G2_R_Icon.sprite.name], _iconNameToResourceType[G2_R_Icon.sprite.name]);
            if (enoughResource)
                G2_R_Icon.color = Color.white;
            else
                G2_R_Icon.color = Color.grey;
        }
        if (G3_L_Icon.sprite.name != "IconSlot")
        {
            bool enoughResource = CheckResource(_iconNameToResourceAmount[G3_L_Icon.sprite.name], _iconNameToResourceType[G3_L_Icon.sprite.name]);
            if (enoughResource)
                G3_L_Icon.color = Color.white;
            else
                G3_L_Icon.color = Color.grey;
        }
        if (G3_R_Icon.sprite.name != "IconSlot")
        {
            bool enoughResource = CheckResource(_iconNameToResourceAmount[G3_R_Icon.sprite.name], _iconNameToResourceType[G3_R_Icon.sprite.name]);
            if (enoughResource)
                G3_R_Icon.color = Color.white;
            else
                G3_R_Icon.color = Color.grey;
        }
        if (G4_L_Icon.sprite.name != "IconSlot")
        {
            bool enoughResource = CheckResource(_iconNameToResourceAmount[G4_L_Icon.sprite.name], _iconNameToResourceType[G4_L_Icon.sprite.name]);
            if (enoughResource)
                G4_L_Icon.color = Color.white;
            else
                G4_L_Icon.color = Color.grey;
        }
        if (G4_R_Icon.sprite.name != "IconSlot")
        {
            bool enoughResource = CheckResource(_iconNameToResourceAmount[G4_R_Icon.sprite.name], _iconNameToResourceType[G4_R_Icon.sprite.name]);
            if (enoughResource)
                G4_R_Icon.color = Color.white;
            else
                G4_R_Icon.color = Color.grey;
        }
        if (G5_L_Icon.sprite.name != "IconSlot")
        {
            bool enoughResource = CheckResource(_iconNameToResourceAmount[G5_L_Icon.sprite.name], _iconNameToResourceType[G5_L_Icon.sprite.name]);
            if (enoughResource)
                G5_L_Icon.color = Color.white;
            else
                G5_L_Icon.color = Color.grey;
        }
        if (G5_R_Icon.sprite.name != "IconSlot")
        {
            bool enoughResource = CheckResource(_iconNameToResourceAmount[G5_R_Icon.sprite.name], _iconNameToResourceType[G5_R_Icon.sprite.name]);
            if (enoughResource)
                G5_R_Icon.color = Color.white;
            else
                G5_R_Icon.color = Color.grey;
        }
    }

    private bool CheckResource(float resourceAmount, BaseSkill.Resource resourceType)
    {
        Health healthResource = Player.GetComponent<Health>();
        Stamina staminaResource = Player.GetComponent<Stamina>();
        Mana manaResource = Player.GetComponent<Mana>();
        UltimatePoints ultimateResource = Player.GetComponent<UltimatePoints>();


        if (resourceType == BaseSkill.Resource.Health)
            return (resourceAmount <= healthResource.m_currentHealth);
        if (resourceType == BaseSkill.Resource.Stamina)
            return (resourceAmount <= staminaResource._currentStamina);
        if (resourceType == BaseSkill.Resource.Mana)
            return (resourceAmount <= manaResource._currentMana);
        if (resourceType == BaseSkill.Resource.Ultimate)
            return (resourceAmount <= ultimateResource.CurrentUltimatePoints);

        return false;
    }

    public void SetSetBar(int index)
    {
        if (index < 4 && index >= 0)
            currentlySelectedGroup = index;
    }

    public Type[] FindMeleeSkills()
    {
        return (from domainAssembly in AppDomain.CurrentDomain.GetAssemblies()
                from assemblyType in domainAssembly.GetTypes()
                where typeof(MeleeSkill).IsAssignableFrom(assemblyType)
                select assemblyType).ToArray();
    }

    public Type[] FindMagicSkills()
    {
        return (from domainAssembly in AppDomain.CurrentDomain.GetAssemblies()
                from assemblyType in domainAssembly.GetTypes()
                where typeof(MagicSkill).IsAssignableFrom(assemblyType)
                select assemblyType).ToArray();
    }

    public Type[] FindArcherySkills()
    {
        return (from domainAssembly in AppDomain.CurrentDomain.GetAssemblies()
                from assemblyType in domainAssembly.GetTypes()
                where typeof(ArcherySkill).IsAssignableFrom(assemblyType)
                select assemblyType).ToArray();
    }
}
