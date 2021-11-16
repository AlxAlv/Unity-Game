using TMPro;
using UnityEngine;

public class StatManager : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private int _initialInt = 1;
    [SerializeField] private int _initialDex = 1;
    [SerializeField] private int _initialStr = 1;

    [Header("Panels")]
    [SerializeField] private GameObject _characterPanel;
    [SerializeField] private GameObject _statContainer;
    [SerializeField] private TextMeshProUGUI _intelligenceTextAmount;
    [SerializeField] private TextMeshProUGUI _dexterityTextAmount;
    [SerializeField] private TextMeshProUGUI _strengthTextAmount;
    [SerializeField] private TextMeshProUGUI _pointsTextAmount;
    [SerializeField] private OnClickStat _dexterityButton;
    [SerializeField] private OnClickStat _intelligenceButton;
    [SerializeField] private OnClickStat _strengthButton;

    [Header("AI")]
    [SerializeField] private int _intPerLevel = 1;
    [SerializeField] private int _dexPerLevel = 1;
    [SerializeField] private int _strPerLevel = 1;
    [SerializeField] public int HealthPerLevel = 0;

    public Intelligence Intelligence;
    public Dexterity Dexterity;
    public Strength Strength;

    private Exp _exp;

    // Start is called before the first frame update
    void Awake()
    {
        Intelligence = new Intelligence(StatType.Intelligence, _initialInt);
        Dexterity = new Dexterity(StatType.Dexterity, _initialDex);
        Strength = new Strength(StatType.Strength, _initialStr);

        _exp = GetComponent<Exp>();

        if (GetComponent<Entity>().EntityType == Entity.EntityTypes.Player)
        {
            GameObject statsGroup = GameObject.FindWithTag("StatsGroup");
            _statContainer = statsGroup.transform.Find("StatContainer").gameObject;
            _characterPanel = statsGroup.transform.parent.gameObject;

            _dexterityButton = _statContainer.transform.Find("Dexterity").Find("UpButton").GetComponent<OnClickStat>();
            _intelligenceButton = _statContainer.transform.Find("Intelligence").Find("UpButton").GetComponent<OnClickStat>();
            _strengthButton = _statContainer.transform.Find("Strength").Find("UpButton").GetComponent<OnClickStat>();

            _intelligenceTextAmount = _statContainer.transform.Find("Intelligence").Find("Amount").GetComponent<TextMeshProUGUI>();
            _dexterityTextAmount = _statContainer.transform.Find("Dexterity").Find("Amount").GetComponent<TextMeshProUGUI>();
            _strengthTextAmount = _statContainer.transform.Find("Strength").Find("Amount").GetComponent<TextMeshProUGUI>();
            _pointsTextAmount = statsGroup.transform.Find("StatPoints").Find("Amount").GetComponent<TextMeshProUGUI>();

            _dexterityButton.SetStat(0, this);
            _intelligenceButton.SetStat(1, this);
            _strengthButton.SetStat(2, this);
        }

        if (_characterPanel != null)
            _characterPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<Entity>().EntityType == Entity.EntityTypes.Player)
            HandleInput();
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            _characterPanel.SetActive(!_characterPanel.activeSelf);
            _statContainer.SetActive(_characterPanel.activeSelf);

        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            _characterPanel.SetActive(false);
            _statContainer.SetActive(false);

        }

        if (_characterPanel.activeSelf)
        {
            UpdateStats();
        }
    }

    public void ModifyEquipStats(int strBonus, int intBonus, int dexBonus)
    {
        Intelligence.BonusAmount += intBonus;
        Strength.BonusAmount += strBonus;
        Dexterity.BonusAmount += dexBonus;
    }

    public void AddIntelligence()
    {
        if (_exp.UseStatPoints(1))
        {
            Intelligence.StatAmount++;
            DialogManager.Instance.InstantSystemMessage("Increased Intelligence to " + Intelligence.StatAmount);
        }
    }

    public void AddDexterity()
    {
        if (_exp.UseStatPoints(1))
        {
            Dexterity.StatAmount++;
            DialogManager.Instance.InstantSystemMessage("Increased Dexterity to " + Dexterity.StatAmount);
        }
    }

    public void AddStrength()
    {
        if (_exp.UseStatPoints(1))
        {
            Strength.StatAmount++;
            DialogManager.Instance.InstantSystemMessage("Increased Strength to " + Strength.StatAmount);
        }
    }

    void UpdateStats()
    {
        _intelligenceTextAmount.text = Intelligence.TotalAmount.ToString();
        _dexterityTextAmount.text = Dexterity.TotalAmount.ToString();
        _strengthTextAmount.text = Strength.TotalAmount.ToString();
        if (_pointsTextAmount != null && _exp != null)
            _pointsTextAmount.text = _exp.GetPoints().ToString();
    }

    public void SetStatsData(int intelligence, int dexterity, int strength)
    {
	    Intelligence.StatAmount = intelligence;
	    Dexterity.StatAmount = dexterity;
	    Strength.StatAmount = strength;
    }

    public void RemoveStats()
    {
	    Dexterity.StatAmount = 0;
	    Intelligence.StatAmount = 0;
	    Strength.StatAmount = 0;
    }

    public void SetLevel(int level)
    {
	    Intelligence.StatAmount += (_intPerLevel * level);
	    Dexterity.StatAmount += (_dexPerLevel * level);
	    Strength.StatAmount += (_strPerLevel * level);
	    HealthPerLevel *= level;
    }
}
