using UnityEngine;

public class PlayerKick : MonoBehaviour
{
    public KeyCode kickKey;
    public float kickForce = 10f;
    public float kickRange = 0.8f;
    public LayerMask ballLayer;
    public Transform kickPoint;

    [Header("Kick Cooldown")]
    public float kickCooldown = 0.35f;

    private PlayerMovement playerMovement;
    private float lastKickTime = -999f;

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        UpdateKickPointPosition();

        if (!GameManager.Instance.CanPlayersAct())
            return;

        if (Input.GetKeyDown(kickKey) && Time.time >= lastKickTime + kickCooldown)
        {
            KickBall();
            lastKickTime = Time.time;
        }
    }

    void UpdateKickPointPosition()
    {
        if (kickPoint == null || playerMovement == null)
            return;

        Vector2 dir = playerMovement.facingDirection.normalized;
        kickPoint.localPosition = new Vector3(dir.x * 0.6f, dir.y * 0.6f, 0f);
    }

    void KickBall()
    {
        Collider2D ballHit = Physics2D.OverlapCircle(kickPoint.position, kickRange, ballLayer);

        if (ballHit != null)
        {
            Rigidbody2D ballRb = ballHit.GetComponent<Rigidbody2D>();

            if (ballRb != null)
            {
                Vector2 kickDirection = playerMovement.facingDirection.normalized;
                ballRb.AddForce(kickDirection * kickForce, ForceMode2D.Impulse);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (kickPoint != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(kickPoint.position, kickRange);
        }
    }
}