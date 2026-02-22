using System.Collections;
using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] public float moveSpeed = 3f;
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float viewDistance = 10f;
    [SerializeField] private float viewAngle = 45f;
    [SerializeField] private float waitTime = 2f;
    private bool isWaiting = false;
    private int currentIndex = 0;
    private bool hasCaught = false;
    private Player playerAction;

    void Start()
    {
        if (player != null)
        {
            playerAction = player.GetComponent<Player>();
        }
    }

    void Update()
    {
        if (!isWaiting)
        { MoveAlongPath(); }
        CheckVision();

        //Test 1
        // Vector3 dirToPlayer = (player.position - transform.position).normalized;
        // float angle = Vector3.Angle(transform.forward, dirToPlayer);

        // if (angle < viewAngle)
        // {
        //     //Vẫn hiện Debug được khi có vật thể nào đó trong tầm nhìn
        //     Debug.Log("Enemy thấy player trong góc nhìn!");
        //     //Từ khúc này không hoạt động kiểu như không thấy player
        //     float dist = Vector3.Distance(transform.position, player.position);
        //     if (dist < viewDistance)
        //     {
        //         // Kiểm tra có vật cản không
        //         if (!Physics.Raycast(transform.position, dirToPlayer, dist))
        //         {
        //             CatchPlayer();
        //         }
        //     }
        // }
        // Test 2
        // if (player == null) return;

        // float dist = Vector3.Distance(transform.position, player.position);
        // if (dist < viewDistance)
        // {
        //     Debug.Log("Enemy thấy player!");
        // }

    }

    IEnumerator WaitAtPoint()
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitTime); // dừng lại waitTime giây
        currentIndex = (currentIndex + 1) % waypoints.Length;
        isWaiting = false;
        Debug.Log("Enemy đã dừng lại tại điểm chờ: " + waypoints[currentIndex].name);
    }

    public void SetSpeed(float newSpeed)
    {
        moveSpeed = newSpeed;
        Debug.Log(gameObject.name + " speed set to: " + moveSpeed);
    }

    void MoveAlongPath()
    {
        if (waypoints.Length == 0) return;

        Transform target = waypoints[currentIndex];
        transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);

        // Xoay mượt theo hướng đi
        Vector3 direction = (target.position - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }

        // Kiểm tra nếu đã đến waypoint
        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            // Nếu waypoint là StopPoint thì dừng lại
            if (target.CompareTag("StopPoint"))
            {
                StartCoroutine(WaitAtPoint());
            }
            // Nếu waypoint là MovePoint thì đi tiếp ngay
            else if (target.CompareTag("MovePoint"))
            {
                currentIndex = (currentIndex + 1) % waypoints.Length;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Vẽ bán kính tầm nhìn
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewDistance);

        // Vẽ hai đường biên góc nhìn
        Vector3 leftBoundary = Quaternion.Euler(0, -viewAngle, 0) * transform.forward;
        Vector3 rightBoundary = Quaternion.Euler(0, viewAngle, 0) * transform.forward;

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, leftBoundary * viewDistance);
        Gizmos.DrawRay(transform.position, rightBoundary * viewDistance);
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
                if (hit.transform == player)
                {
                    Debug.Log("Không có vật cản giữa enemy và player");

                    // ✅ Enemy check busy mới
                    if (playerAction.enemyBusy)
                    {
                        Debug.Log("Enemy đang nhìn thấy player bận (mang đồ hoặc làm nhiệm vụ)!");

                        if (!hasCaught)
                        {
                            CatchPlayer();
                            hasCaught = true;
                        }
                    }
                    else
                    {
                        hasCaught = false; // Nếu player không busy thì không bắt
                    }
                }
                else
                {
                    Debug.Log("Có vật cản giữa enemy và player: " + hit.transform.name);
                    hasCaught = false;
                }
            }
            else
            {
                Debug.Log("Raycast không chạm gì cả!");
                hasCaught = false;
            }
        }
        else
        {
            hasCaught = false;
        }
    }

    void CatchPlayer()
    {
        Debug.Log("Enemy đã phát hiện và bắt player!");
        if (player != null)
        {
            Destroy(player.gameObject);
        }
    }

}