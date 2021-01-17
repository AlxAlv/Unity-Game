using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideSkillInShop : MonoBehaviour
{
	[SerializeField] private SkillUnlocker _skillUnlocker;

	void Update()
	{
		gameObject.SetActive(!(_skillUnlocker.IsUnlocked));
	}
}
