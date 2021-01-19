using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraFilter : Singleton<CameraFilter>
{
	public bool IsTransitioning = false;
	private const float SLOW_BLACK_SCREEN_SPEED = 1.000000005f;

	private Coroutine _currentCoroutine = null;

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

		if (_currentCoroutine != null)
			StopCoroutine(_currentCoroutine);

		_currentCoroutine = StartCoroutine(FadeAway());
	}

	public void BlackScreenFade()
	{
		Color color = Color.black;
		color.a = 0.0f;

		_image.color = color;

		if (_currentCoroutine != null)
			StopCoroutine(_currentCoroutine);

		_currentCoroutine = StartCoroutine(FadeIntoAndAway());
		IsTransitioning = true;
	}

	public void FirstHalfBlackScreenFade()
	{
		Color color = Color.black;
		color.a = 0.0f;

		_image.color = color;

		if (_currentCoroutine != null)
			StopCoroutine(_currentCoroutine);

		_currentCoroutine = StartCoroutine(FadeSlowInto());
		IsTransitioning = true;
	}

	public void SecondHalfBlackScreenFade()
	{
		if (_currentCoroutine != null)
			StopCoroutine(_currentCoroutine);

		_currentCoroutine = StartCoroutine(FadeSlowOutOf());
		IsTransitioning = true;
	}

	private IEnumerator FadeIntoAndAway()
	{
		Color color = _image.color;

		while (color.a < (1))
		{
			color.a += ((SLOW_BLACK_SCREEN_SPEED * Time.deltaTime) / 3.0f);
			_image.color = color;

			yield return null;
		}

		yield return new WaitForSeconds(1.0f);

		Color reverse = _image.color;

		while (reverse.a > (0))
		{
			reverse.a -= ((SLOW_BLACK_SCREEN_SPEED * Time.deltaTime) / 1.5f);
			_image.color = reverse;

			yield return null;
		}

		IsTransitioning = false;
	}

	private IEnumerator FadeSlowInto()
	{
		Color color = _image.color;

		while (color.a < (1))
		{
			color.a += ((SLOW_BLACK_SCREEN_SPEED * Time.deltaTime) / 3.0f);
			_image.color = color;

			yield return null;
		}

		IsTransitioning = false;
	}

	private IEnumerator FadeSlowOutOf()
	{
		Color reverse = _image.color;

		while (reverse.a > (0))
		{
			reverse.a -= ((SLOW_BLACK_SCREEN_SPEED * Time.deltaTime) / 1.5f);
			_image.color = reverse;

			yield return null;
		}

		IsTransitioning = false;
	}

	private IEnumerator FadeInto()
	{
		Color color = _image.color;

		while (color.a < 1)
		{
			color.a += _filterSpeed * Time.deltaTime;
			_image.color = color;

			yield return null;
		}
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
