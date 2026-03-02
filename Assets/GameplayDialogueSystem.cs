using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class GameplayDialogueSystem : MonoBehaviour
{
    [Header("UI")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public Button nextButton;
    public Button autoButton;
    public TextMeshProUGUI autoButtonText;

    [Header("Dialogue Lines")]
    [TextArea(2, 5)]
    public string[] lines;

    [Header("Settings")]
    public float autoDelay = 2.5f; // thời gian tự chuyển câu

    [Header("Player Lock")]
    public MonoBehaviour playerMovement; // kéo FPSMovement vào đây

    private int currentLine = 0;
    private bool isDialogueActive = false;
    private bool isAutoMode = false;
    private Coroutine autoCoroutine;

    void Start()
    {
        // Gán sự kiện nút
        nextButton.onClick.AddListener(NextLine);
        autoButton.onClick.AddListener(ToggleAuto);

        StartDialogue();
    }

    void StartDialogue()
    {
        isDialogueActive = true;
        dialoguePanel.SetActive(true);
        currentLine = 0;

        // Unlock chuột để bấm UI
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (playerMovement != null)
            playerMovement.enabled = false;

        ShowLine();
    }

    void ShowLine()
    {
        dialogueText.text = lines[currentLine];

        // Nếu đang Auto thì restart timer
        if (isAutoMode)
        {
            if (autoCoroutine != null)
                StopCoroutine(autoCoroutine);

            autoCoroutine = StartCoroutine(AutoNext());
        }
    }

    public void NextLine()
    {
        if (!isDialogueActive) return;

        currentLine++;

        if (currentLine >= lines.Length)
        {
            EndDialogue();
        }
        else
        {
            ShowLine();
        }
    }

    public void ToggleAuto()
    {
        isAutoMode = !isAutoMode;

        if (isAutoMode)
        {
            autoButtonText.text = "AUTO: ON";
            autoCoroutine = StartCoroutine(AutoNext());
        }
        else
        {
            autoButtonText.text = "AUTO: OFF";

            if (autoCoroutine != null)
                StopCoroutine(autoCoroutine);
        }
    }

    IEnumerator AutoNext()
    {
        yield return new WaitForSeconds(autoDelay);
        NextLine();
    }

        void EndDialogue()
    {
        isDialogueActive = false;
        dialoguePanel.SetActive(false);

        // Lock lại chuột
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (playerMovement != null)
            playerMovement.enabled = true;

        if (autoCoroutine != null)
            StopCoroutine(autoCoroutine);
    }
}