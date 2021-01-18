using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitySounds : MonoBehaviour
{
	[SerializeField] public string EnemyHitSoundPath;
	[SerializeField] public string EnemyDeathSoundPath;
	[SerializeField] public string EnemyPushbackSoundPath;
	[SerializeField] public SpriteRenderer Renderer;

	public void PlayHitSound()
	{
		if (Renderer.isVisible)
			SoundManager.Instance.Playsound(EnemyHitSoundPath);
	}

	public void PlayKnockbackSound()
	{
		if (Renderer.isVisible)
			SoundManager.Instance.Playsound(EnemyPushbackSoundPath);
	}

	public void PlayDeathSound()
	{
		if (Renderer.isVisible)
			SoundManager.Instance.Playsound(EnemyDeathSoundPath);
	}
}
