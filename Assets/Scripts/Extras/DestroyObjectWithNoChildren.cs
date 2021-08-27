using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObjectWithNoChildren : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (gameObject.transform.childCount == 0)
        {
            Object.Destroy(gameObject);
        }
    }
}
