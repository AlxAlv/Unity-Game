﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Float : MonoBehaviour
{
	// User Inputs
	public float amplitude = (0.35f);
	public float frequency = (0.75f);

	// Position Storage Variables
	Vector3 posOffset = new Vector3();
	Vector3 tempPos = new Vector3();

	// Use this for initialization
	void Start()
	{
		// Store the starting position & rotation of the object
		posOffset = transform.localPosition;
	}

	// Update is called once per frame
	void Update()
	{
		// Float up/down with a Sin()
		tempPos = posOffset;
		tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;

		transform.localPosition = tempPos;
	}
}
