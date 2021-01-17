using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : Singleton<CoinManager>
{
	public int Coins { get; set; }

	public void AddCoins(int amount)
    {
	    UIManager.Instance.BounceCoinText();
        Coins += amount;
    }

    public bool RemoveCoins(int amount)
    {
	    if (amount > Coins)
		    return false;

	    UIManager.Instance.BounceCoinText();
        Coins -= amount;

        return true;
    }
}
