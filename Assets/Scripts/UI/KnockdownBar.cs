using UnityEngine;

public class KnockdownBar : MonoBehaviour
{
    [SerializeField] Transform _containerTransform;

    private EntityStunGuage _entityStunGuage;

    private float _currentGuageAmount = 0;
    private float _maxAmount = 100;
    private float _currentPercent = 0;

    private float _regenTimer = 0f;
    private float _totalRegenTime = 5f;
    private float _currentRegenTime = 5f;

    public float CurrentGuageAmount => _currentGuageAmount;

    private void Awake()
    {
        
    }

    public void ResetCurrentGuage()
    {
        _currentGuageAmount = 0.0f;
    }

    void Update()
    {
        UpdateKnockbackGuage();
        UpdateUI();
    }

    public void SetSunGuage(EntityStunGuage stunGuage)
    {
        _entityStunGuage = stunGuage;
    }

    private void UpdateUI()
    {
        _currentPercent = _currentGuageAmount / _maxAmount;
        _containerTransform.localScale = new Vector3(Mathf.Min(_currentPercent, 1), 1);
    }
    
    public void AddAmount(float amount)
    {
        _currentGuageAmount = Mathf.Min((_currentGuageAmount + amount), (_maxAmount * 1.5f));

        if (_currentGuageAmount >= 100)
            _entityStunGuage.BecomeKnockedback();
    }

    private void UpdateKnockbackGuage()
    {
        _currentRegenTime = _totalRegenTime / _maxAmount;

        if (_regenTimer < _currentRegenTime)
        {
            _regenTimer += Time.deltaTime;
        }
        else
        {
            _regenTimer = 0.0f;
            _currentGuageAmount -= 1;

            if (_currentGuageAmount < 0)
                _currentGuageAmount = 0;
        }
    }
}
