using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harvestable : MonoBehaviour
{
    [SerializeField] protected GameObject _player;
    [SerializeField] protected float _rechargeTime = 10;

    public bool PlayerIsInRange { get; set; }

    protected float _timer = 0.0f;
    protected bool _isHarvestable = true;

    protected virtual void Update()
    {
        UpdateAnimation();
        UpdateRechargeTimer();

        if (_timer == 0.0f)
            _timer = Time.time + _rechargeTime;
    }

    public virtual void OnClick()
    {

    }

    protected virtual void UpdateRechargeTimer()
    {
        if (!_isHarvestable && IsTimerDone())
        {
            UpdateAnimation();
        }
    }

    protected virtual bool IsTimerDone()
    {
        if (Time.time > _timer)
        {
            _isHarvestable = true;

            _timer = 0.0f;
            return true;
        }

        return false;
    }

    protected virtual void UpdateAnimation()
    {
        
    }
}
