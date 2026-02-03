using UnityEngine;
using TMPro;

public class MessageUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private float displayDuration = 2f;

    private float timer = 0f;

    void Start()
    {
        if (messageText != null)
            messageText.text = "";
    }

    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            if (timer <= 0 && messageText != null)
                messageText.text = "";
        }
    }

    public void ShowMessage(string msg)
    {
        if (messageText != null)
        {
            messageText.text = msg;
            timer = displayDuration;
        }
    }
}
