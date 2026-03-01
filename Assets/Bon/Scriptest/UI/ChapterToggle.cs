using UnityEngine;

public class ChapterToggle : MonoBehaviour
{
    [SerializeField] private GameObject chapterPanel; // Panel chứa các nút Chapter con
    [SerializeField] private float animDuration = 0.3f;

    private bool isOpen = false;
    private CanvasGroup panelGroup;
    private Coroutine animCoroutine;

    void Start()
    {
        panelGroup = chapterPanel.GetComponent<CanvasGroup>();
        if (panelGroup == null)
        {
            panelGroup = chapterPanel.AddComponent<CanvasGroup>();
        }

        // Ban đầu ẩn
        panelGroup.alpha = 0;
        panelGroup.interactable = false;
        panelGroup.blocksRaycasts = false;
        chapterPanel.SetActive(false);
    }

    public void TogglePanel()
    {
        if (animCoroutine != null) StopCoroutine(animCoroutine);
        if (isOpen)
        {
            animCoroutine = StartCoroutine(HidePanel());
        }
        else
        {
            animCoroutine = StartCoroutine(ShowPanel());
        }
        isOpen = !isOpen;
    }

    private System.Collections.IEnumerator ShowPanel()
    {
        chapterPanel.SetActive(true);
        float t = 0;
        while (t < animDuration)
        {
            t += Time.deltaTime;
            float progress = t / animDuration;
            panelGroup.alpha = Mathf.Lerp(0, 1, progress);
            yield return null;
        }
        panelGroup.alpha = 1;
        panelGroup.interactable = true;
        panelGroup.blocksRaycasts = true;
    }

    private System.Collections.IEnumerator HidePanel()
    {
        float t = 0;
        while (t < animDuration)
        {
            t += Time.deltaTime;
            float progress = t / animDuration;
            panelGroup.alpha = Mathf.Lerp(1, 0, progress);
            yield return null;
        }
        panelGroup.alpha = 0;
        panelGroup.interactable = false;
        panelGroup.blocksRaycasts = false;
        chapterPanel.SetActive(false);
    }
}
