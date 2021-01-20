using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnclosedRoom : MonoBehaviour
{
    // Enclosing Walls
	[SerializeField] private GameObject _walls;
	[SerializeField] private EnemySpawner _enemySpawner;
	[SerializeField] private List<Weapon> _possibleWeapons;
	[SerializeField] private GameObject enemyToSpawn;
	[SerializeField] private int _numberOfWaves = 3;

	// Objective Information
	private List<GameObject> _spawnedEntities;
	private bool _isStarted = false;

	private int _currentRoom = 1;

    // Start is called before the first frame update
    void Start()
    {
	    _spawnedEntities = new List<GameObject>();
		_isStarted = false;
    }

    // Update is called once per frame
    void Update()
    {
	    // Check for dead entities
	    _spawnedEntities.RemoveAll(item => item == null);

		if (_isStarted)
	    {
		    if (_spawnedEntities.Count == 0)
		    {
			    _currentRoom++;

				if (_currentRoom <= _numberOfWaves)
					StartWave();
			}

			if (_currentRoom > _numberOfWaves)
			{
				SoundManager.Instance.Playsound("Audio/SoundEffects/OpeningRoom");
				Destroy(gameObject);
			}
	    }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
	    if (other.CompareTag("Player") && !_isStarted)
	    {
		    _walls.gameObject.SetActive(true);
		    _enemySpawner.gameObject.SetActive(true);

			DialogManager.Instance.InstantSystemMessage("Ambushed!\nDefeat " + _numberOfWaves + " Waves Of Enemies!");
			SoundManager.Instance.Playsound("Audio/SoundEffects/ClosingRoom");
			_isStarted = true;

			StartWave();

			// Recalculate all graphs
			AstarPath.active.Scan();
		}
    }

    private void StartWave()
    {
	    for (int i = 0; i < Random.Range(2, 4); ++i)
		    SpawnEnemy();
	}

    private void SpawnEnemy()
    {
	    GameObject spawnedEnemy = _enemySpawner.SpawnEnemy(enemyToSpawn);
	    Weapon weaponToUse = _possibleWeapons[Random.Range(0, (_possibleWeapons.Count - 1))];

	    // Outfit The Enemy
	    spawnedEnemy.GetComponent<EntityWeapon>().EquipWeapon(weaponToUse);

	    _spawnedEntities.Add(spawnedEnemy);
	}
}
