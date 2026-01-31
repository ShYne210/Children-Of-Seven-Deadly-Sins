using UnityEngine;
using UnityEngine.AI;

public class NPCChasePlayer : MonoBehaviour
{
    public Transform player;

    public float catchDistance = 1.2f;   // khoảng cách bắt thật
    private NavMeshAgent agent;

    private bool isChasing = false;
    private bool hasCaught = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (!isChasing || hasCaught || player == null) return;

        // đuổi theo
        agent.SetDestination(player.position);

        float dist = Vector3.Distance(transform.position, player.position);

        // chỉ bắt khi RẤT GẦN
        if (dist <= catchDistance)
        {
            CatchPlayer();
        }
    }

    // ===== PHÁT HIỆN → CHỈ ĐUỔI =====
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isChasing = true;
            Debug.Log("Phát hiện player → bắt đầu đuổi");
        }
    }

    // ===== RA KHỎI VÙNG → NGỪNG ĐUỔI =====
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isChasing = false;
            agent.ResetPath();
            Debug.Log("Mất dấu player");
        }
    }

    // ===== BẮT =====
    void CatchPlayer()
    {
        if (hasCaught) return;

        hasCaught = true;

        agent.isStopped = true;

        Debug.Log("NPC đã bắt được Player!");

        Time.timeScale = 0f; // Game Over
    }
}
