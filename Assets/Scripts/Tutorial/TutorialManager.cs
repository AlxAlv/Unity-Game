using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [Header("Tutorial Settings")]
    [SerializeField] public Transform StartPosition;
    [SerializeField] public GameObject MeleeSkills;
    [SerializeField] public GameObject MagicSkills;
    [SerializeField] public SkillBar SkillBar;
    [SerializeField] public GameObject Player;
    [SerializeField] public bool SkipRoomOne;
    [SerializeField] public Transform SpawnPosition;

    [Header("Room One Settings")]
    [SerializeField] public Transform HallWayEntrancePosition;
    [SerializeField] public Transform RoomOneEntrance;
    [SerializeField] public GameObject RoomOneHealthReward;
    [SerializeField] public GameObject RoomOneCoinReward;
    [SerializeField] public Transform NextHallwayEntrancePosition;
    [SerializeField] public Transform BlockedPathPosition;
    [SerializeField] public Transform SecondHalfOfHallway;

    [Header("Hallway One Settings")]
    [SerializeField] public GameObject HallwayOneJar3;
    [SerializeField] public GameObject HallwayOneJar2;
    [SerializeField] public GameObject HallwayOneJar1;
    [SerializeField] public GameObject SkillWindow;
    [SerializeField] public Image LeftMostSkill;
    [SerializeField] public Transform RoomTwoEntrance;

    [Header("Room Two Settings")]
    [SerializeField] public GameObject RoomTwoEnemy;
    [SerializeField] public Transform RoomTwoPosition;
    [SerializeField] public GameObject RoomTwoReward;
    [SerializeField] public GameObject InventoryPanel;
    [SerializeField] public GameObject CharacterPanel;

    private bool _tutorialStarted = false;

    // Complete Room Flags
    private bool IsRoomOneDone = false;

    // Room 1 Flags
    private bool One_HasMovedToEntrance = false;
    private bool One_HasMovedToNextRoom = false;
    private bool One_HasCollectedCollectables = false;
    private bool One_HasReachedBlockedPath = false;

    // Hallway One Flags
    private bool One_HasOpenedSkillsWindow = false;
    private bool One_HasEquippedSkill = false;
    private bool One_HasClosedSkillsWindow = false;
    private bool One_HasSwitchedSkillSet = false;
    private bool One_HasLoadedSkill = false;
    private bool One_HasUsedSkill = false;
    private bool One_HasDestroyedJars = false;
    private bool One_IsMovingToRoomTwo = false;
    private bool One_ChangedZoomed = false;

    // Room Two Flags
    private bool Two_HasStarted = false;
    private bool FinishedDialogue = false;
    private int RoomTwoDialogIndex = 0;
    private List<string> RoomTwoDialogList = new List<string> 
    {"This is an enemy!\n[SPACE BAR]",
     "Different level enemies are different colors\n[SPACE BAR]",
    };
    private bool Two_IsEnemyTargetted = false;
    private bool Two_SkillIsCancelled = false;
    private bool Two_IsEnemyTargettedWithAutoTarget = false;
    private bool Two_IsEnemyDamaged = false;
    private bool Two_EnemyIsStale = false;
    private bool Two_NoLongerStale = false;
    private bool Two_KnockedBack = false;
    private float Two_CurrentEnemyHealth;
    private List<string> RoomTwoKBDialogList = new List<string>
    {"If attacked while knocked back -\nAn enemy will be immune to stun\n[SPACE BAR]"};
    private bool Two_KnockBackInfo = false;
    private bool Two_EnemyIsInvincible = false;
    private bool Two_EnemyDead = false;
    private bool Two_CharacterWindowOpened = false;
    private bool Two_SkillPointUsed = false;
    private bool Two_CharacterWindowClosed = false;
    private bool Two_RewardPickedUp  = false;
    private bool Two_InventoryOpened = false;
    private bool Two_InventoryClosed = false;
    private bool Two_SwitchedWeapons = false;
    private bool Two_MagicOpened = false;
    private bool Two_MagicClosed = false;


    // Start is called before the first frame update
    void Start()
    {
        DialogManager.Instance.AddSystemMessage("June 16th Build\nAdded Equipment Drops And Fixed AI Pathing\nPress [F1] For Help");
    }

    // Update is called once per frame
    void Update()
    {
        //HandleKeyPress();
        //TutorialHandler();
        HandleHelpText();
    }

    private void  TutorialHandler()
    {
        if (_tutorialStarted)
        {
            if (!IsRoomOneDone)
                RoomOneHandler();
        }
    }

    void HandleHelpText()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            DialogManager.Instance.AddSystemMessage("Controls\n---------------------\n[S] To Target Enemy]\n[A] [D] To Change Skills\n[F] Interact With Dungeon Entry\n[Mouse Buttons] Move And Attack");
        }
    }

    void HandleKeyPress()
    {
        if (Input.GetKeyDown(KeyCode.F12) && !_tutorialStarted)
        {
            _tutorialStarted = true;

            if (SkipRoomOne)
            {
                SetRoomOneFlagsToDone();
                StartPosition = RoomTwoEntrance;
            }
            // Remove All UI Stuff
            SkillBar.ResetSkillIcons();

            // Hold the DialogManager Hostage
            DialogManager.Instance.HeldHostage = true;

            // Remove All Player Stuff
            Player.GetComponent<EntityWeapon>().RemoveAllWeapons();
            Player.transform.position = StartPosition.position;

            MeleeSkills.SetActive(false);
            MagicSkills.SetActive(false);
        }
    }

    private void RoomOneHandler()
    {
        if (!One_HasMovedToEntrance)
        {
            DialogManager.Instance.SetText("Using the Left Mouse Button\nMove to the hallway entrance");

            if (Player.transform.position.x > (HallWayEntrancePosition.localPosition.x + HallWayEntrancePosition.parent.localPosition.x))
            {
                One_HasMovedToEntrance = true;
                SoundManager.Instance.Playsound("Audio/SoundEffects/SuccessFx");
            }
        }
        else if (!One_HasMovedToNextRoom)
        {
            DialogManager.Instance.SetText("Move down the hallway.\nYou can run by holding down Left-Shift.");

            if (Player.transform.position.x > (RoomOneEntrance.localPosition.x + RoomOneEntrance.parent.localPosition.x))
            {
                One_HasMovedToNextRoom = true;
                SoundManager.Instance.Playsound("Audio/SoundEffects/SuccessFx");
            }
        }
        else if (!One_HasCollectedCollectables)
        {
            DialogManager.Instance.SetText("Pick up the rewards or continue right!");

            if ((RoomOneHealthReward == null && RoomOneCoinReward == null) || (Player.transform.position.x > (NextHallwayEntrancePosition.localPosition.x + NextHallwayEntrancePosition.parent.localPosition.x)))
            {
                One_HasCollectedCollectables = true;
                SoundManager.Instance.Playsound("Audio/SoundEffects/SuccessFx");
            }
        }
        else if (!One_HasReachedBlockedPath)
        {
            DialogManager.Instance.SetText("Move down the hallway to the right!\nRemember, run using Left-Shift");

            if (Player.transform.position.x > (BlockedPathPosition.localPosition.x + BlockedPathPosition.parent.localPosition.x))
            {
                One_HasReachedBlockedPath = true;
                SoundManager.Instance.Playsound("Audio/SoundEffects/SuccessFx");
            }
        }
        else if (!One_HasOpenedSkillsWindow)
        {
            DialogManager.Instance.SetText("Equip a skill \nPress \"Z\" to open your skills window");

            if (SkillWindow.activeSelf)
            {
                One_HasOpenedSkillsWindow = true;
                SoundManager.Instance.Playsound("Audio/SoundEffects/SuccessFx");
            }
        }
        else if (!One_HasEquippedSkill)
        {
            DialogManager.Instance.SetText("Drag and Drop a skill to the left most skill slot at the top");

            if (LeftMostSkill.sprite.name != "IconSlot")
            {
                One_HasEquippedSkill = true;
                SoundManager.Instance.Playsound("Audio/SoundEffects/SuccessFx");
            }
        }
        else if (!One_HasClosedSkillsWindow)
        {
            DialogManager.Instance.SetText("Either press the ESC key or \"Z\" again to close the skills window");

            if (!SkillWindow.activeSelf)
            {
                One_HasClosedSkillsWindow = true;
                SoundManager.Instance.Playsound("Audio/SoundEffects/SuccessFx");
            }
        }
        else if (!One_HasSwitchedSkillSet)
        {
            DialogManager.Instance.SetText("Change skillsets with \"A\", \"D\", or numbers [1 - 5]");

            if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Alpha5)) || Player.GetComponent<EntityWeapon>().IsAnySkillLoaded())
            {
                One_HasSwitchedSkillSet = true;
                SoundManager.Instance.Playsound("Audio/SoundEffects/SuccessFx");
            }
        }
        else if (!One_HasLoadedSkill)
        {
            DialogManager.Instance.SetText("With the first skillset selected -\nLeft-Click on a jar to load the skill\n");

            if (Player.GetComponent<EntityWeapon>().IsAnySkillLoaded())
            {
                One_HasLoadedSkill = true;
                SoundManager.Instance.Playsound("Audio/SoundEffects/SuccessFx");
            }
        }
        else if (!One_HasUsedSkill)
        {
            DialogManager.Instance.SetText("Left-Click, again, to execute your skill!");

            if (!Player.GetComponent<EntityWeapon>().IsAnySkillLoaded())
            {
                One_HasUsedSkill = true;
                SoundManager.Instance.Playsound("Audio/SoundEffects/SuccessFx");
            }
        }
        else if (!One_HasDestroyedJars)
        {
            DialogManager.Instance.SetText("Destroy the remaining jars!\n");
            if (HallwayOneJar3.GetComponent<Health>().m_currentHealth < 0 && HallwayOneJar2.GetComponent<Health>().m_currentHealth < 0 && HallwayOneJar1.GetComponent<Health>().m_currentHealth < 0)
            {
                One_HasDestroyedJars = true;
                SoundManager.Instance.Playsound("Audio/SoundEffects/SuccessFx");
            }
        }
        else if (!One_IsMovingToRoomTwo)
        {
            DialogManager.Instance.SetText("Head right.");

            if (Player.transform.position.x > (RoomTwoEntrance.localPosition.x + RoomTwoEntrance.parent.localPosition.x))
            {
                One_IsMovingToRoomTwo = true;
                SoundManager.Instance.Playsound("Audio/SoundEffects/SuccessFx");
            }
        }
        else if (!One_ChangedZoomed)
        {
            DialogManager.Instance.SetText("Used the mouse scroll wheel to change the zoom");

            if (Input.mouseScrollDelta.y > 0 || Input.mouseScrollDelta.y < 0)
            {
                One_ChangedZoomed = true;
                SoundManager.Instance.Playsound("Audio/SoundEffects/SuccessFx");
            }
        }
        else if (!Two_HasStarted)
        {
            DialogManager.Instance.SetText("Let's learn the combat basics.\nPress [SPACE BAR] to enter the room.");

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Two_HasStarted = true;
                Player.transform.position = RoomTwoPosition.position;
                RoomTwoEnemy.SetActive(true);
                RoomTwoDialogIndex = 0;

                SoundManager.Instance.Playsound("Audio/SoundEffects/SuccessFx");
            }
        }
        else if (!FinishedDialogue)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                RoomTwoDialogIndex++;
            }


            if (RoomTwoDialogIndex == RoomTwoDialogList.Count)
            {
                FinishedDialogue = true;
                SoundManager.Instance.Playsound("Audio/SoundEffects/SuccessFx");
            }
            else
                DialogManager.Instance.SetText(RoomTwoDialogList[RoomTwoDialogIndex]);
        }
        else if (!Two_IsEnemyTargetted)
        {
            DialogManager.Instance.SetText("Left-Click the enemy to fight him!");

            if (Player.GetComponent<EntityTarget>().CurrentTarget == RoomTwoEnemy)
            {
                Two_IsEnemyTargetted = true;
                SoundManager.Instance.Playsound("Audio/SoundEffects/SuccessFx");
            }
        }
        else if (!Two_SkillIsCancelled)
        {
            DialogManager.Instance.SetText("Cancel a loaded skill with ESC or the Middle Mouse Button");

            if (Player.GetComponent<EntityWeapon>().IsAnySkillLoaded() && (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(2)))
            {
                Two_SkillIsCancelled = true;
                SoundManager.Instance.Playsound("Audio/SoundEffects/SuccessFx");
            }
        }
        else if (!Two_IsEnemyTargettedWithAutoTarget)
        {
            DialogManager.Instance.SetText("Hold down the \"S\" key for easy clicking and targetting!");

            if (Player.GetComponent<EntityTarget>()._potentialTarget == RoomTwoEnemy)
            {
                Two_IsEnemyTargettedWithAutoTarget = true;
                SoundManager.Instance.Playsound("Audio/SoundEffects/SuccessFx");

                Two_CurrentEnemyHealth = RoomTwoEnemy.GetComponent<Health>().m_currentHealth;
            }
        }
        else if (!Two_IsEnemyDamaged)
        {
            DialogManager.Instance.SetText("While keeping the \"S\" key pressed -\nLeft-Click to attack!\n");

            if ((Two_CurrentEnemyHealth != RoomTwoEnemy.GetComponent<Health>().m_currentHealth) && Input.GetKey(KeyCode.S))
            {
                Two_IsEnemyDamaged = true;
                SoundManager.Instance.Playsound("Audio/SoundEffects/SuccessFx");
            }
            else
                Two_CurrentEnemyHealth = RoomTwoEnemy.GetComponent<Health>().m_currentHealth;
        }
        else if (!Two_EnemyIsStale)
        {
            DialogManager.Instance.SetText("Keep attacking with the same skill");

            if (RoomTwoEnemy.GetComponent<Health>().IsStaled())
            {
                Two_EnemyIsStale = true;
                SoundManager.Instance.Playsound("Audio/SoundEffects/SuccessFx");
            }
        }
        else if (!Two_NoLongerStale)
        {
            DialogManager.Instance.SetText("This skill is now stale\nUse a different skill!");

            if (!RoomTwoEnemy.GetComponent<Health>().IsStaled())
            {
                Two_NoLongerStale = true;
                SoundManager.Instance.Playsound("Audio/SoundEffects/SuccessFx");
            }
        }
        else if (!Two_KnockedBack)
        {
            DialogManager.Instance.SetText("Attack until the enemy is knocked back");

            if (RoomTwoEnemy.GetComponent<EntityStunGuage>().KnockedBack)
            {
                Two_KnockedBack = true;
                SoundManager.Instance.Playsound("Audio/SoundEffects/SuccessFx");
                RoomTwoDialogIndex = 0;
            }
        }
        else if (!Two_KnockBackInfo)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                RoomTwoDialogIndex++;
            }


            if (RoomTwoDialogIndex == RoomTwoKBDialogList.Count)
            {
                Two_KnockBackInfo = true;
                SoundManager.Instance.Playsound("Audio/SoundEffects/SuccessFx");
            }
            else
                DialogManager.Instance.SetText(RoomTwoKBDialogList[RoomTwoDialogIndex]);
        }
        else if (!Two_EnemyIsInvincible)
        {
            DialogManager.Instance.SetText("Attack the enemy as it's being knocked back!\nTip: Try using something like Arrow Revolver");

            if (RoomTwoEnemy.GetComponent<EntityStunGuage>().Invincible)
            {
                Two_EnemyIsInvincible = true;
                SoundManager.Instance.Playsound("Audio/SoundEffects/SuccessFx");

                RoomTwoEnemy.GetComponent<Health>().SetHealth(10f, 20f);
            }
        }
        else if (!Two_EnemyDead)
        {
            DialogManager.Instance.SetText("Attack to finish the enemy!");

            if (RoomTwoEnemy == null)
            {
                SoundManager.Instance.Playsound("Audio/SoundEffects/SuccessFx");
                Two_EnemyDead = true;
            }
        }
        else if (!Two_CharacterWindowOpened)
        {
            DialogManager.Instance.SetText("Level Up!\nPress \"C\" to open the character window");

            if (CharacterPanel.activeSelf)
            {
                SoundManager.Instance.Playsound("Audio/SoundEffects/SuccessFx");
                Two_CharacterWindowOpened = true;
            }
        }
        else if (!Two_SkillPointUsed)
        {
            DialogManager.Instance.SetText("Use your new stat points!");

            if (Player.GetComponent<Exp>().CurrentStatPoints == 0)
            {
                SoundManager.Instance.Playsound("Audio/SoundEffects/SuccessFx");
                Two_SkillPointUsed = true;
            }
        }
        else if (!Two_CharacterWindowClosed)
        {
            DialogManager.Instance.SetText("Press \"C\" or ESC to close");

            if (!CharacterPanel.activeSelf)
            {
                SoundManager.Instance.Playsound("Audio/SoundEffects/SuccessFx");
                Two_CharacterWindowClosed = true;
                RoomTwoReward.SetActive(true);
            }
        }
        else if (!Two_RewardPickedUp)
        {
            DialogManager.Instance.SetText("Pick up the weapon drop!");

            if (RoomTwoReward == null)
            {
                SoundManager.Instance.Playsound("Audio/SoundEffects/SuccessFx");
                Two_RewardPickedUp = true;
            }
        }
        else if (!Two_InventoryOpened)
        {
            DialogManager.Instance.SetText("Open your equipment manager with \"E\"");

            if (InventoryPanel.activeSelf)
            {
                SoundManager.Instance.Playsound("Audio/SoundEffects/SuccessFx");
                Two_InventoryOpened = true;
            }
        }
        else if (!Two_InventoryClosed)
        {
            DialogManager.Instance.SetText("Close it with \"E\" or ESC");

            if (!InventoryPanel.activeSelf)
            {
                SoundManager.Instance.Playsound("Audio/SoundEffects/SuccessFx");
                Two_InventoryClosed = true;
            }
        }
        else if (!Two_SwitchedWeapons)
        {
            DialogManager.Instance.SetText("Press [TAB] to switch weapons!");

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                SoundManager.Instance.Playsound("Audio/SoundEffects/SuccessFx");
                Two_SwitchedWeapons = true;
                MagicSkills.SetActive(true);
            }
        }
        else if (!Two_MagicOpened)
        {
            DialogManager.Instance.SetText("Press \"Z\" to see your new magic skills");

            if (SkillWindow.activeSelf)
            {
                SoundManager.Instance.Playsound("Audio/SoundEffects/SuccessFx");
                Two_MagicOpened = true;
            }
        }
        else if (!Two_MagicClosed)
        {
            DialogManager.Instance.SetText("Close it with \"Z\" or ESC");

            if (!SkillWindow.activeSelf)
            {
                SoundManager.Instance.Playsound("Audio/SoundEffects/SuccessFx");
                Two_MagicClosed = true;
            }
        }
        else
        {
            MeleeSkills.SetActive(true);
            MagicSkills.SetActive(true);

            Player.transform.position = SpawnPosition.position;

            DialogManager.Instance.HeldHostage = false;
            DialogManager.Instance.SetText("");
            DialogManager.Instance.InstantSystemMessage("The tutorial is complete!");

            IsRoomOneDone = true;
            _tutorialStarted = false;
        }
    }

    void SetRoomOneFlagsToDone()
    {
        // Room 1 Flags
        One_HasMovedToEntrance = true;
        One_HasMovedToNextRoom = true;
        One_HasCollectedCollectables = true;
        One_HasReachedBlockedPath = true;

        // Hallway One Flags
        One_HasOpenedSkillsWindow = true;
        One_HasEquippedSkill = true;
        One_HasSwitchedSkillSet = true;
        One_HasClosedSkillsWindow = true;
        One_HasLoadedSkill = true;
        One_HasUsedSkill = true;
        One_HasDestroyedJars = true;
        One_IsMovingToRoomTwo = true;
    }
}
