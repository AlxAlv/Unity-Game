using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderHelper : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {

        // Forward to the parent (or just deal with it here).
        // Let's say it has a script called "PlayerCollisionHelper" on it:
        //MeleeWeapon parentScript = transform.parent.GetComponent<MeleeWeapon>();

        // Let it know a collision happened:
        //parentScript.CollisionFromChild(collision);

        Debug.Log("Hit a wall. Would report here?");
    }
}
