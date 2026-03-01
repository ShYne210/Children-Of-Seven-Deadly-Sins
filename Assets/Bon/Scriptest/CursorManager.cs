using UnityEngine;

public class CursorManager : MonoBehaviour
{
    void Start()
    {
        // Hiện chuột và cho phép di chuyển
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
