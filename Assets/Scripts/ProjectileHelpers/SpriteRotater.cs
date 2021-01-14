using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteRotater : MonoBehaviour
{
	[SerializeField] private float rotationSpeed = 8.0f;

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
			_sprite.transform.Rotate(new Vector3(0, 0, rotationSpeed));
    }
}
