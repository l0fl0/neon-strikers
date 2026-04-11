using System.Collections;
using UnityEngine;

public class PlayerLoseEffects : MonoBehaviour
{
    [Header("References")]
    public SpriteRenderer bodyRenderer;
    public SpriteRenderer outlineRenderer;
    public Rigidbody2D rb;

    [Header("Lose Flash")]
    public Color loseOutlineColor = Color.red;
    public float redFlashDuration = 1.2f;
    public float flashSpeed = 12f;

    [Header("Blast Reaction")]
    public float upwardKick = 4f;

    private Color originalOutlineColor;
    private bool isFlashing = false;

    private void Awake()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();

        if (outlineRenderer != null)
            originalOutlineColor = outlineRenderer.color;
    }

    public void PlayLoseEffects(Vector2 blastDirection, float blastForce)
    {
        // Push player
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.AddForce((blastDirection.normalized * blastForce) + Vector2.up * upwardKick, ForceMode2D.Impulse);
        }

        // Start red flashing outline
        if (!isFlashing && outlineRenderer != null)
            StartCoroutine(FlashRedOutline());
    }

    private IEnumerator FlashRedOutline()
    {
        isFlashing = true;

        float timer = 0f;
        while (timer < redFlashDuration)
        {
            timer += Time.unscaledDeltaTime;

            float pulse = (Mathf.Sin(timer * flashSpeed) + 1f) * 0.5f;
            outlineRenderer.color = Color.Lerp(originalOutlineColor, loseOutlineColor, pulse);

            yield return null;
        }

        outlineRenderer.color = loseOutlineColor;
    }

    public void ResetEffects()
    {
        StopAllCoroutines();
        isFlashing = false;

        if (outlineRenderer != null)
            outlineRenderer.color = originalOutlineColor;
    }
}