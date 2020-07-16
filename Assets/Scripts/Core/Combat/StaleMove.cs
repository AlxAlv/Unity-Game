using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaleMove : MonoBehaviour
{
    public const string NonStaleMove = "LevelComponent";
    private string _lastMoveUsed;
    private bool _alreadyWarned = false;
    private int _timesUsed = 20;
    private int _hitsUntilStale = 100;
    private float _reductionPerHit = 0.10f;

    public bool IsStale { get; set; }

    public float CalculateDamage(float damage, string skillName = NonStaleMove)
    {
        _timesUsed++;

        if (_lastMoveUsed == skillName && skillName != NonStaleMove && _timesUsed > _hitsUntilStale)
        {
            if (!_alreadyWarned)
                DialogManager.Instance.InstantSystemMessage(skillName + " is becoming stale...");

            _alreadyWarned = true;
            IsStale = true;

            float reductionPerStaleHit = (damage * _reductionPerHit);
            float damageReduction =  reductionPerStaleHit * (_timesUsed - _hitsUntilStale);

            return (float)Math.Floor((damage - Mathf.Min(damageReduction, damage)));
        }

        if (skillName != _lastMoveUsed && skillName != NonStaleMove)
        {
            _alreadyWarned = false;
            _lastMoveUsed = skillName;
            _timesUsed = 1;
            IsStale = false;
        }

        return damage;
    }
}
