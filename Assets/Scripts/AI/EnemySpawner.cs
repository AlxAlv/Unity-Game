using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject _enemyObject;
    [SerializeField] float _spawnRate = 5.0f;
    [SerializeField] float _xOffset = 10.0f;
    [SerializeField] float _yOffset = 6.0f;
    [SerializeField] int _maxNumberOfEnemies = 5;
    [SerializeField] int _maxNumberOfTotalSpawns = 0;
    [SerializeField] bool _summonAllAtOnce = false;
    [SerializeField] List<Weapon> _possibleWeapons;
    [SerializeField] private bool _arenaSpawner = false;

    private float _nextSpawn = 0.0f;
    private int _currentSpawns = 0;

    void Update()
    {
	    if (_arenaSpawner)
		    return;

        if (    ((Time.time > _nextSpawn) || (transform.childCount == 0)) 
             && ((_currentSpawns < _maxNumberOfTotalSpawns) || (_summonAllAtOnce && _currentSpawns < _maxNumberOfTotalSpawns) || (_maxNumberOfTotalSpawns == 0))
             && (gameObject.transform.childCount < _maxNumberOfEnemies))
        {
            _nextSpawn = Time.time + _spawnRate;

            var objectCreated = SpawnEnemy(_enemyObject);

            Weapon weaponToUse = _possibleWeapons[Random.Range(0, (_possibleWeapons.Count -1))];
            objectCreated.GetComponent<EntityWeapon>().MainWeapon = weaponToUse;

            _currentSpawns++;
        }
    }

    private void SetLevel(GameObject enemy)
    {
		enemy.GetComponent<StatManager>().SetLevel(LevelManager.Instance.CurrentLevel);
		enemy.GetComponent<Health>().CalculateMaxHealth();
		enemy.GetComponent<Health>().RefillHealth();
		enemy.GetComponent<Exp>()._currentLevel = LevelManager.Instance.CurrentLevel;
        enemy.GetComponent<Exp>().ExpToGive *= LevelManager.Instance.CurrentLevel;
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

        SetLevel(objectCreated);

        return objectCreated;
    }
}
