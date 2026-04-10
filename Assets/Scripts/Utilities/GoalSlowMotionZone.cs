using UnityEngine;

public class GoalSlowMotionZone : MonoBehaviour
{
    public float slowTimeScale = 0.6f;
    public float normalTimeScale = 1f;
    public float transitionSpeed = 5f;

    private bool ballInside = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ball"))
        {
            ballInside = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Ball"))
        {
            ballInside = false;
        }
    }

    private void Update()
    {
        float targetTimeScale = ballInside ? slowTimeScale : normalTimeScale;

        Time.timeScale = Mathf.Lerp(Time.timeScale, targetTimeScale, Time.unscaledDeltaTime * transitionSpeed);
    }
}