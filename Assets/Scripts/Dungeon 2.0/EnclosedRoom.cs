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

	// Objective Information
	private List<GameObject> _spawnedEntities;
	private bool _isStarted = false;

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

		if (_isStarted && _spawnedEntities.Count == 0)
	    {
			Destroy(gameObject);
	    }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
	    if (other.CompareTag("Player") && !_isStarted)
	    {
		    _walls.gameObject.SetActive(true);
		    _enemySpawner.gameObject.SetActive(true);

			DialogManager.Instance.InstantSystemMessage("Ambushed!\nFight your way out of the room!");
		    _isStarted = true;

			for (int i = 0; i < Random.Range(2,4); ++i)
				SpawnEnemy();

			// Recalculate all graphs
			AstarPath.active.Scan();
		}
    }

    private void SpawnEnemy()
    {
	    GameObject spawnedEnemy = _enemySpawner.SpawnEnemy(enemyToSpawn);
	    Weapon weaponToUse = _possibleWeapons[Random.Range(0, (_possibleWeapons.Count - 1))];

	    // Outfit The Enemy
	    spawnedEnemy.GetComponent<EntityWeapon>().MainWeapon = weaponToUse;

	    _spawnedEntities.Add(spawnedEnemy);
	}
}
