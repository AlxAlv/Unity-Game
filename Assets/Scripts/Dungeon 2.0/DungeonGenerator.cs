using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class DungeonGenerator : Singleton<DungeonGenerator>
{
	[SerializeField] public Transform PlayerTransform;
	[SerializeField] public int XUnitsPerRoom;
	[SerializeField] public int YUnitsPerRoom;
	[SerializeField] public float MinX;
	[SerializeField] public float MaxX;
	[SerializeField] public float MinY;
	[SerializeField] public Transform DungeonObjects;
	[SerializeField] public Transform DungeonPositions;
	[SerializeField] public LayerMask Room;
	[SerializeField] public List<MaybeSpawnObject> EventSpawners;
	[SerializeField] public List<SpawnObject> PossibleTiles;
	[SerializeField] public GameObject FinalRoom;
	[SerializeField] public Transform WaitingPosition;

	public Transform[] StartingPositions;
	public GameObject[] Rooms; // Index 0 --> LR, Index 1 --> LRB, Index 2 --> LRT, Index 3 --> LRBT

	public bool StartedGeneration = false;
	public bool StopGeneration = true;
	private bool _goingDown = false;

	private int _direction;
	private float _timeBetweenRoom;
	private float _startTimeBetweenRoom = 0.0f;

	private Vector3 _playerOriginalPosition;
	private Vector3 _beginningRoom;
	private Vector3 _finalRoom;

	public void StartDungeon()
	{
		if (_goingDown)
			StartCoroutine(WaitOneSecond());

		int randStartingPos = Random.Range(0, StartingPositions.Length);
		transform.position = StartingPositions[randStartingPos].position;

		GameObject newRoom = Instantiate(Rooms[0], transform.position, Quaternion.identity);
		newRoom.transform.parent = DungeonObjects;

		_direction = Random.Range(1, 6);
		_beginningRoom = transform.position;

		StartedGeneration = true;
		StopGeneration = false;

		// Flooring
		int randFloor = Random.Range(0, PossibleTiles.Count);
		SpawnObject newFlooring = Instantiate(PossibleTiles[randFloor], transform.position, Quaternion.identity);
		newFlooring.transform.parent = newRoom.transform;

		SoundManager.Instance.SetDungeonStatus(StartedGeneration);
	}

	public void EraseDungeon()
	{
		StartedGeneration = false;
		StopGeneration = true;

		// Erase All Dungeon Rooms
		foreach (Transform child in DungeonObjects.transform)
		{
			GameObject.Destroy(child.gameObject);
		}

		// Reset All Dungeon Positions
		foreach (Transform child in DungeonPositions.transform)
		{
			child.GetComponent<SpawnRoom>().ResetObject();
		}

		SoundManager.Instance.SetDungeonMasterStatus(false);
		SoundManager.Instance.SetDungeonStatus(false);
	}

	public void NextFloor()
	{
		StartedGeneration = false;
		StopGeneration = true;
		_goingDown = true;

		// Erase All Dungeon Rooms
		foreach (Transform child in DungeonObjects.transform)
		{
			GameObject.Destroy(child.gameObject);
		}

		// Reset All Dungeon Positions
		foreach (Transform child in DungeonPositions.transform)
		{
			child.GetComponent<SpawnRoom>().ResetObject();
		}

		PlayerTransform.position = WaitingPosition.position;
		StartCoroutine(WaitForFloorGeneration());
		CameraFilter.Instance.FirstHalfBlackScreenFade();
	}

	private void Update()
    {
	    if (_timeBetweenRoom <= 0 && !StopGeneration)
	    {
		    Move();
		    _timeBetweenRoom = _startTimeBetweenRoom;
	    }
	    else
	    {
		    _timeBetweenRoom -= Time.deltaTime;
	    }

	    if (Input.GetKeyDown(KeyCode.B) && _finalRoom != null && Application.isEditor)
	    {
		    PlayerTransform.position = _finalRoom;
	    }
    }

    private void SpawnExtras(GameObject parentRoom)
    {
		// Event
	    int rand = Random.Range(0, EventSpawners.Count);
	    MaybeSpawnObject newRoom = Instantiate(EventSpawners[rand], transform.position, Quaternion.identity);
	    newRoom.transform.parent = parentRoom.transform;

		// Flooring
		int randFloor = Random.Range(0, PossibleTiles.Count);
		SpawnObject newFlooring = Instantiate(PossibleTiles[randFloor], transform.position, Quaternion.identity);
		newFlooring.transform.parent = parentRoom.transform;
	}

    private void Move()
    {
	    if (_direction == 1 || _direction == 2) // Move Right
	    {
		    if (transform.position.x < MaxX)
		    {
			    Vector2 newPos = new Vector2(transform.position.x + XUnitsPerRoom, transform.position.y);
			    transform.position = newPos;

			    int rand = Random.Range(0, Rooms.Length);
			    GameObject newRoom = Instantiate(Rooms[rand], transform.position, Quaternion.identity);
			    newRoom.transform.parent = DungeonObjects;

				//Debug.Log("(RIGHT) Creating room " + newRoom.name + " at " + transform.position.x + "," + transform.position.y);

			    SpawnExtras(newRoom);

				_direction = Random.Range(1, 6);

			    if (_direction == 3)
				    _direction = 2;
				else if (_direction == 4)
				    _direction = 5;
		    }
		    else
		    {
			    _direction = 5;
		    }
	    }
		else if (_direction == 3 || _direction == 4) // Move Left
	    {
		    if (transform.position.x > MinX)
		    {
			    Vector2 newPos = new Vector2(transform.position.x - XUnitsPerRoom, transform.position.y);
			    transform.position = newPos;

			    int rand = Random.Range(0, Rooms.Length);
			    GameObject newRoom = Instantiate(Rooms[rand], transform.position, Quaternion.identity);
			    newRoom.transform.parent = DungeonObjects;

			    //Debug.Log("(LEFT) Creating room " + newRoom.name + " at " + transform.position.x + "," + transform.position.y);

				SpawnExtras(newRoom);

				_direction = Random.Range(3, 5);
		    }
		    else
		    {
			    _direction = 5;
		    }
	    }
		else if (_direction == 5) // Move Down
	    {
		    if (transform.position.y > MinY)
		    {
				// Make sure the room right above has an opening!
				Collider2D roomDetection = Physics2D.OverlapCircle(transform.position, 1, Room);

				if (roomDetection.GetComponent<RoomType>().Type != 1 &&
				    roomDetection.GetComponent<RoomType>().Type != 3)
				{
					roomDetection.GetComponent<RoomType>().RoomDestruction();


					GameObject newBottomRoom = Instantiate(Rooms[3], transform.position, Quaternion.identity);
					newBottomRoom.transform.parent = DungeonObjects;

					//Debug.Log("(DOWN) Creating room to replace above room " + newBottomRoom.name + " at " + transform.position.x + "," + transform.position.y);
					SpawnExtras(newBottomRoom);
				}
				else
				{
					//Debug.Log("(DOWN) Not creating a new room!");
				}

			    Vector2 newPos = new Vector2(transform.position.x, transform.position.y - YUnitsPerRoom);
			    transform.position = newPos;

			    int rand = Random.Range(2, 4);
			    GameObject newRoom = Instantiate(Rooms[rand], transform.position, Quaternion.identity);
			    newRoom.transform.parent = DungeonObjects;
			    //Debug.Log("(LEFT) Creating room " + newRoom.name + " at " + transform.position.x + "," + transform.position.y);

				SpawnExtras(newRoom);

				_direction = Random.Range(1, 6);
		    }
		    else
		    {
				// Replace The Final Room With Exit Portal
				Destroy(DungeonObjects.GetChild(DungeonObjects.childCount - 1).gameObject);

				// Spawn Final Room
				GameObject newRoom = Instantiate(FinalRoom, transform.position, Quaternion.identity);
				newRoom.transform.parent = DungeonObjects;

				// Flooring
				int randFloor = Random.Range(0, PossibleTiles.Count);
				SpawnObject newFlooring = Instantiate(PossibleTiles[randFloor], transform.position, Quaternion.identity);
				newFlooring.transform.parent = newRoom.transform;

				_finalRoom = newRoom.transform.position;

				// Stop Level Generator
				StopGeneration = true;

				if (!_goingDown)
				{
					StartCoroutine(WaitForDungeonGeneration());
					CameraFilter.Instance.BlackScreenFade();
				}
		    }
	    }
    }

    IEnumerator WaitForDungeonGeneration()
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
		FindPositionForPlayer();
    }

    IEnumerator WaitForFloorGeneration()
    {
	    DialogManager.Instance.InstantSystemMessage("3...");
	    yield return new WaitForSeconds(1);

	    DialogManager.Instance.InstantSystemMessage("2...");
	    yield return new WaitForSeconds(1);

	    DialogManager.Instance.InstantSystemMessage("1...");
	    yield return new WaitForSeconds(1);

	    // Move The Player And Start The Dungeon Run
	    FindPositionForPlayer();
		StartDungeon();
    }

    IEnumerator WaitOneSecond()
    {
	    yield return new WaitForSeconds(1);

	    CameraFilter.Instance.SecondHalfBlackScreenFade();

		// Recalculate all graphs
		AstarPath.active.Scan();

	    // Move The Player And Start The Dungeon Run
	    FindPositionForPlayer();
	}

	private void FindPositionForPlayer()
    {
	    Vector2 whereToSpawn;
		RaycastHit2D hit;

	    do
	    {
		    float randXPos = Random.Range(-5, 5);
		    float randYPos = Random.Range(-5, 5);
		    whereToSpawn = new Vector2(randXPos + _beginningRoom.x, randYPos + _beginningRoom.y);

		    hit = Physics2D.BoxCast(whereToSpawn, PlayerTransform.GetComponent<BoxCollider2D>().size, 0.0f, Vector2.zero, 0, LayerMask.GetMask("LevelComponents"));
	    } while (hit.collider != null);

	    _playerOriginalPosition = PlayerTransform.position;
	    PlayerTransform.position = whereToSpawn;
	}
}
