using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBarOpen : Singleton<UIBarOpen>
{
	private const float DEFAULT_TIME = (0.5f);

	public void OpenUpBar(GameObject theGameObject, float time = DEFAULT_TIME)
	{
		theGameObject.transform.localScale = new Vector3(0, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
		StartCoroutine(ScaleUpAnimation(theGameObject, time));
	}

	IEnumerator ScaleUpAnimation(GameObject theGameObject, float time)
	{
		float currentTime = 0f;
		float startValue = 0f;
		float endValue = 1.0f;
		float TARGET;


		while (theGameObject != null && theGameObject.transform.localScale.x < 1)
		{ 
			currentTime += Time.deltaTime;
			TARGET = Mathf.Lerp(startValue, endValue, currentTime / time);
			theGameObject.transform.localScale = new Vector3(TARGET, theGameObject.transform.localScale.y, theGameObject.transform.localScale.z);
			yield return 0;
		}
	}
}
