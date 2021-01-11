using UnityEngine;

public class CoinReward : Collectables
{
	[SerializeField] private int _coinsToAdd = 25;

	protected override void Pick()
	{
		AddCoins();

		SoundManager.Instance.Playsound("Audio/SoundEffects/CoinPickupFx");
	}

	private void AddCoins()
	{
		CoinManager.Instance.AddCoins(_coinsToAdd);
		GoldNumbers.Create(transform.position, (int)_coinsToAdd);
	}
}