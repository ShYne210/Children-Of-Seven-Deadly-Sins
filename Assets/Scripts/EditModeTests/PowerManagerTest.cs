using NUnit.Framework;
using UnityEngine;

public class PowerManagerEditTests
{
    private GameObject managerObject;
    private PowerManager powerManager;
    private GameObject doorBlockObject;
    private GameObject puzzleActivatorObject;

    [SetUp]
    public void Setup()
    {
        // Tạo PowerManager
        managerObject = new GameObject("PowerManager");
        powerManager = managerObject.AddComponent<PowerManager>();
        
        // Tạo door block và puzzle activator
        doorBlockObject = new GameObject("DoorBlock");
        puzzleActivatorObject = new GameObject("PuzzleActivator");
        
        // Gán references
        powerManager.doorBlock = doorBlockObject;
        powerManager.puzzleActivator = puzzleActivatorObject;
        powerManager.totalMachines = 3;
        
        // Set initial states
        doorBlockObject.SetActive(true);
        puzzleActivatorObject.SetActive(false);
    }

    [TearDown]
    public void Teardown()
    {
        Object.DestroyImmediate(puzzleActivatorObject);
        Object.DestroyImmediate(doorBlockObject);
        Object.DestroyImmediate(managerObject);
    }

    // TEST 1: Component tồn tại
    [Test]
    public void PowerManager_ComponentExists()
    {
        Assert.IsNotNull(powerManager);
        Assert.IsTrue(powerManager.enabled);
    }

    // TEST 2: Default values
    [Test]
    public void PowerManager_HasDefaultValues()
    {
        GameObject obj = new GameObject();
        PowerManager pm = obj.AddComponent<PowerManager>();
        
        Assert.AreEqual(3, pm.totalMachines, "TotalMachines mặc định phải là 3");
        Assert.IsNull(pm.doorBlock, "DoorBlock mặc định phải null");
        Assert.IsNull(pm.puzzleActivator, "PuzzleActivator mặc định phải null");
        
        Object.DestroyImmediate(obj);
    }

    // TEST 3: TotalMachines có thể thay đổi
    [Test]
    public void PowerManager_TotalMachinesCanBeChanged()
    {
        powerManager.totalMachines = 5;
        Assert.AreEqual(5, powerManager.totalMachines);
        
        powerManager.totalMachines = 1;
        Assert.AreEqual(1, powerManager.totalMachines);
        
        powerManager.totalMachines = 0;
        Assert.AreEqual(0, powerManager.totalMachines);
    }

    // TEST 4: DoorBlock có thể gán
    [Test]
    public void PowerManager_DoorBlockCanBeAssigned()
    {
        GameObject newDoor = new GameObject("NewDoor");
        powerManager.doorBlock = newDoor;
        
        Assert.AreEqual(newDoor, powerManager.doorBlock);
        
        Object.DestroyImmediate(newDoor);
    }

    // TEST 5: PuzzleActivator có thể gán
    [Test]
    public void PowerManager_PuzzleActivatorCanBeAssigned()
    {
        GameObject newPuzzle = new GameObject("NewPuzzle");
        powerManager.puzzleActivator = newPuzzle;
        
        Assert.AreEqual(newPuzzle, powerManager.puzzleActivator);
        
        Object.DestroyImmediate(newPuzzle);
    }

    // TEST 6: References có thể null
    [Test]
    public void PowerManager_ReferencesCanBeNull()
    {
        powerManager.doorBlock = null;
        powerManager.puzzleActivator = null;
        
        Assert.IsNull(powerManager.doorBlock);
        Assert.IsNull(powerManager.puzzleActivator);
    }

    // TEST 7: MonoBehaviour check
    [Test]
    public void PowerManager_IsMonoBehaviour()
    {
        Assert.IsInstanceOf<MonoBehaviour>(powerManager);
    }

    // TEST 8: Component type name
    [Test]
    public void PowerManager_HasCorrectTypeName()
    {
        Assert.AreEqual("PowerManager", powerManager.GetType().Name);
    }

    // TEST 9: GameObject reference
    [Test]
    public void PowerManager_HasCorrectGameObject()
    {
        Assert.AreEqual(managerObject, powerManager.gameObject);
    }

    // TEST 10: TotalMachines có thể âm (edge case)
    [Test]
    public void PowerManager_TotalMachinesCanBeNegative()
    {
        powerManager.totalMachines = -1;
        Assert.AreEqual(-1, powerManager.totalMachines);
        // Lưu ý: Nên validate để không cho phép giá trị âm
    }

    // TEST 11: Extreme totalMachines values
    [Test]
    public void PowerManager_HandlesExtremeTotalMachines()
    {
        powerManager.totalMachines = int.MaxValue;
        Assert.AreEqual(int.MaxValue, powerManager.totalMachines);
        
        powerManager.totalMachines = 0;
        Assert.AreEqual(0, powerManager.totalMachines);
    }

    // TEST 12: DoorBlock active state
    [Test]
    public void PowerManager_DoorBlockCanBeActivatedDeactivated()
    {
        doorBlockObject.SetActive(true);
        Assert.IsTrue(doorBlockObject.activeSelf);
        
        doorBlockObject.SetActive(false);
        Assert.IsFalse(doorBlockObject.activeSelf);
    }

    // TEST 13: PuzzleActivator active state
    [Test]
    public void PowerManager_PuzzleActivatorCanBeActivatedDeactivated()
    {
        puzzleActivatorObject.SetActive(false);
        Assert.IsFalse(puzzleActivatorObject.activeSelf);
        
        puzzleActivatorObject.SetActive(true);
        Assert.IsTrue(puzzleActivatorObject.activeSelf);
    }

    // TEST 14: Multiple PowerManagers
    [Test]
    public void PowerManager_CanHaveMultipleInstances()
    {
        GameObject obj1 = new GameObject();
        PowerManager pm1 = obj1.AddComponent<PowerManager>();
        pm1.totalMachines = 3;
        
        GameObject obj2 = new GameObject();
        PowerManager pm2 = obj2.AddComponent<PowerManager>();
        pm2.totalMachines = 5;
        
        Assert.AreNotEqual(pm1.totalMachines, pm2.totalMachines);
        
        Object.DestroyImmediate(obj2);
        Object.DestroyImmediate(obj1);
    }

    // TEST 15: Properties readable
    [Test]
    public void PowerManager_PropertiesAreReadable()
    {
        powerManager.totalMachines = 7;
        
        int readTotal = powerManager.totalMachines;
        GameObject readDoor = powerManager.doorBlock;
        GameObject readPuzzle = powerManager.puzzleActivator;
        
        Assert.AreEqual(7, readTotal);
        Assert.AreEqual(doorBlockObject, readDoor);
        Assert.AreEqual(puzzleActivatorObject, readPuzzle);
    }
}