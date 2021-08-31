using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WallGenerator
{
	public static void CreateWalls(HashSet<Vector2Int> floorPositions, TileMapVisualizer tileMapVisualizer)
	{
		var basicWallPositions = FindWallsInDirections(floorPositions, Direction2D.CardinalDirectionsList);
		var cornerWallPositions = FindWallsInDirections(floorPositions, Direction2D.DiagonalDirectionsList);

		CreateBasicWalls(tileMapVisualizer, basicWallPositions, floorPositions);
		CreateCornerWalls(tileMapVisualizer, cornerWallPositions, floorPositions);
	}

	private static void CreateCornerWalls(TileMapVisualizer tileMapVisualizer, HashSet<Vector2Int> cornerWallPositions, HashSet<Vector2Int> floorPositions)
	{
		foreach (var position in cornerWallPositions)
		{
			string neighboursBinaryValue = "";

			foreach (var direction in Direction2D.EightDirectionList)
			{
				var neighbourPosition = position + direction;
				if (floorPositions.Contains(neighbourPosition))
					neighboursBinaryValue += "1";
				else
					neighboursBinaryValue += "0";
			}
			tileMapVisualizer.PaintSingleCornerWall(position, neighboursBinaryValue);
		}
	}

	private static void CreateBasicWalls(TileMapVisualizer tileMapVisualizer, HashSet<Vector2Int> basicWallPositions, HashSet<Vector2Int> floorPositions)
	{
		foreach (var position in basicWallPositions)
		{
			string neighboursBinaryValue = "";

			foreach (var direction in Direction2D.CardinalDirectionsList)
			{
				var neighbourPosition = position + direction;
				if (floorPositions.Contains(neighbourPosition))
					neighboursBinaryValue += "1";
				else
					neighboursBinaryValue += "0";
			}
			tileMapVisualizer.PaintSingleBasicWall(position, neighboursBinaryValue);
		}
	}

	private static HashSet<Vector2Int> FindWallsInDirections(HashSet<Vector2Int> floorPositions, List<Vector2Int> directionList)
	{
		HashSet<Vector2Int> wallPositions = new HashSet<Vector2Int>();

		foreach (var position in floorPositions)
		{
			foreach (var direction in directionList)
			{
				var neighbourPosition = position + direction;

				if (floorPositions.Contains(neighbourPosition) == false)
					wallPositions.Add(neighbourPosition);
			}
		}

		return wallPositions;
	}
}
