using UnityEngine;

public class LightPulse : MonoBehaviour
{
    public Light targetLight;
    public float minIntensity = 0.2f;
    public float maxIntensity = 2f;
    public float pulseSpeed = 2f;

    void Start()
    {
        if (targetLight == null) targetLight = GetComponent<Light>();
    }

    void Update()
    {
        // dùng sin để tạo nhịp sáng yếu → mạnh → yếu
        float t = (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f;
        targetLight.intensity = Mathf.Lerp(minIntensity, maxIntensity, t);
    }
}
