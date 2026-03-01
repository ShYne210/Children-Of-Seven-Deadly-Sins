using UnityEngine;
using TMPro;

public class QuestManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI questText;
    [SerializeField] private Quest quest1;   // Nhiệm vụ 1: Giấu đồ ăn
    [SerializeField] private Quest quest2;   // Nhiệm vụ 2: Tìm nhẫn của mẹ
    [SerializeField] private GameObject ringItem; // Vật phẩm nhẫn (ẩn lúc đầu)

    private int currentQuest = 1;

    void Start()
    {
        if (ringItem != null) ringItem.SetActive(false);
        UpdateQuestUI();
    }

    public void AddProgress(int amount = 1)
    {
        if (currentQuest == 1)
        {
            quest1.AddProgress(amount);
            UpdateQuestUI();

            if (quest1.isCompleted)
            {
                NextQuest();
            }
        }
        else if (currentQuest == 2)
        {
            quest2.AddProgress(amount);
            UpdateQuestUI();

            if (quest2.isCompleted)
            {
                PlayerInventory inv = FindObjectOfType<PlayerInventory>();
                if (inv != null && inv.HasItem("Ring"))
                {
                    FindObjectOfType<CutsceneController>().TriggerEndingCutscene();
                }
                else
                {
                    FindObjectOfType<MessageUI>().ShowMessage("Bạn cần có nhẫn để kết thúc!");
                }
            }
        }
    }

    public void NextQuest()
    {
        if (currentQuest == 1 && quest1.isCompleted)
        {
            currentQuest = 2;
            if (ringItem != null) ringItem.SetActive(true);
            UpdateQuestUI();

            FindObjectOfType<MessageUI>().ShowMessage("Quest 2 bắt đầu – Enemy trở nên nguy hiểm hơn!");

            // Chỉnh enemy hành vi
            EnemyVision enemy = FindObjectOfType<EnemyVision>();
            if (enemy != null) enemy.SetQuest2Behavior();

            // Đổi nhạc nền
            BackgroundMusic bgm = FindObjectOfType<BackgroundMusic>();
            if (bgm != null) bgm.PlayQuest2Music();

            // Đổi audio của enemy
            EnemyAudioManager audioManager = FindObjectOfType<EnemyAudioManager>();
            if (audioManager != null) audioManager.SwitchToQuest2Clips();
        }
    }

    private void UpdateQuestUI()
    {
        if (currentQuest == 1)
            questText.text = quest1.GetQuestText();
        else
            questText.text = quest2.GetQuestText();
    }

    public Quest GetCurrentQuest()
    {
        return currentQuest == 1 ? quest1 : quest2;
    }
}
