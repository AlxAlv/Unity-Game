using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenParticleSystem : Singleton<ScreenParticleSystem>
{
	[Header("Particle Systems")]
	[SerializeField] private ParticleSystem _bottomLeftParticleSystem;
	[SerializeField] private ParticleSystem _bottomRightParticleSystem;

	[Header("Particles")]
	[SerializeField] private Material _celebrationParticles;

	public void PlayCelebrationParticles()
	{
		_bottomLeftParticleSystem.GetComponent<Renderer>().material = _celebrationParticles;
		_bottomRightParticleSystem.GetComponent<Renderer>().material = _celebrationParticles;

		_bottomLeftParticleSystem.Play();
		_bottomRightParticleSystem.Play();
	}
}
