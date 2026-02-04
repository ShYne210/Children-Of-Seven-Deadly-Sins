using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections;

public enum DialogType
{
    Normal,
    Choice1,   // Lựa chọn đầu
    Choice2,   // Có / Không
    End
}

[System.Serializable]
public class DialogLine
{
    public DialogType type;

    [TextArea(2, 6)]
    public string text;
}

public class DialogManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject blackFadePanel;
    public GameObject dialogPanel;
    public GameObject choicePanel;

    [Header("Text")]
    public TextMeshProUGUI dialogText;

    [Header("Buttons")]
    public Button btnNext;
    public Button btnSkip;
    public Button btnAuto;
    public Button btnChoice1;
    public Button btnChoice2;

    [Header("Dialog Data")]
    public DialogLine[] dialogs;

    int index = 0;
    bool waitingForChoice = false;
    Coroutine autoRoutine;

    void Start()
    {
        blackFadePanel.SetActive(true);
        dialogPanel.SetActive(true);
        choicePanel.SetActive(false);

        btnNext.onClick.AddListener(NextDialog);
        btnSkip.onClick.AddListener(SkipToNextChoice);
        btnAuto.onClick.AddListener(ToggleAuto);

        btnChoice1.onClick.AddListener(OnChoice1);
        btnChoice2.onClick.AddListener(OnChoice2);

        ShowDialog();
    }

    void Update()
    {
        // ❗ Chặn click xuyên UI
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (waitingForChoice)
            return;

        if (Input.GetMouseButtonDown(0))
            NextDialog();
    }

    void ShowDialog()
    {
        if (index >= dialogs.Length)
        {
            EndCutscene();
            return;
        }

        DialogLine line = dialogs[index];
        dialogText.text = line.text;

        if (line.type == DialogType.Normal)
        {
            choicePanel.SetActive(false);
            waitingForChoice = false;
        }
        else
        {
            choicePanel.SetActive(true);
            waitingForChoice = true;
        }
    }

    void NextDialog()
    {
        if (waitingForChoice) return;

        index++;
        ShowDialog();
    }

    void SkipToNextChoice()
    {
        if (waitingForChoice) return;

        for (int i = index; i < dialogs.Length; i++)
        {
            if (dialogs[i].type == DialogType.Choice1 ||
                dialogs[i].type == DialogType.Choice2)
            {
                index = i;
                ShowDialog();
                return;
            }
        }

        EndCutscene();
    }

    void ToggleAuto()
    {
        if (waitingForChoice) return;

        if (autoRoutine == null)
            autoRoutine = StartCoroutine(AutoPlay());
        else
        {
            StopCoroutine(autoRoutine);
            autoRoutine = null;
        }
    }

    IEnumerator AutoPlay()
    {
        while (!waitingForChoice)
        {
            yield return new WaitForSeconds(2f);
            NextDialog();
        }
    }

    // ======================
    // CHOICE LOGIC
    // ======================

    void OnChoice1()
    {
        if (dialogs[index].type == DialogType.Choice1)
        {
            // ❌ Lựa chọn 1 → GAME OVER
            GameOver();
        }
        else if (dialogs[index].type == DialogType.Choice2)
        {
            // ❌ Choice 2 - Không
            GameOver();
        }
    }

    void OnChoice2()
    {
        if (dialogs[index].type == DialogType.Choice1)
        {
            // ✅ Sang Choice 2
            index++;
            waitingForChoice = false;
            ShowDialog();
        }
        else if (dialogs[index].type == DialogType.Choice2)
        {
            // ✅ Chấp nhận thử thách → Gameplay
            StartGameplay();
        }
    }

    // ======================
    // ENDINGS
    // ======================

    void GameOver()
    {
        dialogText.text =
            "GOD\n\"Ngươi đúng là hết thuốc chữa.\nHãy làm nô lệ cho bảy chúa quỷ vĩnh viễn.\"";

        choicePanel.SetActive(false);
        waitingForChoice = true;

        btnNext.interactable = false;
        btnSkip.interactable = false;
        btnAuto.interactable = false;
    }

    void StartGameplay()
    {
        dialogText.text =
            "GOD\n\"Vậy thì hãy chuộc lỗi đi.\nĐừng làm ta thất vọng.\"";

        choicePanel.SetActive(false);
        waitingForChoice = true;

        Invoke(nameof(EndCutscene), 2.5f);
    }

    void EndCutscene()
    {
        dialogPanel.SetActive(false);
        blackFadePanel.SetActive(false);

        // 👉 MỞ PLAYER / LOAD SCENE GAMEPLAY Ở ĐÂY
        // SceneManager.LoadScene("Gameplay");
    }
}
