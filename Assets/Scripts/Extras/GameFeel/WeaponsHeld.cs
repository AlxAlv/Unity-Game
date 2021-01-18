using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsHeld : MonoBehaviour
{
	[SerializeField] private SpriteRenderer _renderer;
	[SerializeField] private GameObject _floatingWeaponObject;

    public void UpdateWeapons(EntityWeapon entityWeapon)
    {
		// Delete all the sprite renderer children
		foreach (Transform child in _renderer.transform)
			GameObject.Destroy(child.gameObject);

		bool isFacingLeft = GetComponent<EntityFlip>().m_FacingLeft;

		if (entityWeapon.CurrentOffHandWeapon != null)
		{
			GameObject floatingWeapon = Instantiate(_floatingWeaponObject, transform.position, Quaternion.identity);
			floatingWeapon.transform.parent = _renderer.transform;

			Transform modelTransform = entityWeapon.CurrentOffHandWeapon.transform.Find("Model");
			GameObject modelObject = modelTransform.gameObject;

			floatingWeapon.GetComponent<SpriteRenderer>().sprite = modelObject.GetComponent<SpriteRenderer>().sprite;
			floatingWeapon.GetComponent<SpriteRenderer>().color = modelObject.GetComponent<SpriteRenderer>().color;

			// Rotate to -45
			floatingWeapon.transform.localPosition = new Vector3((_renderer.bounds.size.x / 2), 0.0f, 0.0f);
			floatingWeapon.transform.Rotate(Vector3.forward * (isFacingLeft ? -45.0f : 45.0f));
		}

		if (entityWeapon.CurrentWeapon != null)
		{
			GameObject floatingWeapon = Instantiate(_floatingWeaponObject, transform.position, Quaternion.identity);
			floatingWeapon.transform.parent = _renderer.transform;

			Transform modelTransform = entityWeapon.CurrentWeapon.transform.Find("Model");
			GameObject modelObject = modelTransform.gameObject;

			floatingWeapon.GetComponent<SpriteRenderer>().sprite = modelObject.GetComponent<SpriteRenderer>().sprite;
			floatingWeapon.GetComponent<SpriteRenderer>().color = modelObject.GetComponent<SpriteRenderer>().color;

			floatingWeapon.transform.localPosition = new Vector3(((_renderer.bounds.size.x / 2) + 0.25f), 0.0f, 0.0f);
			floatingWeapon.transform.Rotate(Vector3.forward * (isFacingLeft ? -45.0f : 45.0f));
		}
    }
}
