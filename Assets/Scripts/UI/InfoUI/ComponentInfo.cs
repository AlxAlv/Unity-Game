using TMPro;
using UnityEngine;

public class ComponentInfo : EntityInfo
{
    [SerializeField] private string _nameToDisplay;

    public override void SetShowInfo(bool showInfo)
    {
        base.SetShowInfo(showInfo);
    }

    protected override void Update()
    {
        base.Update();

        if (_showInfo)
        {
            _textObject.text = _nameToDisplay;
        }
    }
}
