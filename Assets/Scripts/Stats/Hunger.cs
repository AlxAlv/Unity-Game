using UnityEngine;

public class Hunger : MonoBehaviour
{
    [SerializeField] private Stamina _stamina;
    [SerializeField] private float _secondsPerPercentageDrop = 5;

    private float _timer = 0.0f;
    private float _currentModifier = 1.0f;

    // Update is called once per frame
    void Update()
    {
        UpdateStamina();

        if (_timer == 0.0f)
            _timer = Time.time + _secondsPerPercentageDrop;

        if (IsTimerDone())
            _currentModifier = Mathf.Max(.20f, (_currentModifier - 0.01f));

        // Update UI
        UIManager.Instance.UpdateHunger(_currentModifier);
    }

    private void UpdateStamina()
    {
        _stamina.CurrentMaxStamina = (_stamina._maxStamina * _currentModifier);
    }

    private bool IsTimerDone()
    {
        if (Time.time > _timer)
        {
            _timer = 0.0f;
            return true;
        }

        return false;
    }

    public void Eat(float hungerValue)
    {
        _currentModifier = Mathf.Min(1.0f, (_currentModifier + hungerValue));
    }
}
