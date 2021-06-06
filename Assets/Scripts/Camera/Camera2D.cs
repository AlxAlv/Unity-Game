using UnityEngine;

public class Camera2D : MonoBehaviour
{
	private enum CameraMode
	{
		Update,
		FixedUpdate,
		LateUpdate
	}

	[Header("Target")]
	[SerializeField] private Transform m_targetTransform;

	[Header("Offset")] 
	[SerializeField] private Vector2 m_offset;

	[Header("Mode")] 
	[SerializeField] private CameraMode m_cameraMode = CameraMode.Update;

	[Header("Game Feel")]
	//_scaleFactor is to make the camera not go all the way to the mouse cursor position, tweak it until it feels right.
	//_maxDistance limits how far the camera can go from the player, tweak it until it feels right.
	[SerializeField] private float _scaleFactor = 0.5f;
	[SerializeField] private float _maxDistance = 3.0f;
	[SerializeField] private bool _smoothCamera = true;
	[SerializeField] private float _smoothSpeed = 0.125f;

	private Camera _camera;
	private Ray _ray;

	private Vector3 _defaultOffset;
	private Vector3 _offsetForOffset;
	private Vector3 _offset;
	private Vector2 _mousePositionCoords;

	private void Start()
	{
		_camera = GetComponent<Camera>();
		_defaultOffset = new Vector3(0, _maxDistance, (-_maxDistance));
		_offsetForOffset = new Vector3(0, _maxDistance, (-_maxDistance));
		_offset = new Vector3(0, _maxDistance, (-_maxDistance));
	}

	private void FollowTarget()
	{
		Vector3 desiredPosition = new Vector3(m_targetTransform.position.x + m_offset.x, m_targetTransform.position.y + m_offset.y, transform.position.z);
		transform.position = desiredPosition;
	}

	private void FollowLate()
	{
		float maxScreenPoint = _maxDistance;
		Vector3 mousePos = Input.mousePosition * maxScreenPoint + new Vector3(Screen.width, Screen.height, 0f) * ((1f - maxScreenPoint) * 0.5f);
		//Vector3 position = (target.position + GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition)) / 2f;
		Vector3 position = (m_targetTransform.transform.position + GetComponent<Camera>().ScreenToWorldPoint(mousePos)) / 2f;
		Vector3 destination = new Vector3(position.x, position.y, -10);
		Vector3 desiredPosition = destination;

		// Vector3 desiredPosition = m_targetTransform.transform.position;

		if (_smoothCamera)
		{
			transform.position = Vector3.Lerp(transform.position, desiredPosition, _smoothSpeed);
			transform.position = new Vector3(transform.position.x, transform.position.y, -10.0f);
		}
	}

	private void Update()
	{
		if (m_cameraMode == CameraMode.Update)
		{
			FollowTarget();
		}

		HandleInput();
	}

	private void FixedUpdate()
	{
		if (m_cameraMode == CameraMode.FixedUpdate)
		{
			FollowTarget();
		}
	}

	private void LateUpdate()
	{
		if (m_cameraMode == CameraMode.LateUpdate)
		{
			FollowLate();
		}
	}

	private void HandleInput()
	{
		if (Input.mouseScrollDelta.y > 0 && Camera.main.orthographicSize > (8))
			Camera.main.orthographicSize = Camera.main.orthographicSize - 1;

		else if (Input.mouseScrollDelta.y < 0 && Camera.main.orthographicSize < (16))
			Camera.main.orthographicSize = Camera.main.orthographicSize + 1;
	}
}
