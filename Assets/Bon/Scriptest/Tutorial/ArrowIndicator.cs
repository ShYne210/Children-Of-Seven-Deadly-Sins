using UnityEngine;

public class ArrowIndicator : MonoBehaviour
{
    [SerializeField] private RectTransform arrowUI; // UI mũi tên
    private Transform target;

    void Update()
    {
        if (target == null || arrowUI == null) return;

        // Lấy vị trí target trên màn hình
        Vector3 screenPos = Camera.main.WorldToScreenPoint(target.position);

        // Tính vector từ tâm màn hình tới target
        Vector3 dir = screenPos - new Vector3(Screen.width / 2f, Screen.height / 2f, 0);

        // Tính góc xoay
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        // Xoay mũi tên UI
        arrowUI.rotation = Quaternion.Euler(0, 0, angle);
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
