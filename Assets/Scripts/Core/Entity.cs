using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{

    public enum EntityTypes
    {
        Player,
        AI
    }

    [SerializeField] private EntityTypes m_entityType;
    [SerializeField] private GameObject m_characterSprite;
    [SerializeField] private Animator m_characterAnimator;

    public EntityTypes EntityType => m_entityType;
    public GameObject CharacterSprite => m_characterSprite;
    public Animator EntityAnimator => m_characterAnimator;

    private void TakeDamage(int damage, string attackName, bool isCriticalHit)
    {
        GetComponent<Health>().TakeDamage(damage, attackName, isCriticalHit);
    }
}
