using UnityEngine;
using System.Collections;

public class LightFlicker : MonoBehaviour
{
    public Light targetLight;
    public float minIntensity = 0.2f;
    public float maxIntensity = 2f;
    public float flickerSpeed = 0.1f;

    void Start()
    {
        if (targetLight == null) targetLight = GetComponent<Light>();
        StartCoroutine(FlickerRoutine());
    }

    IEnumerator FlickerRoutine()
    {
        while (true)
        {
            targetLight.intensity = Random.Range(minIntensity, maxIntensity);
            yield return new WaitForSeconds(flickerSpeed);
        }
    }
}
