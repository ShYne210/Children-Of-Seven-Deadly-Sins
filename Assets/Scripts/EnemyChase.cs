using UnityEngine;
using UnityEngine.AI;

public class EnemyChase : MonoBehaviour
{
    public Transform player;
    public float detectDistance = 8f;

    private NavMeshAgent agent;
    private Animator anim;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        float dist = Vector3.Distance(
            transform.position,
            player.position
        );

        // ===== DETECT PLAYER =====
        if (dist < detectDistance)
        {
            // chạy
            agent.speed = 6f;
            agent.SetDestination(player.position);
        }
        else
        {
            // đi bộ (hoặc đứng)
            agent.speed = 2f;
        }

        // ===== UPDATE ANIMATION =====
        anim.SetFloat("Speed", agent.velocity.magnitude);
    }
}