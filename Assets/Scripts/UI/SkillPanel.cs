using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillPanel : MonoBehaviour
{
    [Header("Panels")]
	[SerializeField] private GameObject _archeryPanel;
	[SerializeField] private GameObject _magicPanel;
	[SerializeField] private GameObject _meleePanel;

	[Header("Arrows")]
    [SerializeField] private GameObject _archeryArrow;
	[SerializeField] private GameObject _magicArrow;
	[SerializeField] private GameObject _meleeArrow;

	public void ShowArcheryTab()
	{
		_archeryPanel.SetActive(true);
		_magicPanel.SetActive(false);
		_meleePanel.SetActive(false);

		_archeryArrow.SetActive(true);
		_magicArrow.SetActive(false);
		_meleeArrow.SetActive(false);
	}

	public void ShowMagicTab()
	{
		_archeryPanel.SetActive(false);
		_magicPanel.SetActive(true);
		_meleePanel.SetActive(false);

		_archeryArrow.SetActive(false);
		_magicArrow.SetActive(true);
		_meleeArrow.SetActive(false);
	}

	public void ShowMeleeTab()
	{
		_archeryPanel.SetActive(false);
		_magicPanel.SetActive(false);
		_meleePanel.SetActive(true);

		_archeryArrow.SetActive(false);
		_magicArrow.SetActive(false);
		_meleeArrow.SetActive(true);
	}

}
