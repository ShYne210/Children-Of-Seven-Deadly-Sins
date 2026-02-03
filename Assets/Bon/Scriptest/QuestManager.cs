using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class QuestManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI questText;
    [SerializeField] private List<Quest> quests = new List<Quest>();
    private int currentQuestIndex = 0;

    void Start()
    {
        UpdateQuestUI();
    }

    public void AddProgress(int amount = 1)
    {
        Quest quest = quests[currentQuestIndex];
        quest.AddProgress(amount);
        UpdateQuestUI();
    }

    private void UpdateQuestUI()
    {
        Quest quest = quests[currentQuestIndex];
        if (!quest.isCompleted)
        {
            questText.text = $"{quest.description} ({quest.currentCount}/{quest.targetCount})";
        }
        else
        {
            questText.text = $"{quest.description} - Hoàn thành!";
        }
    }

    public void NextQuest()
    {
        if (currentQuestIndex < quests.Count - 1)
        {
            currentQuestIndex++;
            UpdateQuestUI();
        }
    }
}
