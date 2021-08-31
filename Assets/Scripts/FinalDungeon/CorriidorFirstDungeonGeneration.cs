using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CorriidorFirstDungeonGeneration : SimpleRandomWalkMapGenerator
{
	[SerializeField] private int _corridorLength = 14, _corridorCount = 5;
	[SerializeField] [Range(0.1f,1)] private float _roomPercent = 0.8f;

	protected override void RunProceduralGeneration()
	{
		CorridorFirstGenerationer();
	}

	private void CorridorFirstGenerationer()
	{
		HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();
		HashSet<Vector2Int> potentialRoomPositions = new HashSet<Vector2Int>();

		CreateCorridors(floorPositions, potentialRoomPositions);

		HashSet<Vector2Int> roomPositions = CreateRooms(potentialRoomPositions);

		List<Vector2Int> deadEnds = FindAllDeadEnds(floorPositions);

		CreateRoomsAtDeadEnds(deadEnds, roomPositions);

		floorPositions.UnionWith(roomPositions);

		_tileMapVisualizer.PaintFloorTiles(floorPositions);
		WallGenerator.CreateWalls(floorPositions, _tileMapVisualizer);
	}

	private void CreateRoomsAtDeadEnds(List<Vector2Int> deadEnds, HashSet<Vector2Int> roomFloors)
	{
		foreach (var position in deadEnds)
		{
			if (roomFloors.Contains(position) == false)
			{
				var room = RunRandomWalk(_randomWalkParameters, position);
				roomFloors.UnionWith(room);
			}
		}
	}

	private List<Vector2Int> FindAllDeadEnds(HashSet<Vector2Int> floorPositions)
	{
		List<Vector2Int> deadEnds = new List<Vector2Int>();

		foreach (var position in floorPositions)
		{
			int neighboursCount = 0;

			foreach (var direction in Direction2D.CardinalDirectionsList)
			{
				if (floorPositions.Contains(position + direction))
					neighboursCount++;

			}

			if (neighboursCount == 1)
				deadEnds.Add(position);
		}

		return deadEnds;
	}

	private HashSet<Vector2Int> CreateRooms(HashSet<Vector2Int> potentialRoomPositions)
	{
		HashSet<Vector2Int> roomPositions = new HashSet<Vector2Int>();
		int roomToCreateCount = Mathf.RoundToInt(potentialRoomPositions.Count * _roomPercent);

		List<Vector2Int> roomsToCreate = potentialRoomPositions.OrderBy(x => Guid.NewGuid()).Take(roomToCreateCount).ToList();

		foreach (var roomPosition in roomsToCreate)
		{
			var roomFloor = RunRandomWalk(_randomWalkParameters, roomPosition);
			roomPositions.UnionWith(roomFloor);
		}

		return roomPositions;
	}

	private void CreateCorridors(HashSet<Vector2Int> floorPositions, HashSet<Vector2Int> potentialRoomPositions)
	{
		var currentPosition = _startPosition;
		potentialRoomPositions.Add(currentPosition);

		for (int i = 0; i < _corridorCount; i++)
		{
			var corridor = ProceduralGenerationAlgorithms.randomWalkCorridor(currentPosition, _corridorLength);
			currentPosition = corridor[corridor.Count - 1];

			potentialRoomPositions.Add(currentPosition);
			floorPositions.UnionWith(corridor);
		}
	}
}
