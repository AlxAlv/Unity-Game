using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettingsManager : Singleton<GameSettingsManager>
{
    [Header("Panels")]
    [SerializeField] private GameObject _settiingsPanel;

    // Private Members
    private bool _targettingNeeded = false;
    private bool _targetNearest = false;

    // Public Members   
    public bool IsTargettingNeeded => _targettingNeeded;
    public bool IsTargetNearest => _targetNearest;

    private void Awake()
    {
        _settiingsPanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            _settiingsPanel.SetActive(!_settiingsPanel.activeSelf);
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            _settiingsPanel.SetActive(false);
        }
    }

    public void ToggleTargetNearest()
    {
        _targetNearest = !_targetNearest;
    }

    public void ToggleTargettingNeeded()
    {
        _targettingNeeded = !_targettingNeeded;
    }
}
