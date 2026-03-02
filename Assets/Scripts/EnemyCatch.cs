using UnityEngine;

public class EnemyCatch : MonoBehaviour
{
    public int maxCatch = 4;   // bắt lần 4 => thua
    private int catchCount = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            catchCount++;

            Debug.Log("Player bị bắt: " + catchCount);

            // ===== NẾU CHƯA THUA =====
            if (catchCount < maxCatch)
            {
                PlayerEscape escape =
                    other.GetComponent<PlayerEscape>();

                if (escape != null)
                {
                    escape.StartEscape();
                }
            }
            else
            {
                // ===== GAME OVER =====
                if (GameOverManager.instance != null)
                {
                    GameOverManager.instance.GameOver();
                }
            }

            // ===== RUNG CAMERA =====
            if (CameraShake.instance != null)
            {
                CameraShake.instance.Shake(0.4f, 0.2f);
            }
        }
    }
}