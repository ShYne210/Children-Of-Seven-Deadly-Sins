using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BookUIManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject bookPanel;
    [SerializeField] private Image bookImage;
    [SerializeField] private TMP_Text bookText;

    private void Awake()
    {
        if (bookPanel != null)
            bookPanel.SetActive(false);
    }

    public void OpenBook(Sprite image, string content)
    {
        if (bookImage != null)
            bookImage.sprite = image;

        if (bookText != null)
            bookText.text = content;

        bookPanel.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CloseBook()
    {
        bookPanel.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
