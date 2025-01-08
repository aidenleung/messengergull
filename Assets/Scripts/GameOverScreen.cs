using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using System.Collections;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] private Button retryButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Text totalScoreText;
    [SerializeField] private Text envelopesDeliveredText;
    [SerializeField] private GameObject transitionIn;
    [SerializeField] private AudioSource buttonClick;

    private int displayedScore = 0;
    private int displayedEnvelopesDelivered = 0;

    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        transitionIn.SetActive(false);
    }

    public void StartGameOverThings()
    {
        retryButton.onClick.AddListener(() => { StartCoroutine(TransitionIn(SceneManager.GetActiveScene().buildIndex)); });
        mainMenuButton.onClick.AddListener(() => { StartCoroutine(TransitionIn(0)); });

        StartCoroutine(TransitionScoreText());
        StartCoroutine(TransitionEnvelopeText());
    }

    private IEnumerator TransitionScoreText()
    {
        while (displayedScore < gameManager.totalScore)
        {
            displayedScore++;
            totalScoreText.text = "Total Score: " + displayedScore.ToString();
            yield return new WaitForSeconds(0.005f);
        }
        displayedScore = gameManager.totalScore;
        totalScoreText.text = "Total Score: " + displayedScore.ToString();
    }

    private IEnumerator TransitionEnvelopeText()
    {
        while (displayedEnvelopesDelivered < gameManager.envelopesDelivered)
        {
            displayedEnvelopesDelivered++;
            envelopesDeliveredText.text = "Envelopes Delivered: " + displayedEnvelopesDelivered.ToString();
            yield return new WaitForSeconds(0.1f);
        }
        displayedEnvelopesDelivered = gameManager.envelopesDelivered;
        envelopesDeliveredText.text = "Envelopes Delivered: " + displayedEnvelopesDelivered.ToString();
    }

    private IEnumerator TransitionIn(int sceneNumber)
    {
        buttonClick.Play();
        transitionIn.SetActive(true);
        yield return new WaitForSeconds(0.7f);
        SceneManager.LoadScene(sceneNumber);
    }
}
