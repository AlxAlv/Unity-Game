using UnityEngine;

public class RepositionParticleSystem : MonoBehaviour
{
    enum Positions { BottomLeft, BottomRight }

    [SerializeField] private Positions _position;
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private float _padding;

    // Update is called once per frame
    void Update()
    {
	    switch (_position)
	    {
            case Positions.BottomLeft:
	            var blshape = _particleSystem.shape;
	            blshape.position = new Vector3((0 - (Screen.width / 2.0f) + _padding), (0 - (Screen.height / 2.0f) + _padding), 0);
	            break;

            case Positions.BottomRight:
	            var brshape = _particleSystem.shape;
	            brshape.position = new Vector3(((Screen.width / 2.0f) - _padding), (0 - (Screen.height / 2.0f) + _padding), 0);
	            break;
		}
    }
}
