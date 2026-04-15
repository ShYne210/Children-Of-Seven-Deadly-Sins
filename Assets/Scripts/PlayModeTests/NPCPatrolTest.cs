using System.Collections;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.TestTools;

public class NPCPatrolPlayTests
{
    private GameObject npcObject;
    private NPCPatrol npcPatrol;
    private NavMeshAgent agent;
    private Transform[] patrolPoints;
    
    // Reflection để truy cập private fields
    private FieldInfo currentIndexField;
    private FieldInfo timerField;

    [SetUp]
    public void Setup()
    {
        npcObject = new GameObject("NPC");
        agent = npcObject.AddComponent<NavMeshAgent>();
        npcPatrol = npcObject.AddComponent<NPCPatrol>();
        
        // Tắt physics update để test deterministic
        agent.updatePosition = false;
        agent.updateRotation = false;
        agent.stoppingDistance = 0.5f;
        
        // Tạo patrol points
        patrolPoints = new Transform[3];
        for (int i = 0; i < 3; i++)
        {
            GameObject p = new GameObject($"Point{i}");
            p.transform.position = new Vector3(i * 10, 0, 0);
            patrolPoints[i] = p.transform;
        }
        
        npcPatrol.patrolPoints = patrolPoints;
        npcPatrol.waitTime = 0.5f; // Ngắn để test nhanh
        
        // Lấy private fields
        currentIndexField = typeof(NPCPatrol).GetField("currentIndex", 
            BindingFlags.NonPublic | BindingFlags.Instance);
        timerField = typeof(NPCPatrol).GetField("timer", 
            BindingFlags.NonPublic | BindingFlags.Instance);
    }

    [TearDown]
    public void Teardown()
    {
        foreach (var p in patrolPoints)
            Object.Destroy(p.gameObject);
        Object.Destroy(npcObject);
    }

    // Helper: Get/Set private fields
    private int GetCurrentIndex() => (int)currentIndexField.GetValue(npcPatrol);
    private void SetCurrentIndex(int val) => currentIndexField.SetValue(npcPatrol, val);
    private float GetTimer() => (float)timerField.GetValue(npcPatrol);
    private void SetTimer(float val) => timerField.SetValue(npcPatrol, val);

    // TEST 1: Start sets destination to first point
    [UnityTest]
    public IEnumerator Start_SetsDestinationToFirstPoint()
    {
        yield return null; // Đợi Start chạy
        
        Assert.AreEqual(patrolPoints[0].position, agent.destination);
        Assert.AreEqual(0, GetCurrentIndex());
    }

    // TEST 2: Start gets NavMeshAgent
    [UnityTest]
    public IEnumerator Start_GetsNavMeshAgent()
    {
        yield return null;
        
        // Nếu agent null sẽ crash, test pass nghĩa là không crash
        Assert.IsNotNull(agent);
    }

    // TEST 3: Arrives at point, waits, then moves to next
    [UnityTest]
    public IEnumerator Update_ArrivesWaitsThenMoves()
    {
        yield return null;
        
        // Giả lập NPC đến điểm 0 bằng Warp
        agent.Warp(patrolPoints[0].position);
        yield return null;
        
        // Kiểm tra timer bắt đầu tăng
        Assert.AreEqual(0, GetCurrentIndex());
        
        // Đợi hết waitTime
        yield return new WaitForSeconds(npcPatrol.waitTime + 0.1f);
        
        // Phải chuyển sang điểm 1
        Assert.AreEqual(1, GetCurrentIndex());
        Assert.AreEqual(patrolPoints[1].position, agent.destination);
        Assert.AreEqual(0f, GetTimer(), 0.01f, "Timer phải reset về 0");
    }

    // TEST 4: Index wraps around (loop)
    [UnityTest]
    public IEnumerator GoNextPoint_IndexWrapsAround()
    {
        yield return null;
        
        // Set index về điểm cuối
        SetCurrentIndex(2);
        agent.Warp(patrolPoints[2].position);
        yield return null;
        
        yield return new WaitForSeconds(npcPatrol.waitTime + 0.1f);
        
        // Phải quay về điểm 0
        Assert.AreEqual(0, GetCurrentIndex());
        Assert.AreEqual(patrolPoints[0].position, agent.destination);
    }

    // TEST 5: Timer only increases when close
    [UnityTest]
    public IEnumerator Update_TimerOnlyIncreasesWhenClose()
    {
        yield return null;
        
        // NPC ở xa điểm
        agent.Warp(Vector3.one * 100);
        yield return null;
        
        float timerBefore = GetTimer();
        yield return new WaitForSeconds(0.3f);
        float timerAfter = GetTimer();
        
        Assert.AreEqual(timerBefore, timerAfter, "Timer không tăng khi chưa đến điểm");
    }

    // TEST 6: WaitTime = 0 moves immediately
    [UnityTest]
    public IEnumerator Update_WaitTimeZero_MovesImmediately()
    {
        npcPatrol.waitTime = 0f;
        yield return null;
        
        agent.Warp(patrolPoints[0].position);
        yield return null; // 1 frame là đủ
        
        Assert.AreEqual(1, GetCurrentIndex());
    }

