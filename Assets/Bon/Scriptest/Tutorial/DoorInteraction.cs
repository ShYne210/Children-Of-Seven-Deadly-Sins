using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.InputSystem; // thêm dòng này

public class DoorInteraction : MonoBehaviour
{
    [SerializeField] private float interactDistance = 3f; // khoảng cách để hiện E
    [SerializeField] private TextMeshProUGUI interactText; // UI hiện chữ E
    [SerializeField] private string sceneToLoad = "NextScene"; // tên scene cần load

    private Transform player;
    private QuestManager questManager;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        questManager = FindObjectOfType<QuestManager>();

        if (interactText != null)
            interactText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (player == null) return;

        float dist = Vector3.Distance(transform.position, player.position);

        // Nếu trong khoảng cách thì hiện chữ E
        if (dist <= interactDistance)
        {
            if (interactText != null)
                interactText.gameObject.SetActive(true);

            // Nhấn E để tương tác (Input System mới)
            if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
            {
                SceneManager.LoadScene(sceneToLoad);
            }
        }
        else
        {
            if (interactText != null)
                interactText.gameObject.SetActive(false);
        }
    }
}
