using UnityEngine;
using System.Collections;

public class PortalTeleporter : MonoBehaviour
{
    public Transform exitPoint;
    public float teleportCooldown = 0.25f;
    public float exitSpeedMultiplier = 1.1f;
    public float playerSpeedMultiplier = 0.9f;
    public float minExitSpeed = 5f;
    public float exitOffset = 0.5f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Ball") &&
            !other.CompareTag("Player") &&
            !other.CompareTag("Player2"))
        {
            return;
        }

        PortalCooldown cooldown = other.GetComponent<PortalCooldown>();
        if (cooldown == null)
        {
            cooldown = other.gameObject.AddComponent<PortalCooldown>();
        }

        if (cooldown.CanTeleport())
        {
            cooldown.StartCooldown(teleportCooldown);
            TeleportObject(other.transform);
        }
    }

    void TeleportObject(Transform obj)
    {
        if (exitPoint == null)
            return;

        Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();

        float savedSpeed = 0f;

        if (rb != null)
        {
            savedSpeed = rb.linearVelocity.magnitude;
        }

        obj.position = exitPoint.position + (Vector3)(exitPoint.up * exitOffset);

        if (rb != null)
        {
            Vector2 exitDirection = exitPoint.up.normalized;

            if (savedSpeed < minExitSpeed)
            {
                savedSpeed = minExitSpeed;
            }

            float finalMultiplier = exitSpeedMultiplier;

            if (obj.CompareTag("Player") || obj.CompareTag("Player2"))
            {
                finalMultiplier = playerSpeedMultiplier;
            }

            rb.linearVelocity = exitDirection * savedSpeed * finalMultiplier;
        }
    }
}