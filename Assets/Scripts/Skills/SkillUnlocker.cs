using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillUnlocker : MonoBehaviour
{
	[SerializeField] public bool IsUnlocked = true;
	[SerializeField] private GameObject _skillToUnlock;

    // Update is called once per frame
    void Update()
    {
        _skillToUnlock.SetActive(IsUnlocked);
    }
}
