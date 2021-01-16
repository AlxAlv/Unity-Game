using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenParticleSystem : Singleton<ScreenParticleSystem>
{
	[Header("Particle Systems")]
	[SerializeField] private ParticleSystem _bottomLeftParticleSystem;
	[SerializeField] private ParticleSystem _bottomRightParticleSystem;
	[SerializeField] private ParticleSystem _middleLeftParticleSystem;
	[SerializeField] private ParticleSystem _middleRightParticleSystem;

	[Header("Particles")]
	[SerializeField] private Material _celebrationParticles;

	public void PlayCelebrationParticles()
	{
		_bottomLeftParticleSystem.GetComponent<Renderer>().material = _celebrationParticles;
		_bottomRightParticleSystem.GetComponent<Renderer>().material = _celebrationParticles;
		_middleLeftParticleSystem.GetComponent<Renderer>().material = _celebrationParticles;
		_middleRightParticleSystem.GetComponent<Renderer>().material = _celebrationParticles;

		ParticleSystem.EmissionModule blEmission = _bottomLeftParticleSystem.emission;
		blEmission.rateOverTime = 4.0f;

		ParticleSystem.EmissionModule brEmission = _bottomRightParticleSystem.emission;
		brEmission.rateOverTime = 4.0f;

		ParticleSystem.EmissionModule mlEmission = _middleLeftParticleSystem.emission;
		mlEmission.rateOverTime = 4.0f;

		ParticleSystem.EmissionModule mrEmission = _middleRightParticleSystem.emission;
		mrEmission.rateOverTime = 4.0f;

		_bottomLeftParticleSystem.Play();
		_bottomRightParticleSystem.Play();
		_middleLeftParticleSystem.Play();
		_middleRightParticleSystem.Play();
	}
}
