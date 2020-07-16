using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStateController : MonoBehaviour
{
    [Header("State")]
    [SerializeField] private AIState _currentState;
    [SerializeField] private AIState _remainState;
    [SerializeField] public Transform SkillLoadingTransform;

    public Transform Target { get; set; }
    public EntityMovement EntityMovement { get; set; }
    public Path Path { get; set; }
    public Collider2D Collider2D { get; set; }
    public EntityWeapon EntityWeapon { get; set; }

    private void Awake()
    {
        EntityMovement = GetComponent<EntityMovement>();
        EntityWeapon = GetComponent<EntityWeapon>();
        Path = GetComponent<Path>();
        Collider2D = GetComponent<Collider2D>();
    }

    private void Update()
    {
        _currentState.EvaluateState(this);
    }

    public void TransitionToState(AIState nextState)
    {
        if (nextState != _remainState)
        {
            if (nextState.name.Contains("Idle") || nextState.name.Contains("idle"))
            {
                ExclamationMark.RemoveInstanceFromList(gameObject.GetInstanceID());
                //Debug.Log("Removed an enemy from being aggroed!");
            }

            _currentState = nextState;
        }
    }

    public WeaponAim[] GetWeaponAims()
    {
        return GetComponentsInChildren<WeaponAim>();
    }
}
