using UnityEngine;

public class BillboardUI : MonoBehaviour
{
    void Update()
    {
        if (Camera.main != null)
            transform.forward = Camera.main.transform.forward;
    }
}
