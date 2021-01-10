using System.Collections.Generic;
using UnityEngine;

public class Exp : MonoBehaviour
{
    private Dictionary<int, int> levelToExpNeeded = new Dictionary<int, int>()
    {
        { 1, 40 }, { 2, 80 }, { 3, 200 }, { 4, 400 }, { 5, 700 },
        { 6, 1000 }, { 7, 1500 }, { 8, 2000 }, { 9, 3500 }, { 10, 5000 }
    };

    [SerializeField] public int ExpToGive = 10;

    private int _currentStatPoints = 0;
    private int _pointsPerLevel = 2;

    private int _currentExp;
    private int _expForNextLevel;

    private int _currentLevel;

    public int CurrentStatPoints => _currentStatPoints;

    // Start is called before the first frame update
    void Start()
    {
        _currentExp = 0;
        _currentLevel = 1;
        _expForNextLevel = levelToExpNeeded[_currentLevel];
    }

    // Update is called once per frame
    void Update()
    {
        if (_currentExp >= _expForNextLevel)
        {
            LevelUp();
        }
        
        UpdateExp();
    }

    private void UpdateExp()
    {
        if (GetComponent<Entity>() != null && GetComponent<Entity>().EntityType == Entity.EntityTypes.Player)
            UIManager.Instance.UpdateExp(_currentExp, _expForNextLevel, _currentLevel, true);
    }

    private void LevelUp()
    {
        _currentExp = (_currentExp - _expForNextLevel);
        _currentLevel++;
        _currentStatPoints += _pointsPerLevel;

        _expForNextLevel = levelToExpNeeded[_currentLevel];

        DialogManager.Instance.InstantSystemMessage("Levled Up To Lv." + _currentLevel);
        DialogManager.Instance.AddSystemMessage("Press \"C\" To Use Points");
        SoundManager.Instance.Playsound("Audio/SoundEffects/LevelUpFx");

        RefillResources();
    }

    private void RefillResources()
    {
        if (GetComponent<Mana>() != null)
            GetComponent<Mana>().RefillMana();

        if (GetComponent<Health>() != null)
            GetComponent<Health>().RefillHealth();

        if (GetComponent<Stamina>() != null)
            GetComponent<Stamina>().RefillStamina();
    }

    public void GainExp(int amount)
    {
        _currentExp += amount;
    }

    public bool UseStatPoints(int amount)
    {
        if (_currentStatPoints >= amount)
        {
            _currentStatPoints -= amount;
            return true;
        }

        DialogManager.Instance.InstantSystemMessage("Not Enough Stat Points");

        return false;
    }

    public int GetPoints()
    {
        return _currentStatPoints;
    }
}
