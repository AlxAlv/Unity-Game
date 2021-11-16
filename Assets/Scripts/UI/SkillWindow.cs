using UnityEngine;

public class SkillWindow : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject _skillPanel;

    private void Start()
    {
        _skillPanel = GameObject.FindWithTag("SkillPanel");
        _skillPanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            _skillPanel.SetActive(!_skillPanel.activeSelf);
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            _skillPanel.SetActive(false);
        }
    }
}
