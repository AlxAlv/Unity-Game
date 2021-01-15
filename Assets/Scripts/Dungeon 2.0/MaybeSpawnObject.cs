using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaybeSpawnObject : MonoBehaviour
{
	public int ChanceDivider = 4;
	public GameObject[] objects;

	void Start()
	{
		int rand = Random.Range(0, objects.Length * ChanceDivider);

		if (rand < objects.Length)
		{
			GameObject spawnedObject = Instantiate((objects[rand]), transform.position, Quaternion.identity);
			spawnedObject.transform.parent = transform;
		}
	}
}
