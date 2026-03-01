using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadGameOver()
    {
        Debug.Log("Loading Final Scene");
        SceneManager.LoadScene("Tutorial");
    }

    public void LoadEnding()
    {
        Debug.Log("Loading Ending Scene");
        SceneManager.LoadScene("EndingScene");
    }
}
