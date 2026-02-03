using UnityEngine;

[System.Serializable]
public class Quest
{
    public string questName;        // tên nhiệm vụ
    public string description;      // mô tả nhiệm vụ
    public int targetCount;         // số lượng cần hoàn thành (ví dụ 6)
    public int currentCount;        // số lượng hiện tại (ví dụ 0)
    public bool isCompleted;        // trạng thái hoàn thành

    public Quest(string name, string desc, int target)
    {
        questName = name;
        description = desc;
        targetCount = target;
        currentCount = 0;
        isCompleted = false;
    }

    // thêm tiến trình
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

    // reset nhiệm vụ
    public void ResetQuest()
    {
        currentCount = 0;
        isCompleted = false;
    }

    // lấy text hiển thị
    public string GetQuestText()
    {
        if (!isCompleted)
            return $"{description} ({currentCount}/{targetCount})";
        else
            return $"{description} - Hoàn thành!";
    }
}