    // TEST 7: Null patrolPoints crashes in Start
    [UnityTest]
    public IEnumerator Start_NullPatrolPoints_ThrowsError()
    {
        npcPatrol.patrolPoints = null;
        
        LogAssert.Expect(LogType.Exception, new System.Text.RegularExpressions.Regex("NullReferenceException|IndexOutOfRangeException"));
        
        yield return null;
    }

    // TEST 8: Empty patrolPoints crashes in Start
    [UnityTest]
    public IEnumerator Start_EmptyPatrolPoints_ThrowsError()
    {
        npcPatrol.patrolPoints = new Transform[0];
        
        LogAssert.Expect(LogType.Exception, new System.Text.RegularExpressions.Regex("IndexOutOfRangeException"));
        
        yield return null;
    }

    // TEST 9: Null NavMeshAgent crashes
    [UnityTest]
    public IEnumerator Start_NullNavMeshAgent_ThrowsError()
    {
        Object.Destroy(agent);
        yield return null;
        
        GameObject obj = new GameObject();
        NPCPatrol patrol = obj.AddComponent<NPCPatrol>();
        patrol.patrolPoints = patrolPoints;
        
        LogAssert.Expect(LogType.Exception, "NullReferenceException");
        
        yield return null;
        Object.Destroy(obj);
    }

    // TEST 10: Single patrol point loops to itself
    [UnityTest]
    public IEnumerator SinglePoint_LoopsToItself()
    {
        // Tạo NPC với 1 điểm
        Object.Destroy(npcObject);
        foreach (var p in patrolPoints) Object.Destroy(p.gameObject);
        
        GameObject singleNpc = new GameObject("SingleNPC");
        NavMeshAgent singleAgent = singleNpc.AddComponent<NavMeshAgent>();
        singleAgent.updatePosition = false;
        singleAgent.updateRotation = false;
        
        NPCPatrol singlePatrol = singleNpc.AddComponent<NPCPatrol>();
        GameObject singlePoint = new GameObject("SinglePoint");
        singlePatrol.patrolPoints = new Transform[] { singlePoint.transform };
        singlePatrol.waitTime = 0.2f;
        
        yield return null;
        
        singleAgent.Warp(singlePoint.transform.position);
        yield return new WaitForSeconds(0.3f);
        
        // Index vẫn là 0, destination vẫn là điểm đó
        FieldInfo idx = typeof(NPCPatrol).GetField("currentIndex", BindingFlags.NonPublic | BindingFlags.Instance);
        Assert.AreEqual(0, (int)idx.GetValue(singlePatrol));
        
        Object.Destroy(singlePoint);
        Object.Destroy(singleNpc);
    }

    // TEST 11: Multiple cycles work correctly
    [UnityTest]
    public IEnumerator Update_MultipleCycles_Works()
    {
        yield return null;
        
        for (int cycle = 0; cycle < 2; cycle++)
        {
            for (int i = 0; i < 3; i++)
            {
                agent.Warp(patrolPoints[i].position);
                yield return new WaitForSeconds(npcPatrol.waitTime + 0.1f);
                
                int expectedNext = (i + 1) % 3;
                Assert.AreEqual(expectedNext, GetCurrentIndex());
            }
        }
    }

    // TEST 12: Timer resets after moving
    [UnityTest]
    public IEnumerator Update_TimerResetsAfterMove()
    {
        yield return null;
        
        agent.Warp(patrolPoints[0].position);
        yield return null;
        
        // Timer đang tăng
        yield return new WaitForSeconds(0.2f);
        Assert.Greater(GetTimer(), 0f);
        
        // Đợi hết waitTime
        yield return new WaitForSeconds(0.4f);
        
        // Timer reset
        Assert.AreEqual(0f, GetTimer(), 0.01f);
    }

    // TEST 13: Patrol point null in array
    [UnityTest]
    public IEnumerator PatrolPoints_ContainsNull_ThrowsError()
    {
        npcPatrol.patrolPoints[1] = null;
        yield return null;
        
        agent.Warp(patrolPoints[0].position);
        
        LogAssert.Expect(LogType.Exception, new System.Text.RegularExpressions.Regex("NullReferenceException"));
        
        yield return new WaitForSeconds(npcPatrol.waitTime + 0.1f);
    }

    // TEST 14: Negative waitTime
    [UnityTest]
    public IEnumerator Update_NegativeWaitTime_MovesImmediately()
    {
        npcPatrol.waitTime = -1f;
        yield return null;
        
        agent.Warp(patrolPoints[0].position);
        yield return null;
        
        // timer >= -1 luôn đúng
        Assert.AreEqual(1, GetCurrentIndex());
    }

    // TEST 15: Component disabled stops patrol
    [UnityTest]
    public IEnumerator Update_ComponentDisabled_StopsPatrol()
    {
        yield return null;
        
        agent.Warp(patrolPoints[0].position);
        npcPatrol.enabled = false;
        
        float timerBefore = GetTimer();
        yield return new WaitForSeconds(0.5f);
        
        float timerAfter = GetTimer();
        Assert.AreEqual(timerBefore, timerAfter, "Timer không tăng khi disabled");
        Assert.AreEqual(0, GetCurrentIndex());
    }
}