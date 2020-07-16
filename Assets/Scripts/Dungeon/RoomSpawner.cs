using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    [SerializeField] List<Room> TopRooms;
    [SerializeField] List<Room> BottomRooms;
    [SerializeField] List<Room> LeftRooms;
    [SerializeField] List<Room> RightRooms;

    private RoomTemplate _templates;
    private Room TopRoomToSpawn;
    private Room BottomRoomToSpawn;
    private Room LeftRoomToSpawn;
    private Room RightRoomToSpawn;

    // Timer variables
    private float _timer = 0.0f;
    private float _waitTime = 0.05f;

    private bool _spawned = false;
    
    private void Start()
    {
        _timer = Time.time + _waitTime;

        _templates = GameObject.FindGameObjectWithTag("RoomsTemplate").GetComponent<RoomTemplate>();

        var random = new System.Random();

        if (TopRooms.Count > 0)
            TopRoomToSpawn = TopRooms[random.Next(TopRooms.Count)];
        if (BottomRooms.Count > 0)
            BottomRoomToSpawn = BottomRooms[random.Next(BottomRooms.Count)];
        if (LeftRooms.Count > 0)
            LeftRoomToSpawn = LeftRooms[random.Next(LeftRooms.Count)];
        if (RightRooms.Count > 0)
            RightRoomToSpawn = RightRooms[random.Next(RightRooms.Count)];
    }

    private void Update()
    {
        if ((Time.time > _timer) && !_spawned)
        {
            if (BottomRoomToSpawn != null)
                SpawnBottomRoom(BottomRoomToSpawn);
            if (TopRoomToSpawn != null)
                SpawnTopRoom(TopRoomToSpawn);
            if (LeftRoomToSpawn != null)
                SpawnLeftRoom(LeftRoomToSpawn);
            if (RightRoomToSpawn != null)
                SpawnRightRoom(RightRoomToSpawn);

            _spawned = true;
        }
    }

    private void SpawnBottomRoom(Room roomToSpawn)
    {
        int randomNum = 0;

        if (roomToSpawn.Size == 1)
        {
            // Needs to spawn a room with a BOTTOM door
            randomNum = Random.Range(0, _templates.TenByTen_BottomRooms.Length);
            Instantiate(_templates.TenByTen_BottomRooms[randomNum], roomToSpawn.transform.position, Quaternion.identity).transform.parent = this.transform;
            SpawnTenByTenSetPiece(roomToSpawn.transform.position);
        }
        else if (roomToSpawn.Size == 2)
        {
            // Needs to spawn a room with a TOP door
            randomNum = Random.Range(0, _templates.TwentyByTwenty_BottomRooms.Length);
            Instantiate(_templates.TwentyByTwenty_BottomRooms[randomNum], roomToSpawn.transform.position, Quaternion.identity).transform.parent = this.transform;
            SpawnTwentyByTwentySetPiece(roomToSpawn.transform.position);
        }
        else if (roomToSpawn.Size == 3)
        {
            // Needs to spawn a room with a LEFT door
            randomNum = Random.Range(0, _templates.ThirtyByThirty_BottomRooms.Length);
            Instantiate(_templates.ThirtyByThirty_BottomRooms[randomNum], roomToSpawn.transform.position, Quaternion.identity).transform.parent = this.transform;
        }
        else if (roomToSpawn.Size == 4)
        {
            // Needs to spawn a room with a RIGHT door
            randomNum = Random.Range(0, _templates.FortyByForty_BottomRooms.Length);
            Instantiate(_templates.FortyByForty_BottomRooms[randomNum], roomToSpawn.transform.position, Quaternion.identity).transform.parent = this.transform;
        }

        _templates.AddRoom(roomToSpawn.gameObject);
    }

    private void SpawnTopRoom(Room roomToSpawn)
    {
        int randomNum = 0;

        if (roomToSpawn.Size == 1)
        {
            // Needs to spawn a room with a BOTTOM door
            randomNum = Random.Range(0, _templates.TenByTen_TopRooms.Length);
            Instantiate(_templates.TenByTen_TopRooms[randomNum], roomToSpawn.transform.position, Quaternion.identity).transform.parent = this.transform;
            SpawnTenByTenSetPiece(roomToSpawn.transform.position);
        }
        else if (roomToSpawn.Size == 2)
        {
            // Needs to spawn a room with a TOP door
            randomNum = Random.Range(0, _templates.TwentyByTwenty_TopRooms.Length);
            Instantiate(_templates.TwentyByTwenty_TopRooms[randomNum], roomToSpawn.transform.position, Quaternion.identity).transform.parent = this.transform;
            SpawnTwentyByTwentySetPiece(roomToSpawn.transform.position);
        }
        else if (roomToSpawn.Size == 3)
        {
            // Needs to spawn a room with a LEFT door
            randomNum = Random.Range(0, _templates.ThirtyByThirty_TopRooms.Length);
            Instantiate(_templates.ThirtyByThirty_TopRooms[randomNum], roomToSpawn.transform.position, Quaternion.identity).transform.parent = this.transform;
        }
        else if (roomToSpawn.Size == 4)
        {
            // Needs to spawn a room with a RIGHT door
            randomNum = Random.Range(0, _templates.FortyByForty_TopRooms.Length);
            Instantiate(_templates.FortyByForty_TopRooms[randomNum], roomToSpawn.transform.position, Quaternion.identity).transform.parent = this.transform;
        }

        _templates.AddRoom(roomToSpawn.gameObject);
    }

    private void SpawnLeftRoom(Room roomToSpawn)
    {
        int randomNum = 0;

        if (roomToSpawn.Size == 1)
        {
            // Needs to spawn a room with a BOTTOM door
            randomNum = Random.Range(0, _templates.TenByTen_LeftRooms.Length);
            Instantiate(_templates.TenByTen_LeftRooms[randomNum], roomToSpawn.transform.position, Quaternion.identity).transform.parent = this.transform;
            SpawnTenByTenSetPiece(roomToSpawn.transform.position);
        }
        else if (roomToSpawn.Size == 2)
        {
            // Needs to spawn a room with a TOP door
            randomNum = Random.Range(0, _templates.TwentyByTwenty_LeftRooms.Length);
            Instantiate(_templates.TwentyByTwenty_LeftRooms[randomNum], roomToSpawn.transform.position, Quaternion.identity).transform.parent = this.transform;
            SpawnTwentyByTwentySetPiece(roomToSpawn.transform.position);
        }
        else if (roomToSpawn.Size == 3)
        {
            // Needs to spawn a room with a LEFT door
            randomNum = Random.Range(0, _templates.ThirtyByThirty_LeftRooms.Length);
            Instantiate(_templates.ThirtyByThirty_LeftRooms[randomNum], roomToSpawn.transform.position, Quaternion.identity).transform.parent = this.transform;
        }
        else if (roomToSpawn.Size == 4)
        {
            // Needs to spawn a room with a RIGHT door
            randomNum = Random.Range(0, _templates.FortyByForty_LeftRooms.Length);
            Instantiate(_templates.FortyByForty_LeftRooms[randomNum], roomToSpawn.transform.position, Quaternion.identity).transform.parent = this.transform;
        }

        _templates.AddRoom(roomToSpawn.gameObject);
    }

    private void SpawnRightRoom(Room roomToSpawn)
    {
        int randomNum = 0;

        if (roomToSpawn.Size == 1)
        {
            // Needs to spawn a room with a BOTTOM door
            randomNum = Random.Range(0, _templates.TenByTen_RightRooms.Length);
            Instantiate(_templates.TenByTen_RightRooms[randomNum], roomToSpawn.transform.position, Quaternion.identity).transform.parent = this.transform;
            SpawnTenByTenSetPiece(roomToSpawn.transform.position);
        }
        else if (roomToSpawn.Size == 2)
        {
            // Needs to spawn a room with a TOP door
            randomNum = Random.Range(0, _templates.TwentyByTwenty_RightRooms.Length);
            Instantiate(_templates.TwentyByTwenty_RightRooms[randomNum], roomToSpawn.transform.position, Quaternion.identity).transform.parent = this.transform;
            SpawnTwentyByTwentySetPiece(roomToSpawn.transform.position);
        }
        else if (roomToSpawn.Size == 3)
        {
            // Needs to spawn a room with a LEFT door
            randomNum = Random.Range(0, _templates.ThirtyByThirty_RightRooms.Length);
            Instantiate(_templates.ThirtyByThirty_RightRooms[randomNum], roomToSpawn.transform.position, Quaternion.identity).transform.parent = this.transform;
        }
        else if (roomToSpawn.Size == 4)
        {
            // Needs to spawn a room with a RIGHT door
            randomNum = Random.Range(0, _templates.FortyByForty_RightRooms.Length);
            Instantiate(_templates.FortyByForty_RightRooms[randomNum], roomToSpawn.transform.position, Quaternion.identity).transform.parent = this.transform;
        }

        _templates.AddRoom(roomToSpawn.gameObject);
    }

    private void SpawnTenByTenSetPiece(Vector3 location)
    {
        Instantiate(_templates.TenByTen_SetPieces[Random.Range(0, _templates.TenByTen_SetPieces.Length)], location, Quaternion.identity).transform.parent = this.transform;
    }

    private void SpawnTwentyByTwentySetPiece(Vector3 location)
    {
        Instantiate(_templates.TwentyByTwenty_SetPieces[Random.Range(0, _templates.TwentyByTwenty_SetPieces.Length)], location, Quaternion.identity).transform.parent = this.transform;
    }
}
