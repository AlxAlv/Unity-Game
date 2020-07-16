using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPoolCleanup : MonoBehaviour
{
    public const string OprhanedPoolName = "OrphanedPooler";

    private void Update()
    {
        FindAndCleanup();
    }

    private void FindAndCleanup()
    {
        foreach (var gameObj in FindObjectsOfType(typeof(GameObject)) as GameObject[])
        {
            if (gameObj.name == OprhanedPoolName)
            {
                bool canDelete = true;

                foreach (Projectile child in gameObj.GetComponentsInChildren<Projectile>())
                {
                    if (child.gameObject.activeSelf == true)
                    {
                        canDelete = false;
                    }
                }

                if (canDelete)
                {
                    Destroy(gameObj);
                }
            }
        }
    }
}
