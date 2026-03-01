using UnityEngine;

public class BookInteract : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject bookPanel;

    public bool IsOpen { get; private set; }

    private void Awake()
    {
        if (bookPanel != null)
            bookPanel.SetActive(false);
    }

    public void Interact()
    {
        if (IsOpen)
            CloseBook();
        else
            OpenBook();
    }

    public void CloseBook()
    {
        IsOpen = false;
        bookPanel.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
    }

    private void OpenBook()
    {
        IsOpen = true;
        bookPanel.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;
    }
}
