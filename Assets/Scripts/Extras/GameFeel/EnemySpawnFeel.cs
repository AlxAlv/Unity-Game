using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnFeel : MonoBehaviour
{
	[SerializeField] private Transform _bottomOfSpritePosition;
    [SerializeField] private GameObject _spawnParticles;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private string _spawnSoundString;

    // Start is called before the first frame update
    void Start()
    {
	    GameObject particlesObject = Instantiate(_spawnParticles, _bottomOfSpritePosition.transform.position, Quaternion.identity);
	    particlesObject.transform.localScale = new Vector3(1, particlesObject.transform.localScale.y, particlesObject.transform.localScale.z);
	    particlesObject.GetComponent<ParticleSystem>().Play();

	    StartCoroutine(WaitAFrameForRenderer());
    }

    IEnumerator WaitAFrameForRenderer()
    {
	    yield return null;
	    yield return null;
	    yield return null;

	    if (_renderer.isVisible)
		    SoundManager.Instance.Playsound(_spawnSoundString);
    }
}
