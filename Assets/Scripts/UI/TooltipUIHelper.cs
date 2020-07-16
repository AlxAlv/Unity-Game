using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipUIHelper : MonoBehaviour, IPointerClickHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private string _texToDisplay;

    public TooltipUIHelper(string textToDisplay)
    {
        _texToDisplay = textToDisplay;
    }

    public void OnDrag(PointerEventData eventData)
    {
    }

    public void OnPointerClick(PointerEventData eventData)
    {
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _texToDisplay = _texToDisplay.Replace("\\n", "\n");
        Tooltip.ShowToolTip_Static(_texToDisplay, this);
    }

    public void SetText(string textToDisplay)
    {
        _texToDisplay = textToDisplay;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Tooltip.HideTooltip_Static();
    }

    public void HideToolip()
    {
        Tooltip.HideTooltip_Static();
    }
}
