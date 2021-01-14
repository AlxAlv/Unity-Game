using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class UIBounce : Singleton<UIBounce>
{
	private float _timer = 0.0f;
	private float _timeToJump = 0.10f;
	private float _distanceToBeJump = 15.0f;

	public void BounceUI(GameObject UIObject, float distanceToJump = 10.0f, float timeToJump = 0.08f)
	{
		_distanceToBeJump = distanceToJump;
		_timeToJump = timeToJump;

		StartCoroutine(Bounce(UIObject));
	}

	IEnumerator Bounce(GameObject UIObject)
	{
		Vector3 originalPosition = UIObject.transform.localPosition;

		Vector3 destinationPosition = new Vector3(UIObject.transform.localPosition.x, UIObject.transform.localPosition.y + _distanceToBeJump, UIObject.transform.localPosition.z);


		Vector3 currentPosition = new Vector3(0, 0, 0);

		_timer = 0.0f;


		while (_timer < _timeToJump)
		{
			currentPosition = Vector2.Lerp(originalPosition, destinationPosition, (_timer / _timeToJump));
			UIObject.transform.localPosition = currentPosition;

			_timer += Time.deltaTime;

			yield return null;
		}

		_timer = 0.0f;
		_timeToJump /= 2;

		while (_timer < _timeToJump)
		{
			currentPosition = Vector2.Lerp(destinationPosition, originalPosition, (_timer / _timeToJump));
			UIObject.transform.localPosition = currentPosition;

			_timer += Time.deltaTime;

			yield return null;
		}

		UIObject.transform.localPosition = originalPosition;
	}
}
