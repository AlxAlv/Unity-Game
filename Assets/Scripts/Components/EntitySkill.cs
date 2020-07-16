using UnityEngine;
public class EntitySkill : EntityComponent
{
	[Header("Skill Settings")]
	[SerializeField] public Skill m_skillToUse;
	[SerializeField] public Transform m_skillPosition;

	public SpriteRenderer m_skillIcon { get; set; }
	public TextMesh SkillUses { get; set; }
	public Skill m_currentSkill { get; set; }

	protected override void Start()
	{
		base.Start();
		UseSkill(m_skillToUse, m_skillPosition);
	}

	protected override void Update()
	{
		base.Update();
	}

	public void UseSkill(Skill skill, Transform skillPosition)
	{
		m_currentSkill = Instantiate(skill, m_skillPosition.position, m_skillPosition.rotation);
		m_currentSkill.transform.parent = skillPosition;
		m_currentSkill.SetUser(m_entity);

		m_skillIcon = m_currentSkill.GetComponent<SpriteRenderer>();
		SkillUses = m_skillIcon.GetComponentInChildren<TextMesh>();

		if (m_entity.EntityType == Entity.EntityTypes.Player)
		{
			SkillCancelHelper helper = m_currentSkill.GetComponent<SkillCancelHelper>();
			helper.SetEntity(m_entityWeapon);
		}
		
		m_skillIcon.enabled = false;
		SkillUses.text = "";
	}
}
