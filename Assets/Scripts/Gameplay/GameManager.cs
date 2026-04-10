using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Lives")]
    public int blueLives = 3;
    public int redLives = 3;

    [Header("Match Settings")]
    public float goalResetDelay = 1.5f;

    [Header("References")]
    public BallController ball;
    public Transform ballSpawnPoint;
    public Transform player1;
    public Transform player2;
    public Transform player1SpawnPoint;
    public Transform player2SpawnPoint;

    [Header("Goal Burst VFX")]
    public ParticleSystem leftGoalBurst;
    public ParticleSystem rightGoalBurst;

    [Header("UI")]
    public TextMeshProUGUI centerMessageText;

    [Header("Blue Life Images")]
    public Image[] blueLifeImages;

    [Header("Red Life Images")]
    public Image[] redLifeImages;

    private bool gameEnded = false;
    private bool playersCanAct = false;
    private bool goalSequenceRunning = false;

    private PlayerMovement p1Movement;
    private PlayerMovement p2Movement;
    private Rigidbody2D p1Rb;
    private Rigidbody2D p2Rb;

    private Coroutine goalRoutine;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        CacheReferences();
        UpdateLivesUI();
        StartCoroutine(StartMatchSequence());
    }

    public bool CanPlayersAct()
    {
        return playersCanAct && !gameEnded && !goalSequenceRunning;
    }

    void CacheReferences()
    {
        if (player1 != null)
        {
            p1Movement = player1.GetComponent<PlayerMovement>();
            p1Rb = player1.GetComponent<Rigidbody2D>();
        }

        if (player2 != null)
        {
            p2Movement = player2.GetComponent<PlayerMovement>();
            p2Rb = player2.GetComponent<Rigidbody2D>();
        }
    }

    IEnumerator StartMatchSequence()
    {
        ResetPositions();
        yield return StartCoroutine(CountdownRoutine());
        EnableGameplay();
    }

    IEnumerator CountdownRoutine()
    {
        DisableGameplay();

        if (centerMessageText != null)
            centerMessageText.text = "3";
        yield return new WaitForSeconds(1f);

        if (centerMessageText != null)
            centerMessageText.text = "2";
        yield return new WaitForSeconds(1f);

        if (centerMessageText != null)
            centerMessageText.text = "1";
        yield return new WaitForSeconds(1f);

        if (centerMessageText != null)
            centerMessageText.text = "GO!";
        yield return new WaitForSeconds(0.7f);

        if (centerMessageText != null)
            centerMessageText.text = "";
    }

    public void GoalScored(string scoringTeam)
    {
        if (!CanPlayersAct())
            return;

        if (goalRoutine != null)
            StopCoroutine(goalRoutine);

        goalRoutine = StartCoroutine(HandleGoal(scoringTeam));
    }

    IEnumerator HandleGoal(string scoringTeam)
    {
        goalSequenceRunning = true;
        DisableGameplay();

        PlayGoalBurst(scoringTeam);

        ApplyGoalResult(scoringTeam);
        UpdateLivesUI();
        ShowGoalMessage(scoringTeam);

        yield return new WaitForSeconds(goalResetDelay);

        if (blueLives <= 0 || redLives <= 0)
        {
            EndMatch();
            yield break;
        }

        ResetPositions();

        yield return StartCoroutine(CountdownRoutine());

        goalSequenceRunning = false;
        goalRoutine = null;
        EnableGameplay();
    }

    void PlayGoalBurst(string scoringTeam)
    {
        // scoringTeam tells you which team scored.
        // If Blue scores, the ball entered the RIGHT goal.
        // If Red scores, the ball entered the LEFT goal.
        if (scoringTeam == "Blue")
        {
            if (rightGoalBurst != null)
                rightGoalBurst.Play();
        }
        else if (scoringTeam == "Red")
        {
            if (leftGoalBurst != null)
                leftGoalBurst.Play();
        }
    }

    void ApplyGoalResult(string scoringTeam)
    {
        if (scoringTeam == "Blue")
        {
            redLives = Mathf.Max(0, redLives - 1);
        }
        else if (scoringTeam == "Red")
        {
            blueLives = Mathf.Max(0, blueLives - 1);
        }
        else
        {
            Debug.LogWarning("Unknown scoring team: " + scoringTeam);
        }
    }

    void ShowGoalMessage(string scoringTeam)
    {
        if (centerMessageText != null)
        {
            centerMessageText.text = scoringTeam + " SCORES!";
        }
    }

    void ResetPositions()
    {
        ResetBall();
        ResetPlayer(player1, player1SpawnPoint, p1Rb);
        ResetPlayer(player2, player2SpawnPoint, p2Rb);
    }

    void ResetBall()
    {
        if (ball != null && ballSpawnPoint != null)
        {
            ball.ResetBall(ballSpawnPoint.position);
        }
    }

    void ResetPlayer(Transform playerTransform, Transform spawnPoint, Rigidbody2D playerRb)
    {
        if (playerTransform == null || spawnPoint == null)
            return;

        playerTransform.position = spawnPoint.position;

        if (playerRb != null)
        {
            playerRb.linearVelocity = Vector2.zero;
            playerRb.angularVelocity = 0f;
        }
    }

    void EnableGameplay()
    {
        playersCanAct = true;
        SetPlayerControl(true);
    }

    void DisableGameplay()
    {
        playersCanAct = false;
        SetPlayerControl(false);
    }

    void SetPlayerControl(bool enabled)
    {
        if (p1Movement != null)
            p1Movement.SetMovementEnabled(enabled);

        if (p2Movement != null)
            p2Movement.SetMovementEnabled(enabled);
    }

    void UpdateLivesUI()
    {
        UpdateLifeArray(blueLifeImages, blueLives);
        UpdateLifeArray(redLifeImages, redLives);
    }

    void UpdateLifeArray(Image[] images, int currentLives)
    {
        if (images == null)
            return;

        for (int i = 0; i < images.Length; i++)
        {
            if (images[i] != null)
            {
                images[i].enabled = i < currentLives;
            }
        }
    }

    void EndMatch()
    {
        if (gameEnded)
            return;

        gameEnded = true;
        goalSequenceRunning = false;
        DisableGameplay();

        MatchData.blueScore = blueLives;
        MatchData.redScore = redLives;

        if (blueLives > redLives)
            MatchData.winnerText = "Blue Wins!";
        else if (redLives > blueLives)
            MatchData.winnerText = "Red Wins!";
        else
            MatchData.winnerText = "It's a Tie!";

        SceneManager.LoadScene("Results");
    }
}