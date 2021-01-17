using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SaveManager : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
	[SerializeField] CoinManager _coinManager;
	[SerializeField] Exp _expManager;
	[SerializeField] StatManager _statManager;
	[SerializeField] SkillUnlockerManager _skillManager;

	private readonly string COINS_KEY = "MYGAME_MYCOINS_DONOTCHEAT";
	private readonly string EXP_KEY = "MYGAME_MYCURRENTEXP_DONOTCHEAT";
	private readonly string TARGETEXP_KEY = "MYGAME_MYTARGETEXP_DONOTCHEAT";
	private readonly string LEVEL_KEY = "MYGAME_MYCURRENTLEVEL_DONOTCHEAT";
	private readonly string STATPOINTS_KEY = "MYGAME_MYSTATPOINTS_DONOTCHEAT";
	private readonly string INT_KEY = "MYGAME_MYINT_DONOTCHEAT";
	private readonly string DEX_KEY = "MYGAME_MYDEX_DONOTCHEAT";
	private readonly string STR_KEY = "MYGAME_MYSTR_DONOTCHEAT";

	// Skills
	private readonly string POISONARROW_KEY = "MY_POISONARROW_KEY";
	private readonly string CHARGEDSHOT_KEY = "MY_CHARGEDSHOT_KEY";
	private readonly string FROZENDAGGERS_KEY = "MY_FROZENDAGGERS_KEY";
	private readonly string BOULDERTOSS_KEY = "MY_BOULDERTOSS_KEY";
	private readonly string HEAL_KEY = "MY_HEAL_KEY";
	private readonly string SKYFALL_KEY = "MY_SKYFALL_KEY";



	public void OnPointerDown (PointerEventData eventData) { }
    public void OnPointerUp   (PointerEventData eventData) { }
    public void OnPointerClick(PointerEventData eventData)
    {
	    SaveAllData();
    }

    private void Start()
    {
	    LoadAllData();
    }

    private void Update()
    {
	    if (Input.GetKeyDown(KeyCode.P))
	    {
		    SaveAllData();
	    }

    }

    private void SaveSkillsData()
    {
	    PlayerPrefs.SetInt(POISONARROW_KEY, BoolToInt(_skillManager._poisonArrowUnlocker.IsUnlocked));
	    PlayerPrefs.SetInt(CHARGEDSHOT_KEY, BoolToInt(_skillManager._chargedShotUnlocker.IsUnlocked));
	    PlayerPrefs.SetInt(FROZENDAGGERS_KEY, BoolToInt(_skillManager._frozenDaggersUnlocker.IsUnlocked));
	    PlayerPrefs.SetInt(BOULDERTOSS_KEY, BoolToInt(_skillManager._boulderTossUnlocker.IsUnlocked));
	    PlayerPrefs.SetInt(HEAL_KEY, BoolToInt(_skillManager._healUnlocker.IsUnlocked));
	    PlayerPrefs.SetInt(SKYFALL_KEY, BoolToInt(_skillManager._skyfallSkillUnlocker.IsUnlocked));
	}

    private void LoadSkillsData()
    {
	    if (!PlayerPrefs.HasKey(POISONARROW_KEY))
		    return;

	    _skillManager._poisonArrowUnlocker.IsUnlocked = IntToBool(PlayerPrefs.GetInt(POISONARROW_KEY));
	    _skillManager._chargedShotUnlocker.IsUnlocked = IntToBool(PlayerPrefs.GetInt(CHARGEDSHOT_KEY));
	    _skillManager._frozenDaggersUnlocker.IsUnlocked = IntToBool(PlayerPrefs.GetInt(FROZENDAGGERS_KEY));
	    _skillManager._boulderTossUnlocker.IsUnlocked = IntToBool(PlayerPrefs.GetInt(BOULDERTOSS_KEY));
	    _skillManager._healUnlocker.IsUnlocked = IntToBool(PlayerPrefs.GetInt(HEAL_KEY));
	    _skillManager._skyfallSkillUnlocker.IsUnlocked = IntToBool(PlayerPrefs.GetInt(SKYFALL_KEY));
	}

    public void LoadAllData()
	{
		LoadCoins();
		LoadEXPData();
		LoadStatsData();
		LoadSkillsData();
	}

	public void SaveAllData()
    {
	    if (ArenaManager.Instance.IsArenaStarted || ArenaManager.Instance.IsDungeonStarted || DungeonGenerator.Instance.StartedGeneration)
	    {
		    DialogManager.Instance.InstantSystemMessage("Can Not Save Right Now!");
		    return;
	    }

		SaveCoins();
		SaveEXPData();
		SaveStatsData();
		SaveSkillsData();

		DialogManager.Instance.InstantSystemMessage("All Data Has Been Saved!");
	}

	/* Coins */
    public void SaveCoins()
    {
	    PlayerPrefs.SetInt(COINS_KEY, _coinManager.Coins);
    }

    public void LoadCoins()
    {
	    if (!PlayerPrefs.HasKey(COINS_KEY))
		    return;

		_coinManager.Coins = PlayerPrefs.GetInt(COINS_KEY);
    }

	/* EXP */
	public void SaveEXPData()
	{
		PlayerPrefs.SetInt(EXP_KEY, _expManager.CurrentEXP);
		PlayerPrefs.SetInt(TARGETEXP_KEY, _expManager.CurrentTargetEXP);
		PlayerPrefs.SetInt(LEVEL_KEY, _expManager.CurrentLevel);
		PlayerPrefs.SetInt(STATPOINTS_KEY, _expManager.CurrentStatPoints);
	}

	public void LoadEXPData()
	{
		if (!PlayerPrefs.HasKey(EXP_KEY))
			return;

		int currentExp = PlayerPrefs.GetInt(EXP_KEY);
		int targetExp = PlayerPrefs.GetInt(TARGETEXP_KEY);
		int level = PlayerPrefs.GetInt(LEVEL_KEY);
		int statPoints = PlayerPrefs.GetInt(STATPOINTS_KEY);

		_expManager.SetEXPData(statPoints, currentExp, targetExp, level);
	}

	/* Stats */
	public void SaveStatsData()
	{
		PlayerPrefs.SetInt(INT_KEY, _statManager.Intelligence.StatAmount);
		PlayerPrefs.SetInt(DEX_KEY, _statManager.Dexterity.StatAmount);
		PlayerPrefs.SetInt(STR_KEY, _statManager.Strength.StatAmount);
	}

	public void LoadStatsData()
	{
		if (!PlayerPrefs.HasKey(INT_KEY))
			return;

		int intelligence = PlayerPrefs.GetInt(INT_KEY);
		int dexterity = PlayerPrefs.GetInt(DEX_KEY);
		int strength = PlayerPrefs.GetInt(STR_KEY);

		_statManager.SetStatsData(intelligence, dexterity, strength);
	}

	private int BoolToInt(bool val)
	{
		if (val)
			return 1;
		else
			return 0;
	}

	private bool IntToBool(int val)
	{
		if (val != 0)
			return true;
		else
			return false;
	}
}
