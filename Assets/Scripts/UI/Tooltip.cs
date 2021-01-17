using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Tooltip : MonoBehaviour
{
    private static Tooltip Instance;

    private TextMeshPro _tooltipText;
    private RectTransform _backgroundRectTransform;
    private TooltipUIHelper _currentToolTipBeingViewed;

    [SerializeField] private Camera _uiCamera;

    private void Awake()
    {
        _backgroundRectTransform = transform.Find("Background").GetComponent<RectTransform>();
        _tooltipText = transform.Find("Text").GetComponent<TextMeshPro>();

        Instance = this;
        HideTooltip();
    }

    private void Update()
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponent<RectTransform>(), Input.mousePosition, _uiCamera, out localPoint);

        transform.localPosition = new Vector2(localPoint.x, localPoint.y + (50.0f));


        if (_currentToolTipBeingViewed.gameObject.activeInHierarchy == false)
            HideTooltip();
    }

    private void ResetTooltip()
    {
        _currentToolTipBeingViewed = null;
    }

    private void ShowTooltip(string tooltipString, TooltipUIHelper currentUI)
    {
        gameObject.SetActive(true);

        _tooltipText.text = tooltipString;
        _currentToolTipBeingViewed = currentUI;

        float textPaddingSize = 10f;
        Vector2 backgorundSize = new Vector2(_tooltipText.preferredWidth + textPaddingSize * 2f, _tooltipText.preferredHeight + textPaddingSize * 2f);
        _backgroundRectTransform.sizeDelta = backgorundSize;
    }

    private void HideTooltip()
    {
        gameObject.SetActive(false);
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
}
