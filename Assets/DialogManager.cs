using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class DialogManager : MonoBehaviour
{
    // ===== UI =====
    [Header("Panels")]
    public GameObject blackFadePanel;
    public GameObject dialogPanel;
    public GameObject choicePanel;

    [Header("Text")]
    public TextMeshProUGUI dialogText;
    public TextMeshProUGUI choiceText1;
    public TextMeshProUGUI choiceText2;

    [Header("Buttons")]
    public Button btnNext;
    public Button btnSkip;
    public Button btnChoice1;
    public Button btnChoice2;

    // ===== Dialog Data =====
    [Header("Dialog Content")]
    [TextArea(3, 6)]
    public string[] dialogs;

    [Header("Dialog Index Setup")]
    public int indexChoice1;        // trước lựa chọn 1
    public int indexAfterChoice1;   // dialog thần giải thích
    public int indexChoice2;        // trước lựa chọn 2

    int currentIndex = 0;

    enum DialogState
    {
        Normal,
        Choice1,
        AfterChoice1,
        Choice2,
        End
    }

    DialogState currentState = DialogState.Normal;

    void Start()
    {
        blackFadePanel.SetActive(true);
        dialogPanel.SetActive(true);
        choicePanel.SetActive(false);

        ShowDialog();

        btnNext.onClick.AddListener(NextDialog);
        btnSkip.onClick.AddListener(SkipDialog);
        btnChoice1.onClick.AddListener(() => OnChoice1(1));
        btnChoice2.onClick.AddListener(() => OnChoice1(2));
    }

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
            NextDialog();
    }

    // ===================== DIALOG =====================

    void ShowDialog()
    {
        dialogText.text = dialogs[currentIndex];
    }

    void NextDialog()
    {
        if (currentState == DialogState.Choice1 || currentState == DialogState.Choice2)
            return;

        currentIndex++;

        if (currentState == DialogState.Normal && currentIndex >= indexChoice1)
        {
            ShowChoice1();
            return;
        }

        if (currentState == DialogState.AfterChoice1 && currentIndex >= indexChoice2)
        {
            ShowChoice2();
            return;
        }

        if (currentIndex >= dialogs.Length)
        {
            EndDialog();
            return;
        }

        ShowDialog();
    }

    void SkipDialog()
    {
        if (currentState == DialogState.Normal)
        {
            currentIndex = indexChoice1;
            ShowChoice1();
        }
        else if (currentState == DialogState.AfterChoice1)
        {
            currentIndex = indexChoice2;
            ShowChoice2();
        }
    }

    // ===================== CHOICE 1 =====================

    void ShowChoice1()
    {
        currentState = DialogState.Choice1;
        choicePanel.SetActive(true);

        choiceText1.text = "Tại sao ta phải nghe lời ngươi?";
        choiceText2.text = "Nếu ta thất bại thì sao?";

        btnNext.interactable = false;
        btnSkip.interactable = false;
    }

    void OnChoice1(int choice)
    {
        choicePanel.SetActive(false);

        if (choice == 1)
        {
            ShowSingleDialog(
                "Vậy thì ngươi sẽ phải làm nô lệ cho 7 con quỷ, tượng trưng cho bảy tội lỗi, vĩnh viễn."
            );
            currentState = DialogState.End;
        }
        else
        {
            currentState = DialogState.AfterChoice1;
            currentIndex = indexAfterChoice1;
            btnNext.interactable = true;
            btnSkip.interactable = true;
            ShowDialog();
        }
    }

    // ===================== CHOICE 2 =====================

    void ShowChoice2()
    {
        currentState = DialogState.Choice2;
        choicePanel.SetActive(true);

        choiceText1.text = "Ta chấp nhận";
        choiceText2.text = "Ta từ chối";

        btnNext.interactable = false;
        btnSkip.interactable = false;
    }

    void OnChoice2(int choice)
    {
        choicePanel.SetActive(false);

        if (choice == 1)
        {
            ShowSingleDialog(
                "Vậy thì hãy chuộc lỗi đi. Đừng làm ta thất vọng."
            );
        }
        else
        {
            ShowSingleDialog(
                "Ngươi đúng là hết thuốc chữa. Hãy nô dịch cho 7 chúa quỷ vĩnh viễn đi."
            );
        }

        currentState = DialogState.End;
        btnNext.interactable = true;
        btnSkip.interactable = true;
    }

    // ===================== UTIL =====================

    void ShowSingleDialog(string text)
    {
        dialogText.text = text;
    }

    void EndDialog()
    {
        dialogPanel.SetActive(false);
        blackFadePanel.SetActive(false);
        // mở gameplay tại đây
    }
}
