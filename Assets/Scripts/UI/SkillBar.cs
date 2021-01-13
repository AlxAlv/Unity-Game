using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    // Magic Skills
    const string IceBoltName = "IceboltIcon";
    const string FireBoltName = "FireboltIcon";
    const string LightningBoltName = "LightningboltIcon";
    const string BoulderTossName = "BoulderTossIcon";
    const string FrozenDaggersName = "FrozenDaggersIcon";
    const string HealName = "HealIcon";

    // Archery Skills
    const string RangedAttackName = "RangedAttackIcon";
    const string ArrowRevolverName = "ArrowRevolverIcon";
    const string ChargedShotName = "ChargedShotIcon";

    // Melee Skills
    const string MeleeAttackName = "MeleeAttackIcon";

    // Shield Skills
    const string ChargeName = "ChargeIcon";

    // UI Variables
    private int currentlySelectedGroup = 0;

    Dictionary<string, float> _skillNameToResourceAmount = new Dictionary<string, float>()
    {
        { IceBoltName, IceBolt.ResourceAmount},
        { FireBoltName, FireBolt.ResourceAmount},
        { LightningBoltName, LightningBolt.ResourceAmount},
        { BoulderTossName, BoulderToss.ResourceAmount},
        { FrozenDaggersName, FrozenDaggers.ResourceAmount},
        { HealName, Heal.ResourceAmount },
        { RangedAttackName, RangedAttack.ResourceAmount},
        { ArrowRevolverName, ArrowRevolver.ResourceAmount },
        { ChargedShotName, ChargedShot.ResourceAmount},
        { MeleeAttackName, MeleeAttack.ResourceAmount},
        { ChargeName, Charge.ResourceAmount }
    };

    Dictionary<string, BaseSkill.Resource> _skillNameToResourceType = new Dictionary<string, BaseSkill.Resource>()
    {
        { IceBoltName, IceBolt.ResourceType},
        { FireBoltName, FireBolt.ResourceType},
        { LightningBoltName, LightningBolt.ResourceType},
        { BoulderTossName, BoulderToss.ResourceType},
        { FrozenDaggersName, FrozenDaggers.ResourceType},
        { HealName, Heal.ResourceType },
        { RangedAttackName, RangedAttack.ResourceType},
        { ArrowRevolverName, ArrowRevolver.ResourceType },
        { ChargedShotName, ChargedShot.ResourceType},
        { MeleeAttackName, MeleeAttack.ResourceType},
        { ChargeName, Charge.ResourceType }
    };

    Dictionary<string, WeaponType> _skillNameToType = new Dictionary<string, WeaponType>()
    {
        { IceBoltName, WeaponType.Magic },
        { FireBoltName, WeaponType.Magic },
        { LightningBoltName, WeaponType.Magic },
        { BoulderTossName, WeaponType.Magic },
        { FrozenDaggersName, WeaponType.Magic },
        { HealName, WeaponType.Magic },
        { RangedAttackName, WeaponType.Bow },
        { ArrowRevolverName, WeaponType.Bow },
        { ChargedShotName, WeaponType.Bow },
        { MeleeAttackName, WeaponType.Melee },
        { ChargeName, WeaponType.Melee }
    };

    // Start is called before the first frame update
    void Start()
    {
        _playerWeapons = Player.GetComponent<EntityWeapon>();
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
        bool RapidFire = Player.GetComponent<EntityWeapon>().RapidFire;

        if (currentlySelectedGroup == 0)
        {
            if (Input.GetMouseButtonDown(0) || (Input.GetMouseButton(0) && RapidFire))
                return G1_L_Icon;
            else if (Input.GetMouseButtonDown(1) || (Input.GetMouseButton(1) && RapidFire))
                return G1_R_Icon;
        }

        if (currentlySelectedGroup == 1)
        {
            if (Input.GetMouseButtonDown(0) || (Input.GetMouseButton(0) && RapidFire))
                return G2_L_Icon;
            else if (Input.GetMouseButtonDown(1) || (Input.GetMouseButton(1) && RapidFire))
                return G2_R_Icon;
        }

        if (currentlySelectedGroup == 2)
        {
            if (Input.GetMouseButtonDown(0) || (Input.GetMouseButton(0) && RapidFire))
                return G3_L_Icon;
            else if (Input.GetMouseButtonDown(1) || (Input.GetMouseButton(1) && RapidFire))
                return G3_R_Icon;
        }

        if (currentlySelectedGroup == 3)
        {
            if (Input.GetMouseButtonDown(0) || (Input.GetMouseButton(0) && RapidFire))
                return G4_L_Icon;
            else if (Input.GetMouseButtonDown(1) || (Input.GetMouseButton(1) && RapidFire))
                return G4_R_Icon;
        }


        if (currentlySelectedGroup == 4)
        {
            if (Input.GetMouseButtonDown(0) || (Input.GetMouseButton(0) && RapidFire))
                return G5_L_Icon;
            else if (Input.GetMouseButtonDown(1) || (Input.GetMouseButton(1) && RapidFire))
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
        else if (Player.GetComponent<EntityWeapon>().IsAnySkillOccupied() || ((!(_skillNameToType[iconPressed.sprite.name] == WeaponType.Melee)) && Player.GetComponent<EntityWeapon>().IsMeleeWeaponAndBusy()))
            return false;


        WeaponType weaponType = _skillNameToType[iconPressed.sprite.name];

        bool isWeaponFound = false;
        Weapon mainWeapon = _playerWeapons.MainWeapon;
        Weapon primaryOff = _playerWeapons.CurrentOffHand;
        Weapon secondaryWeapon = _playerWeapons.AlternateWeapon;
        Weapon alternateOff = _playerWeapons.AlternateOffHand;

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
            if ((!primaryWeaponUsed && _playerWeapons.IsMainWeaponEquipped()) || (primaryWeaponUsed && !_playerWeapons.IsMainWeaponEquipped()))
                _playerWeapons.SwitchWeapons();

            BaseSkill skillToUse = (SkillToUse(iconPressed.sprite.name, (mainHandUsed ? _playerWeapons.CurrentWeapon : _playerWeapons.CurrentOffHand)));
            _playerWeapons.CurrentWeapon.UseSkill(skillToUse);

            //Player.GetComponent<TintHelper>().SetTintColor(Color.white);

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
        if (iconName == IceBoltName)
            return new IceBolt(weapon as Staff);
        else if (iconName == FireBoltName)
            return new FireBolt(weapon as Staff);
        else if (iconName == LightningBoltName)
            return new LightningBolt(weapon as Staff);
        else if (iconName == BoulderTossName)
            return new BoulderToss(weapon as Staff);
        else if (iconName == FrozenDaggersName)
	        return new FrozenDaggers(weapon as Staff);
        else if (iconName == HealName)
            return new Heal(weapon as Staff);
        else if (iconName == RangedAttackName)
            return new RangedAttack(weapon as Bow);
        else if (iconName == ChargedShotName)
            return new ChargedShot(weapon as Bow);
        else if (iconName == ArrowRevolverName)
            return new ArrowRevolver(weapon as Bow);
        else if (iconName == MeleeAttackName)
            return new MeleeAttack(weapon as Sword);
        else if (iconName == ChargeName)
            return new Charge(weapon as Sword);

        return null;
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
            bool enoughResource = CheckResource(_skillNameToResourceAmount[G1_L_Icon.sprite.name], _skillNameToResourceType[G1_L_Icon.sprite.name]);
            if (enoughResource)
                G1_L_Icon.color = Color.white;
            else
                G1_L_Icon.color = Color.grey;
        }
        if (G1_R_Icon.sprite.name != "IconSlot")
        {
            bool enoughResource = CheckResource(_skillNameToResourceAmount[G1_R_Icon.sprite.name], _skillNameToResourceType[G1_R_Icon.sprite.name]);
            if (enoughResource)
                G1_R_Icon.color = Color.white;
            else
                G1_R_Icon.color = Color.grey;
        }
        if (G2_L_Icon.sprite.name != "IconSlot")
        {
            bool enoughResource = CheckResource(_skillNameToResourceAmount[G2_L_Icon.sprite.name], _skillNameToResourceType[G2_L_Icon.sprite.name]);
            if (enoughResource)
                G2_L_Icon.color = Color.white;
            else
                G2_L_Icon.color = Color.grey;
        }
        if (G2_R_Icon.sprite.name != "IconSlot")
        {
            bool enoughResource = CheckResource(_skillNameToResourceAmount[G2_R_Icon.sprite.name], _skillNameToResourceType[G2_R_Icon.sprite.name]);
            if (enoughResource)
                G2_R_Icon.color = Color.white;
            else
                G2_R_Icon.color = Color.grey;
        }
        if (G3_L_Icon.sprite.name != "IconSlot")
        {
            bool enoughResource = CheckResource(_skillNameToResourceAmount[G3_L_Icon.sprite.name], _skillNameToResourceType[G3_L_Icon.sprite.name]);
            if (enoughResource)
                G3_L_Icon.color = Color.white;
            else
                G3_L_Icon.color = Color.grey;
        }
        if (G3_R_Icon.sprite.name != "IconSlot")
        {
            bool enoughResource = CheckResource(_skillNameToResourceAmount[G3_R_Icon.sprite.name], _skillNameToResourceType[G3_R_Icon.sprite.name]);
            if (enoughResource)
                G3_R_Icon.color = Color.white;
            else
                G3_R_Icon.color = Color.grey;
        }
        if (G4_L_Icon.sprite.name != "IconSlot")
        {
            bool enoughResource = CheckResource(_skillNameToResourceAmount[G4_L_Icon.sprite.name], _skillNameToResourceType[G4_L_Icon.sprite.name]);
            if (enoughResource)
                G4_L_Icon.color = Color.white;
            else
                G4_L_Icon.color = Color.grey;
        }
        if (G4_R_Icon.sprite.name != "IconSlot")
        {
            bool enoughResource = CheckResource(_skillNameToResourceAmount[G4_R_Icon.sprite.name], _skillNameToResourceType[G4_R_Icon.sprite.name]);
            if (enoughResource)
                G4_R_Icon.color = Color.white;
            else
                G4_R_Icon.color = Color.grey;
        }
        if (G5_L_Icon.sprite.name != "IconSlot")
        {
            bool enoughResource = CheckResource(_skillNameToResourceAmount[G5_L_Icon.sprite.name], _skillNameToResourceType[G5_L_Icon.sprite.name]);
            if (enoughResource)
                G5_L_Icon.color = Color.white;
            else
                G5_L_Icon.color = Color.grey;
        }
        if (G5_R_Icon.sprite.name != "IconSlot")
        {
            bool enoughResource = CheckResource(_skillNameToResourceAmount[G5_R_Icon.sprite.name], _skillNameToResourceType[G5_R_Icon.sprite.name]);
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

        if (resourceType == BaseSkill.Resource.Health)
            return (resourceAmount <= healthResource.m_currentHealth);
        if (resourceType == BaseSkill.Resource.Stamina)
            return (resourceAmount <= staminaResource._currentStamina);
        if (resourceType == BaseSkill.Resource.Mana)
            return (resourceAmount <= manaResource._currentMana);

        return false;
    }

    public void SetSetBar(int index)
    {
        if (index < 4 && index >= 0)
            currentlySelectedGroup = index;
    }
}
