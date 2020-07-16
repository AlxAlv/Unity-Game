using System.Collections.Generic;
using UnityEngine;

public class PopupMenu : MonoBehaviour
{
    [SerializeField] private Camera _uiCamera;

    private static PopupMenu Instance;

    private void Start()
    {
        Instance = this;

        HidePopupMenu();
    }

    private void HidePopupMenu()
    {
        gameObject.SetActive(false);

        foreach (Transform child in Instance.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            HidePopupMenu();
        }
    }

    public static void ShowPopupMenu_Static(List<string> menuItems)
    {
        Instance.ShowPopupMenu(menuItems);
    }

    public static void HideTooltip_Static()
    {
        Instance.HidePopupMenu();
    }

    private void ShowPopupMenu(List<string> menuItems)
    {
        gameObject.SetActive(true);
        transform.localPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0.0f);

        var menuItem = Resources.Load("Prefabs/UI/PopupMenuItem");

        // Create
        GameObject newObj = (GameObject)Instantiate(menuItem, transform.position, Quaternion.identity);
        newObj.GetComponent<PopupMenuItem>().Text.text = "Okay, bruh moment";
        newObj.transform.parent = Instance.transform;
        newObj.transform.localScale = new Vector3(1, 1, 1);
    }
}
