using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResultsManager : MonoBehaviour
{
    public TMP_Text winnerText;
    public TMP_Text finalScoreText;
    public Image backgroundImage;
    public RectTransform ghostRect;
    public Image ghostImage;

    public Sprite blueWinsBackground;
    public Sprite redWinsBackground;

    public Vector2 ghostLeftStart = new Vector2(-300f, -40f);
    public Vector2 ghostRightStart = new Vector2(300f, -40f);

    public float riseSpeed = 40f;
    public float swayAmount = 12f;
    public float swaySpeed = 2f;
    public float pulseAmount = 0.05f;
    public float pulseSpeed = 2f;
    public float visibleTime = 1.5f;
    public float fadeDuration = 2f;

    private Vector2 ghostStartPos;
    private Vector3 ghostBaseScale;
    private Color ghostBaseColor;
    private bool animateGhost = false;
    private float ghostTimer = 0f;

    private void Start()
    {
        Time.timeScale = 1f;

        if (finalScoreText != null)
        {
            finalScoreText.text = "Blue: " + MatchData.blueMatchWins + " Red: " + MatchData.redMatchWins;
        }

        bool blueWins = MatchData.blueMatchWins > MatchData.redMatchWins;
        bool redWins = MatchData.redMatchWins > MatchData.blueMatchWins;
        bool tie = MatchData.blueMatchWins == MatchData.redMatchWins;

        if (winnerText != null)
        {
            if (blueWins)
                winnerText.text = "Blue Wins!";
            else if (redWins)
                winnerText.text = "Red Wins!";
            else
                winnerText.text = "It's a Tie!";
        }

        if (backgroundImage != null)
        {
            if (blueWins && blueWinsBackground != null)
                backgroundImage.sprite = blueWinsBackground;
            else if (redWins && redWinsBackground != null)
                backgroundImage.sprite = redWinsBackground;
        }

        if (ghostRect != null)
            ghostBaseScale = ghostRect.localScale;

        if (ghostImage != null)
            ghostBaseColor = ghostImage.color;

        if (ghostRect != null && ghostImage != null)
        {
            if (tie)
            {
                ghostImage.enabled = false;
                animateGhost = false;
                return;
            }

            ghostImage.enabled = true;

            if (blueWins)
                ghostStartPos = ghostRightStart;
            else
                ghostStartPos = ghostLeftStart;

            ghostRect.anchoredPosition = ghostStartPos;
            ghostRect.localScale = ghostBaseScale;

            Color c = ghostImage.color;
            c.a = ghostBaseColor.a;
            ghostImage.color = c;

            ghostTimer = 0f;
            animateGhost = true;
        }
    }

    private void Update()
    {
        if (!animateGhost || ghostRect == null || ghostImage == null)
            return;

        ghostTimer += Time.deltaTime;

        float yOffset = riseSpeed * ghostTimer;
        float xOffset = Mathf.Sin(ghostTimer * swaySpeed) * swayAmount;
        ghostRect.anchoredPosition = ghostStartPos + new Vector2(xOffset, yOffset);

        float pulse = 1f + Mathf.Sin(ghostTimer * pulseSpeed) * pulseAmount;
        ghostRect.localScale = ghostBaseScale * pulse;

        if (ghostTimer > visibleTime)
        {
            float fadeT = (ghostTimer - visibleTime) / fadeDuration;
            fadeT = Mathf.Clamp01(fadeT);

            Color c = ghostImage.color;
            c.a = Mathf.Lerp(ghostBaseColor.a, 0f, fadeT);
            ghostImage.color = c;

            if (fadeT >= 1f)
                animateGhost = false;
        }
    }
}