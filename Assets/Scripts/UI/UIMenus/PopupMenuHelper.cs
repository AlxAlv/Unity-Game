using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PopupMenuHelper : MonoBehaviour, IPointerClickHandler
{
    public virtual void OnPointerClick(PointerEventData eventData)
    {
        var itemList = new List<string>();
        itemList.Add("Bruh");

        PopupMenu.ShowPopupMenu_Static(itemList);
    }
}
