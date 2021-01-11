using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : Singleton<CoinManager>
{
	public int Coins { get; set; }

	public void AddCoins(int amount)
    {
	    Coins += amount;
    }

    public void RemoveCoins(int amount)
    {
	    Coins -= amount;
    }
}
