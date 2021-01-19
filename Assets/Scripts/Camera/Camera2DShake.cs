using System.Collections;
using UnityEngine;

public class Camera2DShake : Singleton<Camera2DShake>
{
	[SerializeField] private float m_shakeVibrato = 0.2f;
	[SerializeField] private float m_shakeRandomness = 0.08f;
	[SerializeField] private float m_shakeTime = 0.08f;

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
			yield return new WaitForSecondsRealtime(m_shakeTime);

			transform.position = shakePosition;
		}
	}
}
