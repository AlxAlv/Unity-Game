using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveChest : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject _popupPanel;
    [SerializeField] private GameObject _chestInfo;

    private bool _canOpenShop;

    void Update()
    {
        if (_canOpenShop && Input.GetKeyDown(KeyCode.F))
        {
            if (!_popupPanel.activeSelf)
            {
                _popupPanel.SetActive(true);
                SoundManager.Instance.Playsound("Audio/SoundEffects/ChestOpenFx");
            }
            else
            {
                _popupPanel.SetActive(false);
                SoundManager.Instance.Playsound("Audio/SoundEffects/ClosingChestFx");
            }    
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _canOpenShop = true;
            _chestInfo.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _canOpenShop = false;
            _popupPanel.SetActive(false);
            _chestInfo.SetActive(false);
        }
    }
}
