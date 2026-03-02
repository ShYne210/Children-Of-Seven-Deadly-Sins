using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogManager : MonoBehaviour
{
    [Header("PANELS")]
    public GameObject dialogPanel;
    public GameObject choicePanel;

    [Header("TEXT")]
    public TextMeshProUGUI dialogText;
    public TextMeshProUGUI choiceText1;
    public TextMeshProUGUI choiceText2;

    [Header("BUTTONS")]
    public Button btnNext;
    public Button btnAuto;
    public Button btnChoice1;
    public Button btnChoice2;

    [Header("Audio")]
    public CutsceneBGM cutsceneBGM;

    [Header("MAIN DIALOG")]
    [TextArea(3,6)]
    public string[] mainDialogs;

    [Header("EXPLAIN A (Tại sao phải nghe?)")]
    [TextArea(3,6)]
    public string[] explainA;

    [Header("EXPLAIN B (Nếu thất bại thì sao?)")]
    [TextArea(3,6)]
    public string[] explainB;

    [Header("ACCEPT DIALOG")]
    [TextArea(3,6)]
    public string[] acceptDialogs;

    [Header("GAME OVER DIALOG")]
    [TextArea(3,6)]
    public string[] gameOverDialogs;

    private int index = 0;
    private bool isAuto = false;
    private float autoDelay = 2.5f;

    private enum State
    {
        Main,
        Choice1,
        ExplainA,
        ExplainB,
        Choice2,
        Accept,
        GameOver,
        End
    }

    private State currentState;

    void Start()
    {
        dialogPanel.SetActive(true);
        choicePanel.SetActive(false);

        btnNext.onClick.AddListener(OnNext);
        btnAuto.onClick.AddListener(ToggleAuto);
        btnChoice1.onClick.AddListener(OnChoice1Selected);
        btnChoice2.onClick.AddListener(OnChoice2Selected);

        StartMain();
    }

    void Update()
    {
        if (isAuto && currentState != State.Choice1 && currentState != State.Choice2)
        {
            autoDelay -= Time.deltaTime;
            if (autoDelay <= 0f)
            {
                autoDelay = 2.5f;
                OnNext();
            }
        }
    }

    // ================= MAIN =================

    void StartMain()
    {
        currentState = State.Main;
        index = 0;
        ShowDialog(mainDialogs);
    }

    void OnNext()
    {
        if (currentState == State.Choice1 || currentState == State.Choice2)
            return;

        index++;

        switch (currentState)
        {
            case State.Main:
                if (index >= mainDialogs.Length)
                {
                    ShowChoice1();
                }
                else ShowDialog(mainDialogs);
                break;

            case State.ExplainA:
                if (index >= explainA.Length)
                {
                    ShowChoice2();
                }
                else ShowDialog(explainA);
                break;

            case State.ExplainB:
                if (index >= explainB.Length)
                {
                    ShowChoice2();
                }
                else ShowDialog(explainB);
                break;

            case State.Accept:
                if (index >= acceptDialogs.Length)
                    EndDialog();
                else ShowDialog(acceptDialogs);
                break;

            case State.GameOver:
                if (index >= gameOverDialogs.Length)
                    EndDialog();
                else ShowDialog(gameOverDialogs);
                break;
        }
    }

    // ================= CHOICE 1 =================

    void ShowChoice1()
    {
        currentState = State.Choice1;
        choicePanel.SetActive(true);

        choiceText1.text = "Tại sao tôi phải nghe lời ông?";
        choiceText2.text = "Nếu tôi thất bại thì sao?";

        btnNext.interactable = false;
    }

    void OnChoice1Selected()
    {
        choicePanel.SetActive(false);
        btnNext.interactable = true;

        currentState = State.ExplainA;
        index = 0;
        ShowDialog(explainA);
    }

    void OnChoice2Selected()
    {
        choicePanel.SetActive(false);
        btnNext.interactable = true;

        currentState = State.ExplainB;
        index = 0;
        ShowDialog(explainB);
    }

    // ================= CHOICE 2 =================

    void ShowChoice2()
    {
        currentState = State.Choice2;
        choicePanel.SetActive(true);

        choiceText1.text = "Ta chấp nhận thử thách.";
        choiceText2.text = "Ta từ chối.";

        btnNext.interactable = false;

        btnChoice1.onClick.RemoveAllListeners();
        btnChoice2.onClick.RemoveAllListeners();

        btnChoice1.onClick.AddListener(OnAccept);
        btnChoice2.onClick.AddListener(OnRefuse);
    }

    void OnAccept()
    {
        choicePanel.SetActive(false);
        btnNext.interactable = true;

        currentState = State.Accept;
        index = 0;
        ShowDialog(acceptDialogs);
    }

    void OnRefuse()
    {
        choicePanel.SetActive(false);
        btnNext.interactable = true;

        currentState = State.GameOver;
        index = 0;
        ShowDialog(gameOverDialogs);
    }

    // ================= UTIL =================

    void ShowDialog(string[] dialogArray)
    {
        if (dialogArray.Length == 0) return;
        dialogText.text = dialogArray[index];
    }

    void ToggleAuto()
    {
        isAuto = !isAuto;
        autoDelay = 2.5f;
    }

    void EndDialog()
{
    // Tắt nhạc cutscene
    if (cutsceneBGM != null)
    {
        cutsceneBGM.StopBGM();
    }

    dialogPanel.SetActive(false);

    // Load scene gameplay
    UnityEngine.SceneManagement.SceneManager.LoadScene("Map2");
}
}