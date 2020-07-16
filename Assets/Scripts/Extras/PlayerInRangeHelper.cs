using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInRangeHelper : MonoBehaviour
{
    [SerializeField] Harvestable _harvestable;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            _harvestable.PlayerIsInRange = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            _harvestable.PlayerIsInRange = false;
    }
}
