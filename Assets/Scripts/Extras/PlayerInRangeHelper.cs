using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInRangeHelper : MonoBehaviour
{
    [SerializeField] DialogueTrigger _dialogueTrigger;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
	        _dialogueTrigger.CanTrigger = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
	        _dialogueTrigger.CanTrigger = false;
    }
}
