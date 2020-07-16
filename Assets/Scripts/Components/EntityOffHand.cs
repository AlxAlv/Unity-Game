using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityOffHand: EntityComponent
{
	[Header("Arm Settings")]
	[SerializeField] public OffArm m_armToUse;
	[SerializeField] private Transform m_armPosition;
	
	public WeaponAim m_armAim { get; set; }

	protected override void Start()
	{
		base.Start();
		EquipArm(m_armToUse, m_armPosition);
	}

	public void EquipArm(OffArm arm, Transform armPosition)
	{
		m_armToUse = Instantiate(arm, armPosition.position, armPosition.rotation);
		m_armToUse.transform.parent = armPosition;
		m_armToUse.SetOwner(m_entity);
		m_armAim = m_armToUse.GetComponent<WeaponAim>();
	}
}
