using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Image destinationBar;
    [SerializeField] private Text stageNumberText;
    [SerializeField] private Text envelopesDeliveredText;
    [SerializeField] private Text totalScoreText;
    [SerializeField] private AudioSource inspirationalMessageSound;
    [SerializeField] private AudioSource stageClearSound;

    public float scrollSpeed = 5f;
    public int obstaclesDodged;
    public int targetObstaclesDodged;
    public int envelopesDelivered;
    public int stageNumber;
    public int totalScore;

    private ObstacleSpawner obstacleSpawner;
    private int displayedScore = 0;
    private bool startedDeliveringCoroutine = false;
    private bool scoreTransitionStarted = false;

    private void Start()
    {
        stageNumber = 1;
        obstacleSpawner = FindAnyObjectByType<ObstacleSpawner>();
        envelopesDelivered = 0;

        RandomizeScores();
        StartCoroutine(ShowStageNumberText());
    }

    private void Update()
    {
        destinationBar.fillAmount = Mathf.Lerp(destinationBar.fillAmount, obstaclesDodged / (float)targetObstaclesDodged, Time.deltaTime * 2f);

        if (obstaclesDodged >= targetObstaclesDodged)
        {
            if (startedDeliveringCoroutine) return;

            startedDeliveringCoroutine = true;
            StartCoroutine(WaitForDestroyedObstacles());
        }

        if (totalScore != displayedScore && !scoreTransitionStarted)
        {
            StartCoroutine(TransitionScoreText());
        }
    }

    private IEnumerator TransitionScoreText()
    {
        scoreTransitionStarted = true;
        while (displayedScore < totalScore)
        {
            displayedScore++;
            totalScoreText.text = "Total Score: " + displayedScore.ToString();
            yield return new WaitForSeconds(0.05f);
        }
        displayedScore = totalScore;
        totalScoreText.text = "Total Score: " + displayedScore.ToString();
        scoreTransitionStarted = false;
    }

    private void RandomizeScores()
    {
        obstaclesDodged = 0;
        targetObstaclesDodged = Random.Range(2, 3) * stageNumber;
    }

    private IEnumerator ShowStageNumberText()
    {
        stageClearSound.Play();
        stageNumberText.text = "Stage " + stageNumber.ToString();
        stageNumberText.gameObject.SetActive(true);
        yield return new WaitForSeconds(5.2f);
        stageNumberText.gameObject.SetActive(false);
    }

    private IEnumerator WaitForDestroyedObstacles()
    {
        yield return new WaitUntil(() => GameObject.FindGameObjectsWithTag("Obstacle").Length == 0);

        if (FindAnyObjectByType<PlayerLogic>() == null) yield break;
        FindAnyObjectByType<PlayerLogic>().DropEnvelope();
        inspirationalMessageSound.Play();

        envelopesDelivered++;
        envelopesDeliveredText.text = "Envelopes Delivered: " + envelopesDelivered.ToString();

        if (envelopesDelivered % 2 == 0 && envelopesDelivered > 0)
        {
            yield return new WaitForSeconds(5.2f);
            stageNumber++;
            StartCoroutine(ShowStageNumberText());
        }

        RandomizeScores();
        obstacleSpawner.StartSpawning();
        startedDeliveringCoroutine = false;
    }
}

