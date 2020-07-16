using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RemoveSkill : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public void OnBeginDrag(PointerEventData eventData)
    {
    }

    public void OnDrag(PointerEventData eventData)
    {
    }

    public void OnEndDrag(PointerEventData eventData)
    {
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            GetComponent<Image>().sprite = Resources.Load<Sprite>("SkillIcons/IconSlot");
        }
    }
}
