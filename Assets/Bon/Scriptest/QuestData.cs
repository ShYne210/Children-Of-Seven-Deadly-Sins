using UnityEngine;

public enum QuestStatus { NotStarted, InProgress, Completed }

[System.Serializable]
public class QuestData
{
    public string questName;        // Tên nhiệm vụ
    public string description;      // Mô tả nhiệm vụ
    public int requiredAmount;      // Số lượng cần đạt (ví dụ: giấu 3 đồ ăn, tìm 1 nhẫn)
    public int currentAmount;       // Tiến độ hiện tại
    public QuestStatus status;      // Trạng thái nhiệm vụ
    public string nextQuestName;    // Tên nhiệm vụ tiếp theo (nếu có)
    public bool isFinalQuest;       // Đánh dấu nhiệm vụ cuối cùng
}
