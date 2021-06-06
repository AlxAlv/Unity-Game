using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltimatePoints : MonoBehaviour
{
    [Header("Mana")]
    [SerializeField] private float _initialUltimatePoints = 0f;
    [SerializeField] private float _maxUltimatePoints = 100f;
    [SerializeField] private float _ultimateLossTick = 1.0f;
    [SerializeField] private float _lossValue = 1.0f;

	private Entity _entity;
	private StatManager _statManager;
	private bool _isPlayer;
	private float _lossTimer;

	public float CurrentUltimatePoints { get; set; }

	public void GainUltimatePoints(float amount)
	{
		CurrentUltimatePoints = Mathf.Min(CurrentUltimatePoints + amount, _maxUltimatePoints);
	}

	public bool UseUltimateBarPoints(float amount)
	{
		if (CurrentUltimatePoints < amount)
		{
			DialogManager.Instance.InstantSystemMessage("Not enough ultimate points!");
			return false;
		}

		if (_entity != null)
		{
			CurrentUltimatePoints -= amount;

			if (_entity != null)
				UIManager.Instance.UpdateUltimatePoints(CurrentUltimatePoints, _maxUltimatePoints, _isPlayer);

			return true;
		}

		return false;
	}

	private void Start()
	{
		_lossTimer = 0.0f;
		_entity = GetComponent<Entity>();

		if (_entity != null)
		{
			_statManager = _entity.GetComponent<StatManager>();
			_isPlayer = (_entity.EntityType == Entity.EntityTypes.Player);
		}

		CurrentUltimatePoints = 0f;
	}

	private void Update()
	{
		Timer();

		if (_entity != null)
			UIManager.Instance.UpdateUltimatePoints(CurrentUltimatePoints, _maxUltimatePoints, _isPlayer);
	}

	private void LossTimer()
	{
		if (CurrentUltimatePoints > 0)
			CurrentUltimatePoints -= _lossValue;
	}

	private void Timer()
	{
		if (_lossTimer < _ultimateLossTick)
		{
			_lossTimer += Time.deltaTime;
		}
		else
		{
			_lossTimer = 0.0f;
			LossTimer();
		}
	}
}
