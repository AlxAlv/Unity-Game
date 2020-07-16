using UnityEngine;
using TMPro;

public class PopupMenuItem : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI Text;

    public PopupMenuItem(string textToUse)
    {
        Text.text = textToUse;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
