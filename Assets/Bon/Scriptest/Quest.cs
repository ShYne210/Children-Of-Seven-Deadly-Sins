using UnityEngine;

[System.Serializable]
public class Quest
{
    public string questName;        // tên nhiệm vụ
    public string description;      // mô tả nhiệm vụ
    public int targetCount;         // số lượng cần hoàn thành
    public int currentCount;        // số lượng hiện tại
    public bool isCompleted;        // trạng thái hoàn thành
    public bool isFinalQuest;       // đánh dấu nhiệm vụ cuối cùng

    public Quest(string name, string desc, int target, bool final = false)
    {
        questName = name;
        description = desc;
        targetCount = target;
        currentCount = 0;
        isCompleted = false;
        isFinalQuest = final;
    }

    public void AddProgress(int amount = 1)
    {
        if (isCompleted) return;

        currentCount += amount;
        if (currentCount >= targetCount)
        {
            currentCount = targetCount;
            isCompleted = true;
        }
    }

    public void ResetQuest()
    {
        currentCount = 0;
        isCompleted = false;
    }

    public string GetQuestText()
    {
        if (!isCompleted)
            return $"{description} ({currentCount}/{targetCount})";
        else
            return $"{description} - Hoàn thành!";
    }
}
