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
    [SerializeField] private GameObject _mainArmModel;
    [SerializeField] private GameObject _offArmModel;
    [SerializeField] private Animator m_characterAnimator;

    public EntityTypes EntityType => m_entityType;
    public GameObject CharacterSprite => m_characterSprite;
    public GameObject MainArmModel => _mainArmModel;
    public GameObject OffArmModel => _offArmModel;
    public Animator EntityAnimator => m_characterAnimator;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void TakeDamage(int damage, string attackName = StaleMove.NonStaleMove)
    {
        GetComponent<Health>().TakeDamage(damage, attackName);
    }
}
