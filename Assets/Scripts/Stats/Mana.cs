using UnityEngine;

public class Mana : MonoBehaviour
{
	[Header("Mana")]
	[SerializeField] private float _initialMana = 20f;
	[SerializeField] private float _maxMana = 20f;
	[SerializeField] private float _manaRegenWait = 2.0f;
	[SerializeField] private float _regenValue = 1.0f;

	private Entity _entity;
	private StatManager _statManager;
	private bool _isPlayer;
	private float _regenTimer;

	public float _currentMana { get; set; }

	public void GainMana(float amount)
	{
		_currentMana = Mathf.Min(_currentMana + amount, _maxMana);
		UpdateMana();
	}

	public bool UseMana(float amount)
	{
		if (_currentMana < amount)
		{
			DialogManager.Instance.InstantSystemMessage("Not enough mana!");
			return false;
		}

		if (_entity != null)
		{
			_currentMana -= amount;

			UpdateMana();

			return true;
		}

		return false;
	}

	private void Start()
	{
		_regenTimer = 0.0f;
		_entity = GetComponent<Entity>();

		if (_entity != null)
		{
			_statManager = _entity.GetComponent<StatManager>();
			_isPlayer = (_entity.EntityType == Entity.EntityTypes.Player);
		}

		_currentMana = CalcualteMaxMana();
		UpdateMana();
	}

	private void Update()
	{
		Regen();

		UpdateMana();
	}

	private void Regen()
	{
		if (_regenTimer < _manaRegenWait)
		{
			_regenTimer += Time.deltaTime;
		}
		else
		{
			_regenTimer = 0.0f;
			GainMana(_regenValue);
		}
	}
	private void UpdateMana()
	{
		if (_statManager != null)
			_maxMana = CalcualteMaxMana();

		if (_entity != null)
			UIManager.Instance.UpdateMana(_currentMana, CalcualteMaxMana(), _isPlayer);
	}

	private float CalcualteMaxMana()
	{
		return _initialMana + (_statManager.Intelligence.TotalAmount * 2);
	}

	public void RefillMana()
	{
		_currentMana = CalcualteMaxMana();
	}
}
