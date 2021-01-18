using UnityEngine;
using UnityEngine.EventSystems;

public class ToggleUI : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private GameObject _uiWindow;
    [SerializeField] private bool _resetChildren = false;

    public void OnPointerClick(PointerEventData eventData)
    {
        _uiWindow.SetActive(!_uiWindow.activeSelf);

		if (_uiWindow.activeSelf && _resetChildren)
			SetChildrenToActive(_uiWindow.transform);
	}

    void SetChildrenToActive(Transform parent)
    {
        foreach (Transform child in parent.transform)
        {
            child.gameObject.SetActive(_uiWindow.activeSelf);
            SetChildrenToActive(child.transform);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
    }

    public void OnPointerUp(PointerEventData eventData)
    {
    }
}
