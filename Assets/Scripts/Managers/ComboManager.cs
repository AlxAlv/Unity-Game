using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboManager : Singleton<ComboManager>
{
	public int ComboCount { get; set; }

	private float _comboTimer = 0.0f;
	private float _comboDuration = 3.0f;

	private void Update()
	{
		if (_comboTimer < _comboDuration)
		{
			_comboTimer += Time.deltaTime;
		}
		else
		{
			ComboCount = 0;
		}
	}

	private void Start()
	{
		ComboCount = 0;
	}

	public void IncrementCounter()
	{
		++ComboCount;
		_comboTimer = 0.0f;
	}
}
