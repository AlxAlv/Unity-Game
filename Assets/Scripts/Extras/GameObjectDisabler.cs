using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectDisabler : MonoBehaviour
{
    private void Update()
    {
        this.gameObject.SetActive(false);
    }
}
