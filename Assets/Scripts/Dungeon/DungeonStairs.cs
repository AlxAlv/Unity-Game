using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonStairs : MonoBehaviour
{
    [SerializeField] private List<Transform> _validPositions;
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _chestInfo;

    private bool _canGo;

    void Update()
    {
        if (_canGo && Input.GetKeyDown(KeyCode.F))
        {
            _player.transform.position = (_validPositions[Random.Range(0, _validPositions.Count)].position);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _chestInfo.SetActive(true);
            _canGo = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _chestInfo.SetActive(false);
            _canGo = false;
        }
    }
}
