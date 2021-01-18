using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : Singleton<BossHealthBar>
{
	[SerializeField] private GameObject _healthObject;
	[SerializeField] private Image _healthBar;

	private GameObject _bossObject;
	private Health _bossHealth;

	void Start()
	{
		UIBounce.Instance.SetOriginalBossPosition(_healthObject.transform.localPosition);
	}

	// Update is called once per frame
    void Update()
    {
        if (SoundManager.Instance.IsInCombat && _bossObject != null && _bossHealth != null)
        {
			if (!_healthObject.activeSelf)
				UIBarOpen.Instance.OpenUpBar(_healthObject);

			_healthBar.fillAmount = Mathf.Lerp(_healthBar.fillAmount, _bossHealth.m_currentHealth / _bossHealth.m_maxHealth, 10f * Time.deltaTime);
			_healthObject.SetActive(true);
		}
        else
	        _healthObject.SetActive(false);
	}

    public void SetBossObject(GameObject bossObject)
    {
	    _bossObject = bossObject;
	    _bossHealth = _bossObject.GetComponent<Health>();
    }

    public void Bounce()
    {
		if (_healthObject.activeSelf)
			UIBounce.Instance.BounceBossUI(_healthObject, 20f);
    }
}
