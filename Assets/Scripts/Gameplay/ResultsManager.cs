using UnityEngine;
using TMPro;

public class ResultsManager : MonoBehaviour
{
    public TMP_Text winnerText;
    public TMP_Text finalScoreText;

    private void Start()
    {
        Time.timeScale = 1f;

        if (winnerText != null)
        {
            winnerText.text = MatchData.winnerText;
        }

        if (finalScoreText != null)
        {
            finalScoreText.text = "Blue: " + MatchData.blueScore + "   Red: " + MatchData.redScore;
        }
    }
}