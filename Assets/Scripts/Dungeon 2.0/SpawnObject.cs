using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : MonoBehaviour
{
	public GameObject[] objects;

	void Start()
    {
	    int rand = Random.Range(0, objects.Length);

	    GameObject spawnedObject = Instantiate((objects[rand]), transform.position, Quaternion.identity);

	    spawnedObject.transform.parent = transform;
    }
}
