using UnityEngine;
using UnityEngine.AI;

public class EnemyVisions : MonoBehaviour
{
    public Transform player;
    public NavMeshAgent agent;

    public float chaseSpeed = 4f;
    public float patrolSpeed = 2f;

    private bool isChasing = false;

    void Update()
    {
        if (isChasing)
        {
            agent.SetDestination(player.position);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isChasing = true;
            agent.speed = chaseSpeed;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isChasing = false;
            agent.speed = patrolSpeed;
        }
    }
}