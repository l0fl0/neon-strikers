using UnityEngine;

public class GoalZone : MonoBehaviour
{
    public string scoringTeam;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ball"))
        {
            GameManager.Instance.GoalScored(scoringTeam);
        }
    }
}