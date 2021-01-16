using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ArenaManager : Singleton<ArenaManager>
{
	// Arena Constants
	private const int NUM_OF_ROUNDS = 5;
	private const int MAX_NUM_DEFEAT_ALL_ENEMIES = 6;
	private const int MAX_NUM_CLEAR_THE_ROOM_ENEMIES = 6;
	private const int MAX_NUM_DEFEAT_BOSS_BOSSES = 1;
	private const int MAX_NUM_OF_NECROMANCERS = 1;
	private const float CLEAR_THE_ROOM_TIME = 60.0f;
	private const float NECROMANCER_TIME = 90.0f;
	private const int COINS_PER_ROUND = 50;


	enum Objectives
	{
		DefeatAllEnemies,
		DefeatTheBoss,
		ClearTheRoomCountdown,
		DefeatTheNecromancer
	};

	Objectives[] ArenaObjectives = new Objectives[] { Objectives.DefeatAllEnemies, Objectives.DefeatTheBoss, Objectives.ClearTheRoomCountdown, Objectives.DefeatTheNecromancer };

    // Locations And Possible Enemies
    [SerializeField] private Transform _hubStartPosition;
	[SerializeField] private List<EnemySpawner> _enemySpawners;
	[SerializeField] private List<GameObject> _possibleEnemies;
	[SerializeField] private List<GameObject> _possibleBosses;
	[SerializeField] private List<GameObject> _possibleDungeonMasters;
	[SerializeField] private List<GameObject> _possibleNecromancers;
    [SerializeField] private List<Weapon> _possibleWeapons;

	// UI
	[SerializeField] private GameObject _objectivesPanel;
	[SerializeField] private TextMeshProUGUI _objectiveStatement;
	[SerializeField] private TextMeshProUGUI _objectiveText;
	[SerializeField] private TextMeshProUGUI _timerText;
	[SerializeField] private Image _objectiveIcon;
	[SerializeField] private Image _timerIcon;

	// Player
	[SerializeField] private GameObject _player;

	[SerializeField] private List<Transform> _validPositions;

	// Objective Information
	private List<GameObject> _spawnedEntities;

    // Arena State
    private bool _isArenaStarted = false;
    private int _currentRound = 0;
    private List<Objectives> _objectivesList;
    private float _countdownTimer;

	// Dungeon Boss Battle State
	private bool _isDungeonStarted = false;

	// Constant Strings To Display To The Player
    private Dictionary<Objectives, string> _roundStartStrings = new Dictionary<Objectives, string>()
    {
	    {Objectives.DefeatAllEnemies, "Defeat All The Enemies"},
	    {Objectives.DefeatTheBoss, "Defeat The Boss Monster"},
	    {Objectives.ClearTheRoomCountdown, "Clear The Room Before Time Runs Out"},
	    {Objectives.DefeatTheNecromancer, "Defeat The Necromancer Before Time Runs Out"}
    };

    // Start is called before the first frame update
    void Start()
    {
	    _spawnedEntities = new List<GameObject>();
	    _objectivesList = new List<Objectives>();
    }

    // Update is called once per frame
    void Update()
    {
	    _objectivesPanel.active = _isArenaStarted;

		if (_isArenaStarted)
		{
			if (_currentRound < NUM_OF_ROUNDS)
			{
				UpdateUI(_objectivesList[_currentRound]);
			}
			
			if (_currentRound == NUM_OF_ROUNDS)
            {
	            DialogManager.Instance.InstantSystemMessage("You Won!");
	            EndArena();
            }
			else if (CheckRoundStatus(_objectivesList[_currentRound]))
		    {
			    _currentRound++;

				if (_currentRound < NUM_OF_ROUNDS)
					StartRound(_objectivesList[_currentRound]);
		    }
	    }
		else if (_isDungeonStarted)
		{
			_objectivesPanel.active = true;

			UpdateUI(Objectives.DefeatTheBoss);

			if (CheckSpawnedEntities())
			{
				DialogManager.Instance.InstantSystemMessage("You Beat The Dungeon!");
				DungeonGenerator.Instance.EraseDungeon();

				EndArena();
			}
		}
    }

    // Initialize the objectives for the arena
    public void StartArena()
    {
	    // Clear Info From Last Round
		_currentRound = 0;
	    _objectivesList.Clear();

		// Randomize Objectives
		for (int i = 0; i < NUM_OF_ROUNDS; ++i)
        {
	        _objectivesList.Add(ArenaObjectives[Random.Range(0, (ArenaObjectives.Length))] );
        }

        StartRound(_objectivesList[_currentRound]);

        _isArenaStarted = true;

        SoundManager.Instance.SetArenaStatus(_isArenaStarted);
    }

    private void StartRound(Objectives objective)
    {
	    switch (objective)
	    {
            case Objectives.DefeatAllEnemies:
	            StartDefeatAllEnemiesRound();
	            break;

            case Objectives.DefeatTheBoss:
	            StartDefeatTheBossRound();
	            break;

			case Objectives.ClearTheRoomCountdown:
				StartClearTheRoomCountdown();
				break;

			case Objectives.DefeatTheNecromancer:
				StartDefeatTheNecromancer();
				break;
	    }

	    StartMessage();
    }

    private bool CheckRoundStatus(Objectives objective)
    {
	    switch (objective)
	    {
		    case Objectives.DefeatTheBoss:
			case Objectives.DefeatAllEnemies:
			    return CheckSpawnedEntities();

			case Objectives.ClearTheRoomCountdown:
			case Objectives.DefeatTheNecromancer:
				return CheckSpawnedEntitiesAndTime();

		    default:
	            return false;
	    }
    }

    private bool CheckSpawnedEntities()
    {
	    // Check for dead entities
	    _spawnedEntities.RemoveAll(item => item == null);

	    if (_spawnedEntities.Count == 0)
		    return true;

	    return false;
    }

    private bool CheckSpawnedEntitiesAndTime()
    {
		// Update Time
		_countdownTimer -= Time.deltaTime;

		// Check Time And Call End Arena If Time Ends
		if (_countdownTimer < 0)
		{
			DialogManager.Instance.InstantSystemMessage("Failed To Defeat All Enemies In Time...");
			EndArena();
		}

		return CheckSpawnedEntities();
    }


	private void StartDefeatAllEnemiesRound()
    {
		// Update UI
		_timerIcon.gameObject.SetActive(false);

	    int numberOfEnemiesThisRound = Random.Range(2, MAX_NUM_DEFEAT_ALL_ENEMIES);

	    for (int i = 0; i < numberOfEnemiesThisRound; ++i)
	    {
		    GameObject enemyToSpawn = _possibleEnemies[Random.Range(0, (_possibleEnemies.Count - 1))];
            Spawn(enemyToSpawn);
	    }
    }

    private void StartClearTheRoomCountdown()
    {
	    // Update UI
	    _timerIcon.gameObject.SetActive(true);
	    _countdownTimer = Time.deltaTime + CLEAR_THE_ROOM_TIME;

	    int numberOfEnemiesThisRound = Random.Range(2, MAX_NUM_CLEAR_THE_ROOM_ENEMIES);

	    for (int i = 0; i < numberOfEnemiesThisRound; ++i)
	    {
		    GameObject enemyToSpawn = _possibleEnemies[Random.Range(0, (_possibleEnemies.Count - 1))];
		    Spawn(enemyToSpawn);
	    }
	}

    private void StartDefeatTheNecromancer()
    {
	    // Update UI
	    _timerIcon.gameObject.SetActive(true);
	    _countdownTimer = Time.deltaTime + NECROMANCER_TIME;

	    int numberOfEnemiesThisRound = Random.Range(1, MAX_NUM_OF_NECROMANCERS);

	    for (int i = 0; i < numberOfEnemiesThisRound; ++i)
	    {
		    GameObject enemyToSpawn = _possibleNecromancers[Random.Range(0, (_possibleNecromancers.Count - 1))];
		    SpawnEnemyWithoutWeapon(enemyToSpawn);
	    }
	}

	private void StartDefeatTheBossRound()
    {
	    _timerIcon.gameObject.SetActive(false);

		int numberOfEnemiesThisRound = Random.Range(1, MAX_NUM_DEFEAT_BOSS_BOSSES);

	    for (int i = 0; i < numberOfEnemiesThisRound; ++i)
	    {
		    GameObject bossToSpawn = _possibleBosses[Random.Range(0, (_possibleBosses.Count - 1))];
		    Spawn(bossToSpawn);
	    }
    }

    private void StartMessage()
    {
		DialogManager.Instance.InstantSystemMessage("\nRound " + (_currentRound + 1) + "/" + NUM_OF_ROUNDS + " - " + _roundStartStrings[_objectivesList[_currentRound]]);
    }

    // Clear out the arena and send the player back
    private void EndArena()
    {
		if (_isDungeonStarted)
			DungeonGenerator.Instance.EraseDungeon();

	    _isArenaStarted = false;
	    _isDungeonStarted = false;

	    SoundManager.Instance.SetArenaStatus(_isArenaStarted);

	    StartCoroutine(WaitSeconds());
		CameraFilter.Instance.BlackScreenFade();

	    AwardPrize();
    }

    private void AwardPrize()
    {
	    int coinsWon = (COINS_PER_ROUND * NUM_OF_ROUNDS);

		CoinManager.Instance.AddCoins(coinsWon);
	    GoldNumbers.Create(_player.transform.position, (int)coinsWon);
	    SoundManager.Instance.Playsound("Audio/SoundEffects/VictoryFx");
	}

    // Spawn An Enemy Into The List
    private void Spawn(GameObject enemyToSpawn)
    {
	    GameObject spawnedEnemy = _enemySpawners[Random.Range(0, (_enemySpawners.Count - 1))].SpawnEnemy(enemyToSpawn);
	    Weapon weaponToUse = _possibleWeapons[Random.Range(0, (_possibleWeapons.Count - 1))];

	    // Outfit The Enemy
	    spawnedEnemy.GetComponent<EntityWeapon>().MainWeapon = weaponToUse;

	    _spawnedEntities.Add(spawnedEnemy);
    }

    private void SpawnEnemyWithoutWeapon(GameObject enemyToSpawn)
    {
	    GameObject spawnedEnemy = _enemySpawners[Random.Range(0, (_enemySpawners.Count - 1))].SpawnEnemy(enemyToSpawn);
	    _spawnedEntities.Add(spawnedEnemy);
	}

    private void UpdateUI(Objectives objective)
    {
		if (_isArenaStarted)
			_objectiveStatement.text = "(" + (_currentRound + 1) + "/" + NUM_OF_ROUNDS + ") " +  _roundStartStrings[_objectivesList[_currentRound]];
		else
			_objectiveStatement.text = "Defeat The Dungeon Master!";

		switch (objective)
		{
			case Objectives.DefeatAllEnemies:
			case Objectives.DefeatTheBoss:
				_objectiveText.text = "x" + _spawnedEntities.Count;
				break;

			case Objectives.DefeatTheNecromancer:
			case Objectives.ClearTheRoomCountdown:
				_objectiveText.text = "x" + _spawnedEntities.Count;
				_timerText.text = Mathf.RoundToInt(_countdownTimer).ToString() + " SECONDS";
				break;
		}
	}

    // Start The Dungeon Boss Room
    public void StartBossRoom()
    {
		_timerIcon.gameObject.SetActive(false);

		int numberOfEnemiesThisRound = Random.Range(1, MAX_NUM_DEFEAT_BOSS_BOSSES);

		for (int i = 0; i < numberOfEnemiesThisRound; ++i)
		{
			GameObject bossToSpawn = _possibleDungeonMasters[Random.Range(0, (_possibleDungeonMasters.Count - 1))];
			Spawn(bossToSpawn);
		}

		_player.transform.position = transform.position;
		_isDungeonStarted = true;
	}

    public void StartArenaFromButton()
    {
	    StartCoroutine(WaitForTimer());
	    CameraFilter.Instance.BlackScreenFade();
	}

    IEnumerator WaitSeconds()
    {
	    yield return new WaitForSeconds(3);

	    // Recalculate all graphs
	    AstarPath.active.Scan();

		// Move The Player And Start The Dungeon Run
		_player.transform.position = (_hubStartPosition.position);
	}

    IEnumerator WaitForTimer()
    {
	    DialogManager.Instance.InstantSystemMessage("3...");
	    yield return new WaitForSeconds(1);

	    DialogManager.Instance.InstantSystemMessage("2...");
	    yield return new WaitForSeconds(1);

	    DialogManager.Instance.InstantSystemMessage("1...");
	    yield return new WaitForSeconds(1);

	    // Recalculate all graphs
	    AstarPath.active.Scan();

	    // Move The Player And Start The Dungeon Run
	    StartArena();
	    _player.transform.position = (_validPositions[Random.Range(0, _validPositions.Count)].position);
    }
}
