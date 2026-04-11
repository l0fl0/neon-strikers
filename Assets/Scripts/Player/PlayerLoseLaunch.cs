using UnityEngine;

public class PlayerLoseLaunch : MonoBehaviour
{
    public Rigidbody2D rb;
    public SpriteRenderer outlineRenderer;

    [Header("Lose Visual")]
    public Color loseOutlineColor = Color.red;

    [Header("Launch")]
    public float upwardForce = 6f;

    private Color originalOutlineColor;

    private void Awake()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();

        if (outlineRenderer != null)
            originalOutlineColor = outlineRenderer.color;
    }

    public void Launch(Vector2 direction, float force)
    {
        if (outlineRenderer != null)
            outlineRenderer.color = loseOutlineColor;

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.AddForce((direction.normalized * force) + Vector2.up * upwardForce, ForceMode2D.Impulse);
            rb.AddTorque(Random.Range(-220f, 220f));
        }
    }

    public void ResetLoseVisual()
    {
        if (outlineRenderer != null)
            outlineRenderer.color = originalOutlineColor;
    }
}