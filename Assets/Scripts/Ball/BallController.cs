using UnityEngine;

public class BallController : MonoBehaviour
{
    public float maxSpeed = 12f;
    public float minStopSpeed = 0.08f;

    private Rigidbody2D rb;
    private ZoneSlowable zoneSlowable;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        zoneSlowable = GetComponent<ZoneSlowable>();
        if (zoneSlowable == null)
        {
            zoneSlowable = gameObject.AddComponent<ZoneSlowable>();
        }
    }

    void FixedUpdate()
    {
        float slowMultiplier = 1f;

        if (zoneSlowable != null)
        {
            slowMultiplier = zoneSlowable.slowMultiplier;
        }

        // Apply local slowdown ONLY if inside zone
        if (slowMultiplier < 0.999f)
        {
            rb.linearVelocity *= slowMultiplier;
        }

        // Keep your original logic intact
        if (rb.linearVelocity.magnitude > maxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
        }

        if (rb.linearVelocity.magnitude < minStopSpeed)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    public void ResetBall(Vector3 position)
    {
        transform.position = position;
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
    }
}