using UnityEngine;
using UnityEngine.EventSystems;

public class SkillCancelHelper : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    private EntityWeapon _entityWeapon;

    public void OnPointerClick(PointerEventData eventData)
    {
        //Debug.Log("Clicked the skill icon");
        if (_entityWeapon != null)
            _entityWeapon.CancelAllSkills();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
    }

    public void OnPointerUp(PointerEventData eventData)
    {
    }

    public void SetEntity(EntityWeapon entity)
    {
        _entityWeapon = entity;
    }
}
