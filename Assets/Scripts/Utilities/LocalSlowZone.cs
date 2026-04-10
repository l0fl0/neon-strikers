using UnityEngine;
using System.Collections.Generic;

public class LocalSlowZone : MonoBehaviour
{
    [Header("Slow values")]
    public float ballSlowMultiplier = 0.65f;
    public float playerSlowMultiplier = 0.8f;

    private Dictionary<GameObject, ZoneSlowable> slowedObjects = new Dictionary<GameObject, ZoneSlowable>();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Ball") &&
            !other.CompareTag("Player") &&
            !other.CompareTag("Player2"))
        {
            return;
        }

        ZoneSlowable slowable = other.GetComponent<ZoneSlowable>();
        if (slowable == null)
        {
            slowable = other.gameObject.AddComponent<ZoneSlowable>();
        }

        if (other.CompareTag("Ball"))
        {
            slowable.SetSlow(ballSlowMultiplier);
        }
        else
        {
            slowable.SetSlow(playerSlowMultiplier);
        }

        slowedObjects[other.gameObject] = slowable;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (slowedObjects.TryGetValue(other.gameObject, out ZoneSlowable slowable))
        {
            slowable.ResetSlow();
            slowedObjects.Remove(other.gameObject);
        }
    }

    private void OnDisable()
    {
        foreach (var pair in slowedObjects)
        {
            if (pair.Value != null)
            {
                pair.Value.ResetSlow();
            }
        }

        slowedObjects.Clear();
    }
}