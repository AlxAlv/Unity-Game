using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
    public LineRenderer LineRenderer { get; set; }

    private void Awake()
    {
        EntityMovement = GetComponent<EntityMovement>();
        EntityWeapon = GetComponent<EntityWeapon>();
        Path = GetComponent<Path>();
        Collider2D = GetComponent<Collider2D>();
        LineRenderer = GetComponent<LineRenderer>();
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
                ThoughtPopups.RemoveInstanceFromList(gameObject.GetInstanceID());
            }

            _currentState = nextState;
        }
    }

    public WeaponAim[] GetWeaponAims()
    {
        return GetComponentsInChildren<WeaponAim>();
    }

    /* Members Used By Decision Making */
    // Timer to be used for decision making
    private float _timer = float.MaxValue;
    public float Timer => _timer;
    
    public bool IsTimePassed()
    {
	    return (Time.time > _timer);

    }

    public void ResetTime(float timeToWait)
    {
	    _timer = Time.time + timeToWait;
    }

    public void DrawPolygon(int vertexNumber, float radius, Vector3 centerPos, float startWidth, float endWidth, Color color)
    {
	    LineRenderer.startWidth = startWidth;
	    LineRenderer.endWidth = endWidth;
	    LineRenderer.startColor = color;
	    LineRenderer.endColor = color;
	    LineRenderer.loop = true;
	    float angle = 2 * Mathf.PI / vertexNumber;
	    LineRenderer.positionCount = vertexNumber;

	    for (int i = 0; i < vertexNumber; i++)
	    {
		    Matrix4x4 rotationMatrix = new Matrix4x4(new Vector4(Mathf.Cos(angle * i), Mathf.Sin(angle * i), 0, 0),
			    new Vector4(-1 * Mathf.Sin(angle * i), Mathf.Cos(angle * i), 0, 0),
			    new Vector4(0, 0, 1, 0),
			    new Vector4(0, 0, 0, 1));
		    Vector3 initialRelativePosition = new Vector3(0, radius, 0);
		    LineRenderer.SetPosition(i, centerPos + rotationMatrix.MultiplyPoint(initialRelativePosition));
	    }
    }
}
