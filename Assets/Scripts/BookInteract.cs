using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class BookInteract : MonoBehaviour
{
    [Header("UI Book")]
    public GameObject bookUI;
    public TextMeshProUGUI bookText;
    public Image bookImage;

    [Header("Book Content")]
    [TextArea(3,6)]
    public string content;
    public Sprite bookSprite;

    [Header("Scene Settings")]
    public string nextSceneName; // Tên scene sẽ chuyển

    [Header("Player")]
    public MonoBehaviour playerMovement;

    private bool isPlayerNear = false;
    private bool isOpen = false;
    private bool hasReadBook = false;

    void Update()
    {
        // Nhấn E để mở sách
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E) && !isOpen)
        {
            OpenBook();
        }

        // Nhấn E lần nữa để đóng sách và chuyển scene
        else if (isOpen && Input.GetKeyDown(KeyCode.E))
        {
            CloseBookAndLoadScene();
        }
    }

    void OpenBook()
    {
        isOpen = true;
        bookUI.SetActive(true);

        bookText.text = content;
        bookImage.sprite = bookSprite;

        // Khóa player khi đọc sách
        if (playerMovement != null)
            playerMovement.enabled = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void CloseBookAndLoadScene()
    {
        isOpen = false;
        hasReadBook = true;
        bookUI.SetActive(false);

        // Mở lại điều khiển (trong lúc loading)
        if (playerMovement != null)
            playerMovement.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Load Scene mới
        if (!string.IsNullOrEmpty("Tutorial"))
        {
            SceneManager.LoadScene("Tutorial");
        }
        else
        {
            Debug.LogWarning("Chưa nhập tên Scene!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            isPlayerNear = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            isPlayerNear = false;
    }
}