using System.Collections.Generic;
using UnityEngine;

public class RoomTemplate : MonoBehaviour
{
    // 10 x 10 Rooms
    [SerializeField] public GameObject[] TenByTen_BottomRooms;
    [SerializeField] public GameObject[] TenByTen_TopRooms;
    [SerializeField] public GameObject[] TenByTen_LeftRooms;
    [SerializeField] public GameObject[] TenByTen_RightRooms;
    [SerializeField] public GameObject TenByTen_ClosedRoom;

    // 20 x 20 Rooms
    [SerializeField] public GameObject[] TwentyByTwenty_BottomRooms;
    [SerializeField] public GameObject[] TwentyByTwenty_TopRooms;
    [SerializeField] public GameObject[] TwentyByTwenty_LeftRooms;
    [SerializeField] public GameObject[] TwentyByTwenty_RightRooms;
    [SerializeField] public GameObject TwentyByTwenty_ClosedRoom;

    // 30 x 30 Rooms
    [SerializeField] public GameObject[] ThirtyByThirty_BottomRooms;
    [SerializeField] public GameObject[] ThirtyByThirty_TopRooms;
    [SerializeField] public GameObject[] ThirtyByThirty_LeftRooms;
    [SerializeField] public GameObject[] ThirtyByThirty_RightRooms;
    [SerializeField] public GameObject ThirtyByThirty_ClosedRoom;

    // 40 x 40 Rooms
    [SerializeField] public GameObject[] FortyByForty_BottomRooms;
    [SerializeField] public GameObject[] FortyByForty_TopRooms;
    [SerializeField] public GameObject[] FortyByForty_LeftRooms;
    [SerializeField] public GameObject[] FortyByForty_RightRooms;
    [SerializeField] public GameObject FortyByForty_ClosedRoom;

    // 10 x 10 SetPieces
    [SerializeField] public GameObject[] TenByTen_SetPieces;

    // 20 x 20 SetPieces
    [SerializeField] public GameObject[] TwentyByTwenty_SetPieces;

    // 30 x 30 SetPieces
    [SerializeField] public GameObject[] ThirtyByThirty_SetPieces;

    // 40 x 40 SetPieces
    [SerializeField] public GameObject[] FortyByForty_SetPieces;

    public List<GameObject> Rooms = new List<GameObject>();

    // Can use the last room in boss to spawn some boss or prefab
    public void AddRoom(GameObject room)
    {
	    if (Rooms.Count > 32)
		    return;

        Rooms.Add(room);
        Debug.Log("Current size is " + Rooms.Count);
    }
}
