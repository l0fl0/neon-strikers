using UnityEngine;

public class PortalCooldown : MonoBehaviour
{
    private float cooldownTimer = 0f;

    void Update()
    {
        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
        }
    }

    public bool CanTeleport()
    {
        return cooldownTimer <= 0f;
    }

    public void StartCooldown(float duration)
    {
        cooldownTimer = duration;
    }
}