using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillClick : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private SkillBar _skillBarToUse;
    [SerializeField] private int _index;

    public void OnPointerClick(PointerEventData eventData)
    {
        //if (eventData.button == PointerEventData.InputButton.Left)
        //{
        //    _skillBarToUse.HandleKeyPress(GetComponent<Image>());
        //    _skillBarToUse.SetSetBar(_index);
        //}
    }

    public void OnPointerDown(PointerEventData eventData)
    {
    }

    public void OnPointerUp(PointerEventData eventData)
    {
    }
}
