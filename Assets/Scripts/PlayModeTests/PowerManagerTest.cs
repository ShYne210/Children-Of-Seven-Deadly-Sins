using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Reflection;

public class PowerManagerPlayTests
{
    private GameObject managerObject;
    private PowerManager powerManager;
    private GameObject doorBlockObject;
    private GameObject puzzleActivatorObject;
    private FieldInfo repairedMachinesField;

    [SetUp]
    public void Setup()
    {
        managerObject = new GameObject("PowerManager");
        powerManager = managerObject.AddComponent<PowerManager>();
        
        doorBlockObject = new GameObject("DoorBlock");
        puzzleActivatorObject = new GameObject("PuzzleActivator");
        
        powerManager.doorBlock = doorBlockObject;
        powerManager.puzzleActivator = puzzleActivatorObject;
        powerManager.totalMachines = 3;
        
        doorBlockObject.SetActive(true);
        puzzleActivatorObject.SetActive(false);
        
        // Lấy private field repairedMachines
        repairedMachinesField = typeof(PowerManager).GetField("repairedMachines", 
            BindingFlags.NonPublic | BindingFlags.Instance);
    }

    [TearDown]
    public void Teardown()
    {
        Object.Destroy(puzzleActivatorObject);
        Object.Destroy(doorBlockObject);
        Object.Destroy(managerObject);
    }

    // TEST 1: MachineRepaired tăng counter
    [UnityTest]
    public IEnumerator MachineRepaired_IncreasesCounter()
    {
        int initialCount = (int)repairedMachinesField.GetValue(powerManager);
        Assert.AreEqual(0, initialCount, "Ban đầu phải là 0");
        
        powerManager.MachineRepaired();
        yield return null;
        
        int newCount = (int)repairedMachinesField.GetValue(powerManager);
        Assert.AreEqual(1, newCount, "Phải tăng lên 1");
    }

    // TEST 2: MachineRepaired nhiều lần
    [UnityTest]
    public IEnumerator MachineRepaired_CalledMultipleTimes_IncreasesCorrectly()
    {
        powerManager.MachineRepaired();
        yield return null;
        Assert.AreEqual(1, (int)repairedMachinesField.GetValue(powerManager));
        
        powerManager.MachineRepaired();
        yield return null;
        Assert.AreEqual(2, (int)repairedMachinesField.GetValue(powerManager));
        
        powerManager.MachineRepaired();
        yield return null;
        Assert.AreEqual(3, (int)repairedMachinesField.GetValue(powerManager));
    }

    // TEST 3: Đạt totalMachines - deactivate door
    [UnityTest]
    public IEnumerator MachineRepaired_ReachesTotal_DeactivatesDoorBlock()
    {
        Assert.IsTrue(doorBlockObject.activeSelf, "Door ban đầu phải active");
        
        // Sửa đủ 3 máy
        for (int i = 0; i < 3; i++)
        {
            powerManager.MachineRepaired();
            yield return null;
        }
        
        Assert.IsFalse(doorBlockObject.activeSelf, "Door phải bị deactivate");
    }

    // TEST 4: Đạt totalMachines - activate puzzle
    [UnityTest]
    public IEnumerator MachineRepaired_ReachesTotal_ActivatesPuzzle()
    {
        Assert.IsFalse(puzzleActivatorObject.activeSelf, "Puzzle ban đầu phải inactive");
        
        for (int i = 0; i < 3; i++)
        {
            powerManager.MachineRepaired();
            yield return null;
        }
        
        Assert.IsTrue(puzzleActivatorObject.activeSelf, "Puzzle phải được activate");
    }

    // TEST 5: Complete flow test
    [UnityTest]
    public IEnumerator MachineRepaired_CompleteFlow_WorksCorrectly()
    {
        // Initial state
        Assert.AreEqual(0, (int)repairedMachinesField.GetValue(powerManager));
        Assert.IsTrue(doorBlockObject.activeSelf);
        Assert.IsFalse(puzzleActivatorObject.activeSelf);
        
        // Repair machine 1
        powerManager.MachineRepaired();
        yield return null;
        Assert.AreEqual(1, (int)repairedMachinesField.GetValue(powerManager));
        Assert.IsTrue(doorBlockObject.activeSelf, "Door vẫn block");
        Assert.IsFalse(puzzleActivatorObject.activeSelf, "Puzzle chưa active");
        
        // Repair machine 2
        powerManager.MachineRepaired();
        yield return null;
        Assert.AreEqual(2, (int)repairedMachinesField.GetValue(powerManager));
        Assert.IsTrue(doorBlockObject.activeSelf, "Door vẫn block");
        Assert.IsFalse(puzzleActivatorObject.activeSelf, "Puzzle chưa active");
        
        // Repair machine 3 - COMPLETE!
        powerManager.MachineRepaired();
        yield return null;
        Assert.AreEqual(3, (int)repairedMachinesField.GetValue(powerManager));
        Assert.IsFalse(doorBlockObject.activeSelf, "Door đã mở");
        Assert.IsTrue(puzzleActivatorObject.activeSelf, "Puzzle đã active");
    }

