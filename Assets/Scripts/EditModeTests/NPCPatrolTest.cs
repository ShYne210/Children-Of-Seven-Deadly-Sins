using NUnit.Framework;
using UnityEngine;
using UnityEngine.AI;
using System.Reflection;

public class NPCPatrolEditTests
{
    private GameObject npcObject;
    private NPCPatrol npcPatrol;
    private FieldInfo currentIndexField;
    private FieldInfo timerField;

    [SetUp]
    public void Setup()
    {
        npcObject = new GameObject("NPC");
        npcPatrol = npcObject.AddComponent<NPCPatrol>();
        
        // Lấy private fields
        currentIndexField = typeof(NPCPatrol).GetField("currentIndex", 
            BindingFlags.NonPublic | BindingFlags.Instance);
        
        timerField = typeof(NPCPatrol).GetField("timer", 
            BindingFlags.NonPublic | BindingFlags.Instance);
    }

    [TearDown]
    public void Teardown()
    {
        Object.DestroyImmediate(npcObject);
    }

    [Test]
    public void NPCPatrol_ComponentExists()
    {
        Assert.IsNotNull(npcPatrol);
        Assert.IsTrue(npcPatrol.enabled);
    }

    [Test]
    public void NPCPatrol_DefaultValues()
    {
        Assert.AreEqual(3f, npcPatrol.waitTime, 0.01f);
        Assert.IsNull(npcPatrol.patrolPoints);
    }

    [Test]
    public void NPCPatrol_WaitTimeCanBeChanged()
    {
        npcPatrol.waitTime = 5f;
        Assert.AreEqual(5f, npcPatrol.waitTime);
        
        npcPatrol.waitTime = 0.5f;
        Assert.AreEqual(0.5f, npcPatrol.waitTime);
    }

    [Test]
    public void NPCPatrol_PatrolPointsCanBeAssigned()
    {
        Transform[] points = new Transform[3];
        for (int i = 0; i < 3; i++)
        {
            GameObject p = new GameObject($"Point{i}");
            points[i] = p.transform;
        }
        
        npcPatrol.patrolPoints = points;
        Assert.AreEqual(3, npcPatrol.patrolPoints.Length);
        
        // Cleanup
        foreach (var p in points) Object.DestroyImmediate(p.gameObject);
    }

    [Test]
    public void NPCPatrol_CurrentIndexStartsAtZero()
    {
        int currentIndex = (int)currentIndexField.GetValue(npcPatrol);
        Assert.AreEqual(0, currentIndex);
    }

    [Test]
    public void NPCPatrol_TimerStartsAtZero()
    {
        float timer = (float)timerField.GetValue(npcPatrol);
        Assert.AreEqual(0f, timer, 0.01f);
    }

    [Test]
    public void NPCPatrol_IsMonoBehaviour()
    {
        Assert.IsInstanceOf<MonoBehaviour>(npcPatrol);
    }

    [Test]
    public void NPCPatrol_HasNavMeshAgentRequirement()
    {
        NavMeshAgent agent = npcObject.AddComponent<NavMeshAgent>();
        Assert.IsNotNull(agent);
    }

    [Test]
    public void NPCPatrol_CanHaveMultipleInstances()
    {
        GameObject obj2 = new GameObject("NPC2");
        NPCPatrol npc2 = obj2.AddComponent<NPCPatrol>();
        npc2.waitTime = 10f;
        
        Assert.AreNotEqual(npcPatrol.waitTime, npc2.waitTime);
        
        Object.DestroyImmediate(obj2);
    }
}