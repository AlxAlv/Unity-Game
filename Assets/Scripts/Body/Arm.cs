using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arm : MonoBehaviour
{
	public Entity m_armOwner { get; set; }
	private EntityMovement m_entityMovement;
	private bool m_facingLeft;

	// Start is called before the first frame update
    void Start()
    {
	    m_facingLeft = true;
    }

    // Update is called once per frame
    void Update()
    {
		RotateWeapon();
    }

    public void SetOwner(Entity owner)
    {
	    m_armOwner = owner;
	    m_entityMovement = m_armOwner.GetComponent<EntityMovement>();
    }

    protected virtual void RotateWeapon()
    {
	    if ((m_armOwner.GetComponent<EntityFlip>().m_FacingLeft) && !m_facingLeft)
	    {
		    m_facingLeft = true;
		    transform.localScale = new Vector3(1, 1, 1);
		    transform.position = new Vector3((transform.position.x - 0.16f), transform.position.y, transform.position.z);
		}
	    else if ((!m_armOwner.GetComponent<EntityFlip>().m_FacingLeft) && m_facingLeft)
	    {
		    m_facingLeft = false;
		    transform.localScale = new Vector3(1, 1, 1);
		    transform.position = new Vector3((transform.position.x + 0.16f), transform.position.y, transform.position.z);
		}
    }
}
