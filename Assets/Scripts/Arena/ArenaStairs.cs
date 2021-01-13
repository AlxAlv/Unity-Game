using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaStairs : MonoBehaviour
{
	[SerializeField] private List<Transform> _validPositions;
	[SerializeField] private GameObject _player;
	[SerializeField] private GameObject _toolTip;
	[SerializeField] private ArenaManager _arenaManager;

	private bool _canGo;

	void Update()
	{
		if (_canGo && Input.GetKeyDown(KeyCode.F))
		{
			_player.transform.position = (_validPositions[Random.Range(0, _validPositions.Count)].position);
			_arenaManager.StartArena(_player);
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			_toolTip.SetActive(true);
			_canGo = true;
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			_toolTip.SetActive(false);
			_canGo = false;
		}
	}
}
