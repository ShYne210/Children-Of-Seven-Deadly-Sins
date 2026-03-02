using UnityEngine;
using UnityEngine.UI;

public class PlayerEscape : MonoBehaviour
{
    public GameObject escapeUI;
    public Image progressBar;

    public FPSMovement playerMovement;

    public float holdTime = 3f;

    float holdTimer;
    bool escaping = false;

    void Start()
    {
        escapeUI.SetActive(false);
    }

    void Update()
    {
        if (!escaping) return;

        if (Input.GetKey(KeyCode.E))
        {
            holdTimer += Time.deltaTime;
            progressBar.fillAmount = holdTimer / holdTime;

            if (holdTimer >= holdTime)
            {
                EscapeSuccess();
            }
        }
        else
        {
            holdTimer = 0;
            progressBar.fillAmount = 0;
        }
    }

    public void StartEscape()
    {
        Debug.Log("PLAYER CAUGHT");

        escaping = true;

        escapeUI.SetActive(true);

        // 🚨 DỪNG PLAYER
        playerMovement.enabled = false;
    }

    void EscapeSuccess()
    {
        escaping = false;

        escapeUI.SetActive(false);

        playerMovement.enabled = true;

        holdTimer = 0;
        progressBar.fillAmount = 0;
    }
}