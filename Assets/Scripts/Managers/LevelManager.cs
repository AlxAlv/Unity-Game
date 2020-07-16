using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Transform m_spawnPosition;
    [SerializeField] private Entity m_entity;

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            ReviveCharacter();
        }
    }

    private void ReviveCharacter()
    {
        if (m_entity.GetComponent<Health>().m_currentHealth <= 0)
        {
            m_entity.GetComponent<Health>().Revive();
            m_entity.transform.position = m_spawnPosition.position;
        }
    }
}
