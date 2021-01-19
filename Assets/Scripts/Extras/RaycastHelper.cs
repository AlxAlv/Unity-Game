using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum TypeOfObject
{
    Entity,
    LevelComponent,
    GameObject
}

public class RaycastHelper : Singleton<RaycastHelper>
{
    public GameObject GetEnemyUnderCursor()
    {
        PointerEventData pointer = new PointerEventData(EventSystem.current);
        pointer.position = Input.mousePosition;

        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointer, raycastResults);

        if (raycastResults.Count > 0)
        {
            foreach (var go in raycastResults)
            {
                if (go.gameObject.GetComponent<Entity>() != null)
                {
                    if (go.gameObject.GetComponent<Entity>().EntityType == Entity.EntityTypes.AI && !IsObjectInFrontOfOtherObject(go.gameObject))
                    {
                        return go.gameObject;
                    }
                }
                else if (go.gameObject.GetComponent<Damageable>() != null && !IsObjectInFrontOfOtherObject(go.gameObject))
                {
                    return go.gameObject;
                }
                else if (go.gameObject.GetComponent<LevelComponent>() != null && !IsObjectInFrontOfOtherObject(go.gameObject))
                {
                    return go.gameObject;
                }
            }
        }

        return null;
    }

    public bool IsObjectInFrontOfOtherObject(GameObject theGameObject)
    {
	    PointerEventData pointer = new PointerEventData(EventSystem.current);
	    pointer.position = Camera.main.WorldToScreenPoint(theGameObject.transform.position);
        Vector2 mousePosition = Input.mousePosition;

        List<RaycastResult> raycastResults = new List<RaycastResult>();
	    EventSystem.current.RaycastAll(pointer, raycastResults);

	    if (raycastResults.Count > 0 && raycastResults[0].gameObject == theGameObject)
	    {
		    return false;
	    }

	    return true;
    }

    public bool IsPlayerUnderCursor()
    {
        bool playerIsUnderCursor = false;

        PointerEventData pointer = new PointerEventData(EventSystem.current);
        pointer.position = Input.mousePosition;

        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointer, raycastResults);

        if (raycastResults.Count > 0)
        {
            foreach (var go in raycastResults)
            {
                if (go.gameObject.GetComponent<Entity>() != null)
                {
                    if (go.gameObject.GetComponent<Entity>().EntityType == Entity.EntityTypes.Player && !IsObjectInFrontOfOtherObject(go.gameObject))
                    {
                        playerIsUnderCursor = true;
                    }
                }
            }
        }

        return playerIsUnderCursor;
    }

    public TypeOfObject CheckObjectType(GameObject gameObject)
    {
        if (gameObject.GetComponent<Entity>() != null)
            return TypeOfObject.Entity;
        else if (gameObject.GetComponent<LevelComponent>())
            return TypeOfObject.LevelComponent;

        return TypeOfObject.GameObject;
    }
}
