using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BerryBush : Harvestable
{
    [SerializeField] protected float _foodValue = 0.10f;
    [SerializeField] protected SpriteRenderer _renderer;
    [SerializeField] protected Sprite _harvestableSprite;
    [SerializeField] protected Sprite _nonHarvestableSprite;

    public override void OnClick()
    {
        base.OnClick();

        if (_isHarvestable && PlayerIsInRange)
        {
            _player.GetComponent<Hunger>().Eat(_foodValue);
            _isHarvestable = false;
        }
    }

    protected override void UpdateAnimation()
    {
        base.UpdateAnimation();

        if (_isHarvestable)
            _renderer.sprite = _harvestableSprite;
        else
            _renderer.sprite = _nonHarvestableSprite;
    }
}
