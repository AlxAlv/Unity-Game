using UnityEngine;

public class Skill : MonoBehaviour
{
	public Entity m_skillUser { get; set; }

	protected virtual void Update()
	{
	}

	public void SetUser(Entity owner)
	{
		m_skillUser = owner;
	}
}
