using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraFilter : Singleton<CameraFilter>
{
	private Image _image = null;
	private float _filterSpeed = 1.10f;

	void Start()
	{
		_image = GetComponent<Image>();
	}

	public void Flash(Color color, float speed, float startingAlpha)
	{
		_filterSpeed = speed;

		color.a = startingAlpha;
		_image.color = color;

		StartCoroutine(FadeAway());
	}

	private IEnumerator FadeAway()
	{
		Color color = _image.color;

		while (color.a > 0)
		{
			color.a -= _filterSpeed * Time.deltaTime;
			_image.color = color;

			yield return null;
		}
	}
}
