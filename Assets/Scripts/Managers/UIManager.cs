using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [Header("Settings")]
    [SerializeField] private Image m_healthBar;
    [SerializeField] private TextMeshProUGUI m_currentHealthText;
    [SerializeField] private Image m_manaBar;
    [SerializeField] private TextMeshProUGUI m_currentManaText;
    [SerializeField] private Image m_staminaBar;
    [SerializeField] private TextMeshProUGUI m_currentStaminaText;
    [SerializeField] private Image _expBar;
    [SerializeField] private TextMeshProUGUI _currentExpText;
    [SerializeField] private Image _hungerBar;

    [Header("Text")] [SerializeField] private TextMeshProUGUI _coinText;

    private float m_playerCurrentHealth;
    private float m_playerMaxHealth;

    private float m_playerCurrentMana;
    private float m_playerMaxMana;
    private float m_playerCurrentStamina;
    private float m_playerMaxStamina;

    private float _currentExp;
    private float _expNeededToLevelUp;
    private int _currentLevel;

    private float _currentHunger;

    private string m_currentWeapon;

    private bool _isPlayer;

    private void Update()
    {
        InternalUpdate();
    }

    private void InternalUpdate()
    {
        if (_isPlayer)
        {
	        m_healthBar.fillAmount = Mathf.Lerp(m_healthBar.fillAmount, m_playerCurrentHealth / m_playerMaxHealth, 10f * Time.deltaTime);
	        m_currentHealthText.text = m_playerCurrentHealth.ToString() + "/" + m_playerMaxHealth.ToString();

 	        m_manaBar.fillAmount = Mathf.Lerp(m_manaBar.fillAmount, m_playerCurrentMana / m_playerMaxMana, 10f * Time.deltaTime);
	        m_currentManaText.text = m_playerCurrentMana.ToString() + "/" + m_playerMaxMana.ToString();

	        m_staminaBar.fillAmount = Mathf.Lerp(m_staminaBar.fillAmount, m_playerCurrentStamina / m_playerMaxStamina, 10f * Time.deltaTime);
	        m_currentStaminaText.text = m_playerCurrentStamina.ToString() + "/" + m_playerMaxStamina.ToString();

            _expBar.fillAmount = Mathf.Lerp(_expBar.fillAmount, _currentExp / _expNeededToLevelUp, 10f * Time.deltaTime);
            _currentExpText.text = "Lv." + _currentLevel.ToString() + " - " +_currentExp.ToString() + "/" + _expNeededToLevelUp.ToString();

            _hungerBar.fillAmount = Mathf.Lerp(_hungerBar.fillAmount, Mathf.Abs(1.0f -_currentHunger), 10f * Time.deltaTime);

        }

        // Update Coins
        _coinText.text = CoinManager.Instance.Coins.ToString();
    }

    public void UpdateHealth(float currentHealth, float maxHealth, float currentShield, float maxShield, bool isPlayer)
    {
        m_playerCurrentHealth = currentHealth;
        m_playerMaxHealth = maxHealth;
        _isPlayer = isPlayer;
    }

    public void UpdateMana(float currentMana, float maxMana, bool isPlayer)
    {
        m_playerCurrentMana = currentMana;
        m_playerMaxMana = maxMana;
        _isPlayer = isPlayer;
    }

    public void UpdateStamina(float currentStamina, float maxStamina, bool isPlayer)
    {
        m_playerCurrentStamina = currentStamina;
        m_playerMaxStamina = maxStamina;
        _isPlayer = isPlayer;
    }

    public void UpdateExp(float currentExp, float expToNextLevel, int currentLevel, bool isPlayer)
    {
        _currentExp = currentExp;
        _expNeededToLevelUp = expToNextLevel;
        _currentLevel = currentLevel;
        _isPlayer = isPlayer;
    }

    public void UpdateHunger(float currentModifier)
    {
        _currentHunger = currentModifier;
    }
}
