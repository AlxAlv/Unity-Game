using UnityEngine;

public class CoinReward : Collectables
{
	[SerializeField] private int _coinsToAdd = 25;

	protected override bool Pick()
	{
		AddCoins();
		SoundManager.Instance.Playsound("Audio/SoundEffects/CoinPickupFx");

		return true;
	}

	private void AddCoins()
	{
		CoinManager.Instance.AddCoins(_coinsToAdd);
		GoldNumbers.Create(transform.position, (int)_coinsToAdd);
	}
}