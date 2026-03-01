using System.Collections;
using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    [Header("===== Công cụ =====")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private AudioSource chaseAudio;

    [Header("===== Tuần tra =====")]
    public float moveSpeed = 3f;
    [SerializeField] private float waitTime = 2f;

    [Header("===== Tầm nhìn =====")]
    [SerializeField] private float viewDistance = 10f;
    [SerializeField] private float viewAngle = 45f;

    [Header("===== Rượt đuổi =====")]
    [SerializeField] private float chaseSpeed = 6f;

    private bool isWaiting = false;
    private int currentIndex = 0;
    private bool isChasing = false;
    private Player playerAction;
    private Animator animator;

    void Start()
    {
        if (player != null)
        {
            playerAction = player.GetComponent<Player>();
        }
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isChasing) ChasePlayer();
        else if (!isWaiting) MoveAlongPath();
        else animator.SetFloat("Speed", 0f);

        CheckVision();
    }

    IEnumerator WaitAtPoint()
    {
        isWaiting = true;
        animator.SetFloat("Speed", 0f);
        yield return new WaitForSeconds(waitTime);
        currentIndex = (currentIndex + 1) % waypoints.Length;
        isWaiting = false;
    }

    void MoveAlongPath()
    {
        if (waypoints.Length == 0) return;

        Transform target = waypoints[currentIndex];
        transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
        animator.SetFloat("Speed", moveSpeed);

        Vector3 direction = (target.position - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }

        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            if (target.CompareTag("StopPoint")) StartCoroutine(WaitAtPoint());
            else currentIndex = (currentIndex + 1) % waypoints.Length;
        }
    }

    void CheckVision()
    {
        if (player == null || playerAction == null) return;

        Vector3 origin = transform.position + Vector3.up * 0.5f;
        Vector3 target = player.position + Vector3.up * 0.5f;
        Vector3 dirToPlayer = (target - origin).normalized;
        float angle = Vector3.Angle(transform.forward, dirToPlayer);
        float dist = Vector3.Distance(origin, target);

        if (angle < viewAngle && dist < viewDistance)
        {
            RaycastHit hit;
            if (Physics.Raycast(origin, dirToPlayer, out hit, dist))
            {
                if (hit.transform == player && playerAction.enemyBusy)
                {
                    if (!isChasing) StartChase();
                }
                else StopChase();
            }
            else StopChase();
        }
        else StopChase();
    }

    void StartChase()
    {
        isChasing = true;
        moveSpeed = chaseSpeed;
        animator.SetFloat("Speed", moveSpeed);

        if (chaseAudio != null && !chaseAudio.isPlaying) chaseAudio.Play();
    }

    void StopChase()
    {
        isChasing = false;
        moveSpeed = 3f;
        if (chaseAudio != null && chaseAudio.isPlaying) chaseAudio.Stop();
    }

    void ChasePlayer()
    {
        Vector3 target = player.position;
        transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);

        Vector3 direction = (target - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }

        if (Vector3.Distance(transform.position, player.position) < 1.0f) CatchPlayer();
    }

    void CatchPlayer()
    {
        Debug.Log("Enemy đã phát hiện và bắt player!");
        if (player != null)
        {
            PlayerInventory inv = player.GetComponent<PlayerInventory>();
            QuestManager qm = FindObjectOfType<QuestManager>();
            Quest currentQuest = qm != null ? qm.GetCurrentQuest() : null;

            // Nếu Quest 2 và có nhẫn → cutscene ending
            if (currentQuest != null && currentQuest.questName == "Tìm nhẫn của mẹ" && inv != null && inv.HasItem("Ring"))
            {
                FindObjectOfType<CutsceneController>().TriggerEndingCutscene();
                return;
            }

            // Các trường hợp khác → cutscene chết
            FindObjectOfType<CutsceneController>().TriggerDeathCutscene();
        }
    }


    public void SetSpeed(float newSpeed)
    {
        moveSpeed = newSpeed;
        if (animator != null) animator.SetFloat("Speed", moveSpeed);
    }
    public void SetQuest2Behavior()
    {
        // Tăng tốc độ tuần tra
        moveSpeed = chaseSpeed; // hoặc gấp đôi tốc độ tuần tra

        // Giảm thời gian dừng
        waitTime = Mathf.Max(0.5f, waitTime - 1f);
    }

}
