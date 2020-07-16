using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    // Needs a bottom door = 1
    // Needs a top door = 2
    // Needs a left door = 3
    // Needs a right door = 4
    [SerializeField] public int OpeningDirection;

    // 10x10 = 1
    // 20x20 = 2
    // 30x30 = 3
    // 40x40 = 4
    [SerializeField] public int Size;

    public bool Spawned = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Rooms"))
        {
            Destroy(gameObject);
        }
    }
}
