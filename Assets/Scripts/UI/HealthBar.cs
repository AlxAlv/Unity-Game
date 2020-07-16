using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Transform _containerTransform;

    private float _currentHealth = 1;
    private float _maxHealth = 1;

    public void UpdateHealth(float currentHealth, float maxHealth)
    {
        _currentHealth = currentHealth;
        _maxHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        _containerTransform.localScale = new Vector3((_currentHealth / _maxHealth), 1);
    }
}
