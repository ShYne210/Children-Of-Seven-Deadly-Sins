using UnityEngine;
using TMPro;
using UnityEngine.InputSystem; // dùng Input System mới

public class TutorialPanelManager : MonoBehaviour
{
    [SerializeField] private GameObject tutorialPanel; // Panel UI chứa hướng dẫn
    [SerializeField] private TextMeshProUGUI tutorialText; // Text hiển thị trong Panel

    private bool isVisible = false;

    void Start()
    {
        // Ẩn panel lúc đầu
        tutorialPanel.SetActive(false);

        // Nội dung toàn bộ hướng dẫn
        tutorialText.text =
            "                                   Hướng dẫn chơi:\n\n" +
            "- Di chuyển bằng WASD    |    Xoay camera bằng chuột\n\n" +
            "- Nhấn E để nhặt và giấu vật phẩm\n\n" +
            "- Đặt vật phẩm vào điểm nhiệm vụ\n\n" +
            "- Tránh Enemy, Tầm nhìn Enemy sẽ là vùng màu đỏ\n\n" +
            "- Khi chạy sẽ tuột Stamina và khi ngưng chạy sẽ hồi lại\n\n" +
            "Nhấn H để đóng/mở hướng dẫn này.";
    }

    void Update()
    {
        if (Keyboard.current == null) return;

        // Nhấn phím H để bật/tắt panel
        if (Keyboard.current.hKey.wasPressedThisFrame)
        {
            isVisible = !isVisible;
            tutorialPanel.SetActive(isVisible);
        }
    }
}
