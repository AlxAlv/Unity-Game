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
    private bool _canSkillModifyMovement = false;
    private bool _canPlayerBeHitStun = false;
    private bool _doesPlayerStopWhenAiming = false;
    private bool _doesRightClickMovePlayer = true;

    // Public Members   
    public bool IsTargettingNeeded => _targettingNeeded;
    public bool IsTargetNearest => _targetNearest;
    public bool IsMovementModifableBySkill => _canSkillModifyMovement;
    public bool IsPlayerHitStunnable => _canPlayerBeHitStun;
    public bool IsPlayerMovementStoppedWhenAiming => _doesPlayerStopWhenAiming;
    public bool IsRightClickAlsoMove => _doesRightClickMovePlayer;

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

    public void ToggleCanSkillModifyMovement()
    {
        _canSkillModifyMovement = !_canSkillModifyMovement;
    }

    public void ToggleCanPlayerBeHitStun()
    {
        _canPlayerBeHitStun = !_canPlayerBeHitStun;
    }

    public void TogglePlayerMovementStopsWhenAiming()
    {
        _doesPlayerStopWhenAiming = !_doesPlayerStopWhenAiming;
    }

    public void ToggleDoesRightClickMovePlayer()
    {
        _doesRightClickMovePlayer = !_doesRightClickMovePlayer;
    }
}
