using UnityEngine;

public class BallController : MonoBehaviour
{
    public float maxSpeed = 12f;
    public float minStopSpeed = 0.08f;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
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