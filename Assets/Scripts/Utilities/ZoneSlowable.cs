using UnityEngine;

public class ZoneSlowable : MonoBehaviour
{
    public float slowMultiplier = 1f;

    public void SetSlow(float multiplier)
    {
        slowMultiplier = multiplier;
    }

    public void ResetSlow()
    {
        slowMultiplier = 1f;
    }
}