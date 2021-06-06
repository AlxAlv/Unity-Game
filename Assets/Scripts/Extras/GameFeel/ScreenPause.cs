using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenPause : Singleton<ScreenPause>
{
	//private const float THE_DEFAULT_DURATION = 0.085f;
	private const float DEFAULT_DURATION = 0.045f;
	private float duration = DEFAULT_DURATION;
	private float _pendingFreezeDuration = 0.0f;
	private bool _isFrozen = false;

    // Update is called once per frame
    void Update()
    {
	    if ((_pendingFreezeDuration > 0) && !_isFrozen)
	    {
		    StartCoroutine(FreezeScreen());
	    }
    }

    public void Freeze(float duration = DEFAULT_DURATION)
    {
	    _pendingFreezeDuration = duration;
    }

    IEnumerator FreezeScreen()
    {
	    _isFrozen = true;

	    float originalScale = Time.timeScale;
	    Time.timeScale = 0.0f;

	    yield return new WaitForSecondsRealtime(duration);

	    Time.timeScale = originalScale;
	    _pendingFreezeDuration = 0.0f;
	    _isFrozen = false;
	}
}
