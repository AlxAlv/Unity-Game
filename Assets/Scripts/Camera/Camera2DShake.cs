using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera2DShake : MonoBehaviour
{
	[SerializeField] private float m_shakeVibrato = .05f;
	[SerializeField] private float m_shakeRandomness = 0.005f;
	[SerializeField] private float m_shakeTime = 0.001f;

	public void Shake()
	{
		StartCoroutine(IEShake());
	}

	private IEnumerator IEShake()
	{
		Vector3 currentPosition = transform.position;

		for (int i = 0; i < m_shakeVibrato; i++)
		{
			Vector3 shakePosition = currentPosition + Random.onUnitSphere * m_shakeRandomness;
			yield return new WaitForSeconds(m_shakeTime);

			transform.position = shakePosition;
		}
	}
}
