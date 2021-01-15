using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRoom : MonoBehaviour
{
	[SerializeField] private LayerMask _roomMask;
	[SerializeField] private DungeonGenerator _dungeonGenerator;
	[SerializeField] private Transform _dungeonRoomsTransform;

	private bool _hasSpawnedARoom = false;

	// Update is called once per frame
    void Update()
    {
	    Collider2D roomDetection = Physics2D.OverlapCircle(transform.position, 1, _roomMask);

	    if (roomDetection == null && _dungeonGenerator.StopGeneration && _dungeonGenerator.StartedGeneration && !_hasSpawnedARoom)
	    {
			// Spawn Random Room
			int rand = Random.Range(0, _dungeonGenerator.Rooms.Length);
			GameObject newRoom = Instantiate(_dungeonGenerator.Rooms[rand], transform.position, Quaternion.identity);
			newRoom.transform.parent = _dungeonRoomsTransform;

			// Spawn Random Event
			int randEvent = Random.Range(0, _dungeonGenerator.EventSpawners.Count);
		    MaybeSpawnObject newEvent = Instantiate(_dungeonGenerator.EventSpawners[randEvent], transform.position, Quaternion.identity);
			newEvent.transform.parent = newRoom.transform;

			// Spawn Random Flooring
			int randFlooring = Random.Range(0, _dungeonGenerator.PossibleTiles.Count);
			SpawnObject newFlooring = Instantiate(_dungeonGenerator.PossibleTiles[randFlooring], transform.position, Quaternion.identity);
			newFlooring.transform.parent = newRoom.transform;

			_hasSpawnedARoom = true;
	    }
    }

    public void ResetObject()
    {
	    _hasSpawnedARoom = false;
    }
}
