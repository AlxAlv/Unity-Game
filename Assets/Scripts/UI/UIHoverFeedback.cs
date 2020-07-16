using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(EventTrigger))]
public class UIHoverFeedback : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] float _enlargeAmount = 1.1f;

    public float _originalAmountX = 0.0f;
    public float _originalAmountY = 0.0f;

    private EventTrigger eventTrigger;

    private void Start()
    {
        eventTrigger = GetComponent<EventTrigger>();
        if (eventTrigger != null)
        {
            EventTrigger.Entry enterUIEntry = new EventTrigger.Entry();
            // Pointer Enter
            enterUIEntry.eventID = EventTriggerType.PointerEnter;
            enterUIEntry.callback.AddListener((eventData) => { EnterUI(); });
            eventTrigger.triggers.Add(enterUIEntry);

            //Pointer Exit
            EventTrigger.Entry exitUIEntry = new EventTrigger.Entry();
            exitUIEntry.eventID = EventTriggerType.PointerExit;
            exitUIEntry.callback.AddListener((eventData) => { ExitUI(); });
            eventTrigger.triggers.Add(exitUIEntry);
        }
    }

    public void EnterUI()
    {
        SoundManager.Instance.Playsound("Audio/SoundEffects/UI_Hover");

        _originalAmountX = GetComponent<RectTransform>().localScale.x;
        _originalAmountY = GetComponent<RectTransform>().localScale.y;

        GetComponent<RectTransform>().localScale = new Vector3(_enlargeAmount, _enlargeAmount, 0.0f);
    }

    public void ExitUI()
    {
        SoundManager.Instance.Playsound("Audio/SoundEffects/UI_Exit");
        GetComponent<RectTransform>().localScale = new Vector3(_originalAmountX, _originalAmountY, 0.0f);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        SoundManager.Instance.Playsound("Audio/SoundEffects/UI_Click");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
    }

    public void OnEndDrag(PointerEventData eventData)
    {
    }

    public void OnDrag(PointerEventData eventData)
    {
    }
}
