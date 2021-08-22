using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckBoolean : MonoBehaviour
{
    [SerializeField] protected Sprite _turnedOn;
    [SerializeField] protected Sprite _turnedOff;
    [SerializeField] protected Image _imageObject;

    private bool _isSet = false;

    // Update is called once per frame
    void Update()
    {
        if (_isSet)
            _imageObject.sprite = _turnedOn;
        else
            _imageObject.sprite = _turnedOff;
    }

    public void ToggleSprite()
    {
        _isSet = !_isSet;
    }
}
