﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Press The PLAY Button
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
	}

    // Press The ERASE DATA Button
    public void EraseData()
    {
		PlayerPrefs.DeleteAll();
    }
}
