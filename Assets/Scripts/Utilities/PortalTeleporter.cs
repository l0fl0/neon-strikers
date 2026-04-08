using UnityEngine;
using System.Collections;

public class PortalTeleporter : MonoBehaviour
{
    public Transform exitPoint;
    public float teleportCooldown = 0.25f;

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

        obj.position = exitPoint.position;
    }
}