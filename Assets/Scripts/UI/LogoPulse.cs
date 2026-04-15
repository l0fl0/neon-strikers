using UnityEngine;

public class LogoPulse : MonoBehaviour
{
    public float pulseSpeed = 2f;
    public float pulseAmount = 0.03f;

    private Vector3 startScale;

    void Start()
    {
        startScale = transform.localScale;
    }

    void Update()
    {
        float scale = 1f + Mathf.Sin(Time.time * pulseSpeed) * pulseAmount;
        transform.localScale = startScale * scale;
    }
}