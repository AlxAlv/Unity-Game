using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            SoundManager.Instance.Playsound("Audio/SoundEffects/UI_Click");
            Image currentImage = GetComponent<Image>();
            Image eventDataImage = eventData.pointerDrag.GetComponent<Image>();

            this.GetComponent<Image>().sprite = eventDataImage.sprite;
        }
    }
}
