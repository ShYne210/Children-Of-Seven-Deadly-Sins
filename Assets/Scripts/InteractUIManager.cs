using UnityEngine;
using TMPro;

public class InteractUIManager : MonoBehaviour
{
    public static InteractUIManager instance;

    public GameObject uiPanel;
    public TextMeshProUGUI textUI;

    void Awake()
    {
        instance = this;
        uiPanel.SetActive(false);
    }

    public void Show(string message)
    {
        uiPanel.SetActive(true);
        textUI.text = message;
    }

    public void Hide()
    {
        uiPanel.SetActive(false);
    }
}