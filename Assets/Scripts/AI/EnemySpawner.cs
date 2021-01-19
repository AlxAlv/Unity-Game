using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    [Header("Auto Spawner Settings")]
    [SerializeField] GameObject _enemyObject;
    [SerializeField] float _spawnRate = 5.0f;
    [SerializeField] float _xOffset = 10.0f;
    [SerializeField] float _yOffset = 6.0f;
    [SerializeField] int _maxNumberOfEnemies = 5;
    [SerializeField] int _maxNumberOfTotalSpawns = 0;
    [SerializeField] bool _summonAllAtOnce = false;
    [SerializeField] List<Weapon> _possibleWeapons;

    [Header("Activated Spawner Settings")]
    [SerializeField] private bool _activatedSpawner = false;

    private float _nextSpawn = 0.0f;
    private int _currentSpawns = 0;

    void Update()
    {
        // If we're an activated spawner, we don't auto-update
	    if (_activatedSpawner)
		    return;

	    bool timeHasPassed = (Time.time > _nextSpawn);
	    bool noEnemiesExist = (transform.childCount == 0);
        bool hasNotReachedMaximumSpawns = (_currentSpawns < _maxNumberOfTotalSpawns);
        bool noMaximumSpawnLimit = (_maxNumberOfTotalSpawns == 0);
        bool maximumSpawnsAtAnyGiveTimeNotReached = (gameObject.transform.childCount < _maxNumberOfEnemies);

        if (    (timeHasPassed || noEnemiesExist || _summonAllAtOnce) 
             && (hasNotReachedMaximumSpawns || noMaximumSpawnLimit)
             &&  maximumSpawnsAtAnyGiveTimeNotReached)
        {
            _nextSpawn = Time.time + _spawnRate;

            var objectCreated = SpawnEnemy(_enemyObject);

            Weapon weaponToUse = _possibleWeapons[Random.Range(0, (_possibleWeapons.Count -1))];
            objectCreated.GetComponent<EntityWeapon>().EquipWeapon(weaponToUse);

            _currentSpawns++;
        }
    }

    private void SetLevel(GameObject enemy, int level)
    {
		UpdateLevel(enemy, level);
        enemy.GetComponent<Exp>().ExpToGive *= level;
    }

    public void UpdateLevel(GameObject enemy, int level)
    {
	    enemy.GetComponent<StatManager>().SetLevel(level);
	    enemy.GetComponent<Health>().CalculateMaxHealth();
	    enemy.GetComponent<Health>().RefillHealth();
	    enemy.GetComponent<Exp>()._currentLevel = level;
    }

    public GameObject SpawnEnemy(GameObject enemyToSpawn)
    {
        Vector2 whereToSpawn;

        RaycastHit2D hit;

        do
        {
            float randXPos = Random.Range(-_xOffset, _xOffset);
            float randYPos = Random.Range(-_yOffset, _yOffset);
            whereToSpawn = new Vector2(randXPos + transform.position.x, randYPos + transform.position.y);

            hit = Physics2D.BoxCast(whereToSpawn, enemyToSpawn.GetComponent<BoxCollider2D>().size, 0.0f, Vector2.zero, 0, LayerMask.GetMask("LevelComponents"));
        } while (hit.collider != null);
	    
        var objectCreated = Instantiate(enemyToSpawn, whereToSpawn, Quaternion.identity);
        objectCreated.transform.parent = transform;

        SetLevel(objectCreated, LevelManager.Instance.CurrentLevel);

        return objectCreated;
    }
}
