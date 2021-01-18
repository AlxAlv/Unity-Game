using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsHeld : MonoBehaviour
{
	[SerializeField] private SpriteRenderer _renderer;
	[SerializeField] private GameObject _floatingWeaponObject;

	private EntityWeapon _entityWeapon;

	void Start()
	{
		_entityWeapon = GetComponent<EntityWeapon>();
	}

    public void UpdateWeapons()
    {
		// Delete all the sprite renderer children
		foreach (Transform child in _renderer.transform)
			GameObject.Destroy(child.gameObject);

		bool isFacingLeft = GetComponent<EntityFlip>().m_FacingLeft;

		if (_entityWeapon.CurrentOffHand != null)
		{
			GameObject floatingWeapon = Instantiate(_floatingWeaponObject, transform.position, Quaternion.identity);
			floatingWeapon.transform.parent = _renderer.transform;

			Transform modelTransform = _entityWeapon.CurrentOffHand.transform.Find("Model");
			GameObject modelObject = modelTransform.gameObject;

			floatingWeapon.GetComponent<SpriteRenderer>().sprite = modelObject.GetComponent<SpriteRenderer>().sprite;

			// Rotate to -45
			floatingWeapon.transform.localPosition = new Vector3((_renderer.bounds.size.x / 2), 0.0f, 0.0f);
			floatingWeapon.transform.Rotate(Vector3.forward * (isFacingLeft ? -45.0f : 45.0f));
		}

		if (_entityWeapon.CurrentWeapon != null)
		{
			GameObject floatingWeapon = Instantiate(_floatingWeaponObject, transform.position, Quaternion.identity);
			floatingWeapon.transform.parent = _renderer.transform;

			Transform modelTransform = _entityWeapon.CurrentWeapon.transform.Find("Model");
			GameObject modelObject = modelTransform.gameObject;

			floatingWeapon.GetComponent<SpriteRenderer>().sprite = modelObject.GetComponent<SpriteRenderer>().sprite;

			floatingWeapon.transform.localPosition = new Vector3(((_renderer.bounds.size.x / 2) + 0.25f), 0.0f, 0.0f);
			floatingWeapon.transform.Rotate(Vector3.forward * (isFacingLeft ? -45.0f : 45.0f));
		}
    }
}
