using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 4f;
    public float liftForce = 8f;
    public float directionalLiftForce = 2f;

    [Header("Controls")]
    public KeyCode upKey;
    public KeyCode downKey;
    public KeyCode leftKey;
    public KeyCode rightKey;
    public KeyCode actionKey;

    [Header("Pulse Ball Interaction")]
    public float pulseRadius = 1.5f;
    public float pulseForce = 12f;
    public float softPossessionRadius = 1.8f;
    public float softPossessionForce = 3f;
    public LayerMask ballLayer;

    [Header("Pulse Effect")]
    public GameObject pulsePrefab;
    public Color pulseColor = Color.cyan;

    [HideInInspector] public Vector2 facingDirection = Vector2.right;

    private Rigidbody2D rb;
    private Vector2 inputDirection;
    private bool canMove = true;
    private Rigidbody2D ballRb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        GameObject ballObj = GameObject.FindGameObjectWithTag("Ball");
        if (ballObj != null)
        {
            ballRb = ballObj.GetComponent<Rigidbody2D>();
        }
    }

    void Update()
    {
        if (!canMove)
        {
            inputDirection = Vector2.zero;
            return;
        }

        float x = 0f;
        float y = 0f;

        if (Input.GetKey(leftKey)) x = -1f;
        if (Input.GetKey(rightKey)) x = 1f;
        if (Input.GetKey(upKey)) y = 1f;
        if (Input.GetKey(downKey)) y = -1f;

        inputDirection = new Vector2(x, y).normalized;

        if (inputDirection.x < 0)
            facingDirection = Vector2.left;
        else if (inputDirection.x > 0)
            facingDirection = Vector2.right;

        if (Input.GetKeyDown(actionKey))
        {
            DoLift();
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(inputDirection.x * moveSpeed, rb.linearVelocity.y);
        ApplySoftPossession();
    }

    void DoLift()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);

        rb.AddForce(Vector2.up * liftForce, ForceMode2D.Impulse);
        rb.AddForce(inputDirection * directionalLiftForce, ForceMode2D.Impulse);

        SpawnPulse();
        PulseBall();
    }

    void PulseBall()
    {
        Collider2D ballHit = Physics2D.OverlapCircle(transform.position, pulseRadius, ballLayer);

        if (ballHit != null)
        {
            Rigidbody2D hitBallRb = ballHit.GetComponent<Rigidbody2D>();

            if (hitBallRb != null)
            {
                Vector2 direction;

                if (inputDirection != Vector2.zero)
                    direction = inputDirection;
                else
                    direction = Vector2.up;

                Vector2 finalDirection = (direction.normalized + Vector2.up * 0.35f).normalized;
                hitBallRb.AddForce(finalDirection * pulseForce, ForceMode2D.Impulse);
            }
        }
    }

    void ApplySoftPossession()
    {
        if (ballRb == null || !canMove)
            return;

        float distance = Vector2.Distance(transform.position, ballRb.position);

        if (distance <= softPossessionRadius)
        {
            Vector2 toPlayer = ((Vector2)transform.position - ballRb.position).normalized;
            ballRb.AddForce(toPlayer * softPossessionForce);
        }
    }

    void SpawnPulse()
    {
        if (pulsePrefab == null)
            return;

        GameObject pulse = Instantiate(pulsePrefab, transform.position, Quaternion.identity);

        SpriteRenderer sr = pulse.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.color = pulseColor;
        }
    }

    public void SetMovementEnabled(bool enabled)
    {
        canMove = enabled;

        if (!enabled)
        {
            inputDirection = Vector2.zero;
            rb.linearVelocity = Vector2.zero;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, pulseRadius);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, softPossessionRadius);
    }
}