using UnityEngine;
using TMPro;

public class NPCInteractTalk : MonoBehaviour
{
    public GameObject canvasUI;
    public TextMeshProUGUI textUI;

    [TextArea]
    public string[] dialogues;

    private bool playerInRange = false;
    private bool isTalking = false;
    private int currentLine = 0;

    void Start()
    {
        canvasUI.SetActive(false);
    }

    void Update()
    {
        if (!playerInRange) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!isTalking)
            {
                StartTalk();
            }
            else
            {
                NextLine();
            }
        }
    }

    void StartTalk()
    {
        isTalking = true;
        currentLine = 0;
        canvasUI.SetActive(true);
        textUI.text = dialogues[currentLine];
    }

    void NextLine()
    {
        currentLine++;

        if (currentLine >= dialogues.Length)
        {
            EndTalk();
            return;
        }

        textUI.text = dialogues[currentLine];
    }

    void EndTalk()
    {
        isTalking = false;
        canvasUI.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            canvasUI.SetActive(true);
            textUI.text = "Press E to talk";
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            isTalking = false;
            canvasUI.SetActive(false);
        }
    }
}
