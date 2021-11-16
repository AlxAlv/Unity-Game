using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Assets.Scripts.Skills.Melee;
using Assets.Scripts.Skills.Magic;
using Assets.Scripts.Skills.Archery;
using TMPro;
using UnityEngine.UI;

public class SkillPanelManager : MonoBehaviour
{
    [Header("Tabs")]
    [SerializeField] private GameObject _archeryTabObject;
    [SerializeField] private GameObject _meleeTabObject;
    [SerializeField] private GameObject _magicTabObject;

    [Header("Skill Panel Object")]
    [SerializeField] GameObject _skillPanelObject;

    private List<string> _skillNames = new List<string>();

    // Start is called before the first frame update
    void Awake()
    {
        GameObject skillPanelObject = GameObject.FindWithTag("SkillPanel");
        Transform skillPanel = skillPanelObject.transform;

        if (skillPanel)
        {
            _archeryTabObject = skillPanel.Find("RangedTab").Find("Ranged Skills").gameObject;
            _meleeTabObject = skillPanel.Find("MeleeTab").Find("Melee Skills").gameObject;
            _magicTabObject = skillPanel.Find("MagicTab").Find("Magic Skills").gameObject;
        }

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
            Type type = Type.GetType(skillName);

            if (type == null)
                Debug.LogError("Skill not found!");

            BaseSkill skill = (BaseSkill)Activator.CreateInstance(type);

            Sprite iconSprite = Resources.Load<Sprite>(skill.GetSkillIconPath());

            WeaponType weaponType = skill.WeaponType;

            string toolTipText = skill.ToolTipInfo;

            CreateSkill(skillName, iconSprite, weaponType, toolTipText);
        }
    }

    private void CreateSkill(string skillName, Sprite iconSprite, WeaponType weaponType, string toolTipText)
    {
        GameObject newObject = Instantiate(_skillPanelObject);
        GameObject nameObject = null;
        GameObject iconObject = null;

        foreach (Transform child in newObject.transform)
        {
            if (child.name == "Name")
                nameObject = child.gameObject;

            if (child.name == "Icon")
                iconObject = child.gameObject;
        }

        nameObject.GetComponent<TextMeshProUGUI>().text = skillName;
        iconObject.GetComponent<Image>().sprite = iconSprite;
        iconObject.GetComponent<TooltipUIHelper>().SetText(toolTipText);

        switch (weaponType)
        {
            case WeaponType.Bow:
                newObject.transform.parent = _archeryTabObject.transform;
                break;
            case WeaponType.Magic:
                newObject.transform.parent = _magicTabObject.transform;
                break;
            case WeaponType.Melee:
                newObject.transform.parent = _meleeTabObject.transform;
                break;
        }

        newObject.transform.localScale = Vector3.one;
        newObject.transform.localPosition = Vector3.zero;
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