    // TEST 6: Null doorBlock không crash
    [UnityTest]
    public IEnumerator MachineRepaired_NullDoorBlock_DoesNotCrash()
    {
        powerManager.doorBlock = null;
        
        // Không nên crash
        for (int i = 0; i < 3; i++)
        {
            powerManager.MachineRepaired();
            yield return null;
        }
        
        Assert.Pass("Không crash khi doorBlock null");
    }

    // TEST 7: Null puzzleActivator không crash
    [UnityTest]
    public IEnumerator MachineRepaired_NullPuzzleActivator_DoesNotCrash()
    {
        powerManager.puzzleActivator = null;
        
        for (int i = 0; i < 3; i++)
        {
            powerManager.MachineRepaired();
            yield return null;
        }
        
        Assert.Pass("Không crash khi puzzleActivator null");
    }

    // TEST 8: Cả 2 null không crash
    [UnityTest]
    public IEnumerator MachineRepaired_BothNull_DoesNotCrash()
    {
        powerManager.doorBlock = null;
        powerManager.puzzleActivator = null;
        
        for (int i = 0; i < 3; i++)
        {
            powerManager.MachineRepaired();
            yield return null;
        }
        
        Assert.AreEqual(3, (int)repairedMachinesField.GetValue(powerManager));
    }

    // TEST 9: Vượt quá totalMachines
    [UnityTest]
    public IEnumerator MachineRepaired_ExceedsTotal_StillWorks()
    {
        powerManager.totalMachines = 3;
        
        // Sửa 5 máy (vượt quá 3)
        for (int i = 0; i < 5; i++)
        {
            powerManager.MachineRepaired();
            yield return null;
        }
        
        Assert.AreEqual(5, (int)repairedMachinesField.GetValue(powerManager));
        Assert.IsFalse(doorBlockObject.activeSelf);
        Assert.IsTrue(puzzleActivatorObject.activeSelf);
    }

    // TEST 10: TotalMachines = 1
    [UnityTest]
    public IEnumerator MachineRepaired_TotalMachinesOne_CompletesImmediately()
    {
        powerManager.totalMachines = 1;
        
        powerManager.MachineRepaired();
        yield return null;
        
        Assert.IsFalse(doorBlockObject.activeSelf);
        Assert.IsTrue(puzzleActivatorObject.activeSelf);
    }

    // TEST 11: TotalMachines = 0 (edge case)
    [UnityTest]
    public IEnumerator MachineRepaired_TotalMachinesZero_CompletesImmediately()
    {
        powerManager.totalMachines = 0;
        
        powerManager.MachineRepaired();
        yield return null;
        
        // repairedMachines = 1 >= totalMachines = 0
        Assert.IsFalse(doorBlockObject.activeSelf);
        Assert.IsTrue(puzzleActivatorObject.activeSelf);
    }

    // TEST 12: Debug.Log được gọi
    [UnityTest]
    public IEnumerator MachineRepaired_LogsProgress()
    {
        LogAssert.Expect(LogType.Log, "Machine Fixed: 1");
        powerManager.MachineRepaired();
        yield return null;
        
        LogAssert.Expect(LogType.Log, "Machine Fixed: 2");
        powerManager.MachineRepaired();
        yield return null;
    }

    // TEST 13: "POWER RESTORED" log khi hoàn thành
    [UnityTest]
    public IEnumerator MachineRepaired_ReachesTotal_LogsPowerRestored()
    {
        powerManager.totalMachines = 2;
        
        powerManager.MachineRepaired();
        yield return null;
        
        LogAssert.Expect(LogType.Log, "Machine Fixed: 2");
        LogAssert.Expect(LogType.Log, "POWER RESTORED");
        
        powerManager.MachineRepaired();
        yield return null;
    }

    // TEST 14: TotalMachines lớn
    [UnityTest]
    public IEnumerator MachineRepaired_LargeTotalMachines_WorksCorrectly()
    {
        powerManager.totalMachines = 10;
        
        for (int i = 0; i < 9; i++)
        {
            powerManager.MachineRepaired();
            yield return null;
            
            Assert.IsTrue(doorBlockObject.activeSelf, $"Door vẫn block ở {i+1}/10");
            Assert.IsFalse(puzzleActivatorObject.activeSelf);
        }
        
        // Máy thứ 10
        powerManager.MachineRepaired();
        yield return null;
        
        Assert.IsFalse(doorBlockObject.activeSelf);
        Assert.IsTrue(puzzleActivatorObject.activeSelf);
    }

    // TEST 15: Component disabled không ảnh hưởng MachineRepaired
    [UnityTest]
    public IEnumerator MachineRepaired_ComponentDisabled_StillWorks()
    {
        powerManager.enabled = false;
        
        powerManager.MachineRepaired();
        yield return null;
        
        // Vẫn hoạt động vì là public method
        Assert.AreEqual(1, (int)repairedMachinesField.GetValue(powerManager));
    }
}