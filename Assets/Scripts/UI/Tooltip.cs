using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Tooltip : MonoBehaviour
{
    private static Tooltip Instance;

    private TextMeshPro _tooltipText;
    private RectTransform _backgroundRectTransform;
    private TooltipUIHelper _currentToolTipBeingViewed;

    [SerializeField] private GameObject _tooltipVisuals;
    [SerializeField] private Camera _uiCamera;

    private void Awake()
    {
        _backgroundRectTransform = _tooltipVisuals.transform.Find("Background").GetComponent<RectTransform>();
        _tooltipText = _tooltipVisuals.transform.Find("Text").GetComponent<TextMeshPro>();

        Instance = this;
        HideTooltip();
    }

    private void Update()
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponent<RectTransform>(), Input.mousePosition, _uiCamera, out localPoint);

        transform.localPosition = new Vector2(localPoint.x, localPoint.y + (16f));

        if ((_currentToolTipBeingViewed && _currentToolTipBeingViewed.gameObject.activeInHierarchy == false) || (_currentToolTipBeingViewed == null))
            HideTooltip();
    }

    private void ResetTooltip()
    {
        _currentToolTipBeingViewed = null;
    }

    private void ShowTooltip(string tooltipString, TooltipUIHelper currentUI)
    { 
	    _tooltipText.text = tooltipString;
        _currentToolTipBeingViewed = currentUI;

        float textPaddingSize = 15.0f;
        Vector2 backgorundSize = new Vector2(_tooltipText.preferredWidth + textPaddingSize * 2f, _tooltipText.preferredHeight + textPaddingSize * 2f);
        _backgroundRectTransform.sizeDelta = backgorundSize;

        StartCoroutine(ShowToolTip());
    }

    private void HideTooltip()
    {
	    _tooltipVisuals.SetActive(false);
        ResetTooltip();
    }
    
    public static void ShowToolTip_Static(string tooltipString, TooltipUIHelper currentUI)
    {
        Instance.ShowTooltip(tooltipString, currentUI); 
    }

    public static void HideTooltip_Static()
    {
        Instance.HideTooltip();
    }

    IEnumerator ShowToolTip()
    {
	    yield return null;

	    _tooltipVisuals.SetActive(true);
    }
}
