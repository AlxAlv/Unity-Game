using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class OnClickStat : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    private int i = 0;
    private StatManager _statManagerToUse;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (i == 0)
            _statManagerToUse.AddDexterity();
        else if (i == 1)
            _statManagerToUse.AddIntelligence();
        else if (i == 2)
            _statManagerToUse.AddStrength();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
    }

    public void OnPointerUp(PointerEventData eventData)
    {
    }

    public void SetStat(int x, StatManager statManagerToUse)
    {
        _statManagerToUse = statManagerToUse;
        i = x;
    }
}
