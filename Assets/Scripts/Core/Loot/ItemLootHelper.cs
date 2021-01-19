using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemLootHelper : MonoBehaviour
{
	[SerializeField] private ItemRewardType _rewardType;
	[SerializeField] private int _minAmountToGive;
	[SerializeField] private int _maxAmountToGive;

	public void GiveItems(Inventory playerInventory)
	{
		if (playerInventory == null)
			return;

		int amount = Random.Range(_minAmountToGive, _maxAmountToGive);
		if (amount > 0)
		{
			ItemNumberPopups.Create(transform.position, (int)amount, _rewardType);
			playerInventory.AddItems(_rewardType, amount);
		}
	}
}
