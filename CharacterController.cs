using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [SerializeField] private float m_speed = 20f;
    private Rigidbody2D m_rigidBody;

    public Vector2 m_currentMovement { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        m_rigidBody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        MoveCharacter();
    }

    private void MoveCharacter()
    {
        Vector2 movementValues = new Vector2( x: Input.GetAxis("Horizontal"), y: Input.GetAxis("Vertical"));
        //m_rigidBody.MovePosition(m_rigidBody.position + (movementValues * m_speed * Time.fixedDeltaTime));
    }
    
    public void SetMovement(Vector2 newPosition)
    {
        m_currentMovement = newPosition;
    }
}
