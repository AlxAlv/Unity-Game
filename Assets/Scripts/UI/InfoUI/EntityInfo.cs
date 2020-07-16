using TMPro;
using UnityEngine;

public class EntityInfo : MonoBehaviour
{
    [SerializeField] protected GameObject _gameObject;
    [SerializeField] protected TextMeshProUGUI _textObject;
    [SerializeField] protected Canvas _canvasToSetCameraTo;
    [SerializeField] protected RectTransform _backgroundRectTransform;

    protected bool _showInfo = false;

    protected virtual void Awake()
    {
        _canvasToSetCameraTo.worldCamera = Camera.main;
    }

    public virtual void SetShowInfo(bool showInfo)
    {
        _showInfo = showInfo;
    }

    protected virtual void Update()
    {
        _gameObject.SetActive(_showInfo);

        // Dynamically Change Background Size
        float textPaddingSize = .1f;
        Vector2 backgorundSize = new Vector2(_textObject.preferredWidth + textPaddingSize, _textObject.preferredHeight + textPaddingSize * .25f);
        _backgroundRectTransform.sizeDelta = backgorundSize;
    }
}
