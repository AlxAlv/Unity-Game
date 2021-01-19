using UnityEngine;

public class CoinReward : Collectables
{
	[SerializeField] private int _coinsToAdd = 25;

	private void Awake()
	{
		_pickupSoundPath = "Audio/SoundEffects/CoinPickupFx";
	}

	protected override bool Pick()
	{
		AddCoins();
		PlayEffects();

		return true;
	}

	private void AddCoins()
	{
		CoinManager.Instance.AddCoins(_coinsToAdd);
		GoldNumbers.Create(transform.position, (int)_coinsToAdd);
	}
}