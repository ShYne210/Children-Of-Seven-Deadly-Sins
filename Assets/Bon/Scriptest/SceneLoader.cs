using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadGameOver()
    {
        Debug.Log("Loading Final Scene");
        SceneManager.LoadScene("Final");
    }

    public void LoadEnding()
    {
        Debug.Log("Loading Ending Scene");
        SceneManager.LoadScene("EndingScene");
    }
}
