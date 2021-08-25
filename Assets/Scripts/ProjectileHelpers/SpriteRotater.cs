using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteRotater : MonoBehaviour
{
    private enum _axis { x, y, z };
	[SerializeField] private float rotationSpeed = 8.0f;
    [SerializeField] private _axis _rotatingAxis = _axis.x;

    private SpriteRenderer _sprite;

    // Start is called before the first frame update
    void Start()
    {
        _sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_sprite.enabled)
        {
            if (_rotatingAxis == _axis.x)
                _sprite.transform.Rotate(new Vector3(rotationSpeed, 0, 0));
            else if (_rotatingAxis == _axis.x)
                _sprite.transform.Rotate(new Vector3(0, rotationSpeed, 0));
            else if (_rotatingAxis == _axis.x)
                _sprite.transform.Rotate(new Vector3(0, 0, rotationSpeed));
        }
    }
}
