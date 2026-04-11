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

    [Header("Animation")]
    public Animator animator;
    public SpriteRenderer spriteRenderer;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.15f;
    public LayerMask groundLayer;

    [HideInInspector] public Vector2 facingDirection = Vector2.right;

    private Rigidbody2D rb;
    private Vector2 inputDirection;
    private bool canMove = true;
    private Rigidbody2D ballRb;
    private ZoneSlowable zoneSlowable;
    private bool isGrounded;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        zoneSlowable = GetComponent<ZoneSlowable>();
        if (zoneSlowable == null)
        {
            zoneSlowable = gameObject.AddComponent<ZoneSlowable>();
        }

        if (animator == null)
            animator = GetComponent<Animator>();

        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
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
        UpdateGroundedState();
        HandleInput();
        UpdateFacingDirection();
        UpdateAnimator();
    }

    void FixedUpdate()
    {
        if (!canMove)
            return;

        float slowMultiplier = 1f;

        if (zoneSlowable != null)
        {
            slowMultiplier = zoneSlowable.slowMultiplier;
        }

        rb.linearVelocity = new Vector2(inputDirection.x * moveSpeed * slowMultiplier, rb.linearVelocity.y);
        ApplySoftPossession();
    }

    void HandleInput()
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

        if (Input.GetKeyDown(actionKey))
        {
            DoLift();
        }
    }

    void UpdateGroundedState()
    {
        if (groundCheck != null)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        }
    }

    void UpdateFacingDirection()
    {
        float horizontal = canMove ? inputDirection.x : rb.linearVelocity.x;

        if (horizontal < -0.05f)
            facingDirection = Vector2.left;
        else if (horizontal > 0.05f)
            facingDirection = Vector2.right;

        if (spriteRenderer != null)
        {
            if (horizontal < -0.05f)
                spriteRenderer.flipX = true;
            else if (horizontal > 0.05f)
                spriteRenderer.flipX = false;
        }
    }

    void UpdateAnimator()
    {
        if (animator == null)
            return;

        float animationSpeed = canMove ? Mathf.Abs(inputDirection.x) : Mathf.Abs(rb.linearVelocity.x);

        animator.SetFloat("Speed", animationSpeed);
        animator.SetBool("isGrounded", isGrounded);
    }

    void DoLift()
    {
        float slowMultiplier = 1f;

        if (zoneSlowable != null)
        {
            slowMultiplier = Mathf.Lerp(1f, zoneSlowable.slowMultiplier, 0.5f);
        }

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);

        rb.AddForce(Vector2.up * liftForce * slowMultiplier, ForceMode2D.Impulse);
        rb.AddForce(inputDirection * directionalLiftForce * slowMultiplier, ForceMode2D.Impulse);

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
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, pulseRadius);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, softPossessionRadius);

        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}