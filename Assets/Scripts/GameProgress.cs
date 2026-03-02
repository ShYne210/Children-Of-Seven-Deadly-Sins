using UnityEngine;

public class GameProgress : MonoBehaviour
{
    public static GameProgress instance;

    public bool game1Completed = false;

    void Awake()
    {
        instance = this;
    }

    public void CompleteGame1()
    {
        game1Completed = true;
        Debug.Log("Game 1 Completed");
    }
}