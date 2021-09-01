using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractDungeonGenerator : MonoBehaviour
{
	[SerializeField] protected TileMapVisualizer _tileMapVisualizer = null;
	[SerializeField] protected Vector2Int _startPosition = Vector2Int.zero;
	[SerializeField] protected Grid _gridObject = null;

	public void GenerateDungeon()
	{
		_tileMapVisualizer.Clear();
		RunProceduralGeneration();
	}

	public void ClearDungeon()
	{
		_tileMapVisualizer.Clear();
	}

	protected abstract void RunProceduralGeneration();
}
