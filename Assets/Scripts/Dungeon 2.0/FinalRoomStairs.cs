using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalRoomStairs : MonoBehaviour
{
	[SerializeField] private GameObject _stairsInfo;

	private bool _canGo;

	void Update()
	{
		if (_canGo && Input.GetKeyDown(KeyCode.F))
		{
			ArenaManager.Instance.StartBossRoom();
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			_stairsInfo.SetActive(true);
			_canGo = true;
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			_stairsInfo.SetActive(false);
			_canGo = false;
		}
	}
}