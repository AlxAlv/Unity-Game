using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomType : MonoBehaviour
{
	[SerializeField] public int Type;

	public void RoomDestruction()
	{
		Destroy(gameObject);
	}
}
