using UnityEngine;
using UnityEngine.AI;

public class NPCPatrol : MonoBehaviour
{
    public Transform[] patrolPoints;
    public float waitTime = 3f;   // đứng chờ bao nhiêu giây

    private NavMeshAgent agent;
    private int currentIndex = 0;
    private float timer = 0f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(patrolPoints[currentIndex].position);
    }

    void Update()
    {
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            timer += Time.deltaTime;

            if (timer >= waitTime)
            {
                GoNextPoint();
                timer = 0f;
            }
        }
    }

    void GoNextPoint()
    {
        currentIndex = (currentIndex + 1) % patrolPoints.Length;
        agent.SetDestination(patrolPoints[currentIndex].position);
    }
}
