using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    [SerializeField] private float _radius = 0.5f;

    private GameObject _entityObject;
    private Transform _entityTransform;
    private Entity.EntityTypes _entityType = Entity.EntityTypes.Undefined;
    private string _currentWeaponSpritePath = "Sprites/Weapons/Equipment/Weapon_Icon";

	// Start is called before the first frame update
	void Start()
    {
        _entityObject = transform.parent.gameObject;
        _entityTransform = _entityObject.transform;

        if (_entityObject.GetComponent<Entity>())
            _entityType = _entityObject.GetComponent<Entity>().EntityType;

        UpdateWeapon(_currentWeaponSpritePath);
    }

    // Update is called once per frame
    void Update()
    {
        MoveHandAroundEntity();
    }

    void MoveHandAroundEntity()
    {
        if (_entityType == Entity.EntityTypes.Player)
        {
			Vector3 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Vector3 playerPos = _entityObject.transform.localPosition;

			Vector3 playerToCursor = cursorPos - playerPos;
			Vector3 dir = playerToCursor.normalized;
			Vector3 cursorVector = dir * _radius;

			gameObject.transform.position = (playerPos + cursorVector);

			//Debug.Log("playerToCursor = " + playerToCursor + ", dir = " + dir + ", cursorVector = " + cursorPos + ", position = " + gameObject.transform.position);
		}
    }

    void UpdateWeapon(string weaponSpritePath)
    {
        foreach (Transform child in gameObject.transform)
        {
            Destroy(child.gameObject);
        }

        _currentWeaponSpritePath = weaponSpritePath;

        GameObject weaponSpriteObj = new GameObject("WeaponSprite");

        weaponSpriteObj.transform.parent = gameObject.transform;
        weaponSpriteObj.transform.position = Vector3.zero;
        weaponSpriteObj.transform.localPosition = Vector3.zero;

        weaponSpriteObj.AddComponent<SpriteRenderer>();
        weaponSpriteObj.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(weaponSpritePath);
        weaponSpriteObj.GetComponent<SpriteRenderer>().sortingLayerName = "Player";
        weaponSpriteObj.GetComponent<SpriteRenderer>().sortingOrder = 11;
    }
}
