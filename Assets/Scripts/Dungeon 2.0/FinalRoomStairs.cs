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
			StartCoroutine(WaitForTimer());
			CameraFilter.Instance.BlackScreenFade();
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

	IEnumerator WaitForTimer()
	{
		DialogManager.Instance.InstantSystemMessage("3...");
		yield return new WaitForSeconds(1);

		DialogManager.Instance.InstantSystemMessage("2...");
		yield return new WaitForSeconds(1);

		DialogManager.Instance.InstantSystemMessage("1...");
		yield return new WaitForSeconds(1);

		// Recalculate all graphs
		AstarPath.active.Scan();

		// Move The Player And Start The Boss Room
		ArenaManager.Instance.StartBossRoom();
		SoundManager.Instance.SetDungeonMasterStatus(true);
	}
}