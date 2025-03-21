﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private Transform m_spawnPosition;
    [SerializeField] private Entity m_entity;

    public int CurrentLevel = 1;
    public int LevelBeforeArena = 1;

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            ReviveCharacter();
        }
    }

    private void ReviveCharacter()
    {
        if (m_entity.GetComponent<Health>().m_currentHealth <= 0)
        {
            m_entity.GetComponent<Health>().Revive();

            StartCoroutine(MovePlayer());
        }
    }

    IEnumerator MovePlayer()
    {
	    yield return new WaitForSeconds(1.0f);

        m_entity.transform.position = m_spawnPosition.position;
    }
}
