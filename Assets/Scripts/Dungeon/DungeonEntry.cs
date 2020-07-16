using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.Rendering.Universal;

public class DungeonEntry : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    // Dungeon Related
    [SerializeField] private Transform _dungeonLocation;
    [SerializeField] private GameObject _bossRoomToSpawn;
    [SerializeField] private GameObject _bossToSpawn;

    // Environment Related
    [SerializeField] private Light2D _globalLight;
    [SerializeField] private Transform _playerTransform;

    // Members
    private Vector3 _originalPlayerLocation;
    private GameObject _spawnedBoss;
    private bool _isDungeonFinished = true;

    private void Update()
    {
        HandleDungeonFinish();
    }

    private void HandleDungeonFinish()
    {
        if (!_isDungeonFinished && (_spawnedBoss == null))
        {
            _isDungeonFinished = true;
            _playerTransform.position = _originalPlayerLocation;
            _globalLight.intensity = 1.0f;

            foreach (Transform child in _dungeonLocation)
            {
                GameObject.Destroy(child.gameObject);
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //// Instantiate
        //Instantiate(_bossRoomToSpawn, _dungeonLocation.position, Quaternion.identity).transform.parent = _dungeonLocation;
        //_spawnedBoss = Instantiate(_bossToSpawn, _dungeonLocation.position, Quaternion.identity);
        //_spawnedBoss.transform.parent = _dungeonLocation;

        //// Mood
        //_globalLight.intensity = 0.7f;

        //// Save Data
        //_originalPlayerLocation = _playerTransform.position;
        //_isDungeonFinished = false;

        //// Move Player
        //_playerTransform.position = _dungeonLocation.position;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
    }

    public void OnPointerUp(PointerEventData eventData)
    {
    }
}
