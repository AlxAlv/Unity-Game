using UnityEngine;

public class CoinReward : Collectables
{
	[SerializeField] private int _coinsToAdd = 25;

	protected override void Pick()
	{
		AddCoins();
	}

	private void AddCoins()
	{
		CoinManager.Instance.AddCoins(_coinsToAdd);
		GoldNumbers.Create(transform.position, (int)_coinsToAdd);
	}
}