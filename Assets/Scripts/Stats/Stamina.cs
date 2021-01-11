using System;
using UnityEngine;

public class Stamina : MonoBehaviour
{
	[Header("Stamina")]
	[SerializeField] private float _initialStamina = 10f;
	[SerializeField] public float _maxStamina = 10f;
	[SerializeField] private float _staminaRegenWait = 1f;
	[SerializeField] private float _regenValue = 1.0f;

	private Entity _entity;
	private bool _isPlayer;
	private float _regenTimer;
	private StatManager _statManager;

	public float CurrentMaxStamina { get; set; }
	public float _currentStamina { get; set; }

	public void GainStamina(float amount)
	{
		if (_currentStamina < CurrentMaxStamina)
		{
			_currentStamina = (float)Math.Round((Mathf.Min(_currentStamina + amount, CurrentMaxStamina)), 1);
			UpdateStamina();
		}
	}

	public bool UseStamina(float amount)
	{
		if (_currentStamina < amount)
		{
			DialogManager.Instance.InstantSystemMessage("Not enough stamina!");
			return false;
		}

		if (_entity != null)
		{
			_currentStamina -= amount;

			UpdateStamina();

			return true;
		}

		return false;
	}

	private void Start()
	{
		_regenTimer = 0.0f;

		_entity = GetComponent<Entity>();
		_statManager = _entity.GetComponent<StatManager>();

		_currentStamina = CalculateStamina();

		if (_entity != null)
		{
			_isPlayer = (_entity.EntityType == Entity.EntityTypes.Player);
		}

		UpdateStamina();
	}

	private void Update()
	{
		Regen();

		UpdateStamina();
	}

	private void Regen()
	{
		if (_regenTimer < _staminaRegenWait)
		{
			_regenTimer += Time.deltaTime;
		}
		else
		{
			_regenTimer = 0.0f;
			GainStamina(_regenValue);
		}
	}

	private float CalculateStamina()
	{
		return _initialStamina + (_statManager.Dexterity.TotalAmount * 5);
	}

	private void UpdateStamina()
	{
		if (_statManager != null)
			_maxStamina = CalculateStamina();

		if ((_currentStamina > CurrentMaxStamina) && (CurrentMaxStamina > 0))
			_currentStamina = CurrentMaxStamina;

		if (_entity != null)
			UIManager.Instance.UpdateStamina(_currentStamina, _maxStamina, _isPlayer);
	}

	public void RefillStamina()
	{
		_currentStamina = CalculateStamina();
	}
}
