using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractDungeonGenerator : MonoBehaviour
{
	[SerializeField] protected TileMapVisualizer _tileMapVisualizer = null;
	[SerializeField] protected Vector2Int _startPosition = Vector2Int.zero;

	public void GenerateDungeon()
	{
		_tileMapVisualizer.Clear();
		RunProceduralGeneration();
	}

	protected abstract void RunProceduralGeneration();
}
