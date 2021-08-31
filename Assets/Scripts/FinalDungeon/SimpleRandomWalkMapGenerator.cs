using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SimpleRandomWalkMapGenerator : AbstractDungeonGenerator
{
    [SerializeField] protected SimpleRandomWalkData _randomWalkParameters;

    protected override void RunProceduralGeneration()
    {
        HashSet<Vector2Int> floorPositions = RunRandomWalk(_randomWalkParameters, _startPosition);

        _tileMapVisualizer.Clear();
        _tileMapVisualizer.PaintFloorTiles(floorPositions);
        WallGenerator.CreateWalls(floorPositions, _tileMapVisualizer);
    }

	protected HashSet<Vector2Int> RunRandomWalk(SimpleRandomWalkData randomWalkParameters, Vector2Int position)
	{
        var currentPosition = position;
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();

		for (int i = 0; i < randomWalkParameters.Iterations; i++)
		{
            var path = ProceduralGenerationAlgorithms.SimpleRandomWalk(currentPosition, randomWalkParameters.WalkLength);
            floorPositions.UnionWith(path);

            if (randomWalkParameters.StartRandomlyEachIteration)
                currentPosition = floorPositions.ElementAt(UnityEngine.Random.Range(0, floorPositions.Count));
		}

        return floorPositions;
	}
}
