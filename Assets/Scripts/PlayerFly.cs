using System.Collections;
using UnityEngine;

public class PlayerFly : MonoBehaviour
{
    [SerializeField] private float flapStrength = 5f;
    [SerializeField] private GameObject inspirationalMessage;
    [SerializeField] private AudioClip flapSound;


    int count = 1;

    private GameManager gameManager;
    private Rigidbody2D rb;
    private bool playerControlEnabled = false;
    private bool enableControlsCoroutineStarted = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        gameManager = FindAnyObjectByType<GameManager>();
    }

    public void EnablePlayerControl()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
        playerControlEnabled = true;
        if (transform.position.y < 6.8f)
        {
            rb.AddForce(Vector2.up * flapStrength, ForceMode2D.Impulse);
            AudioSource.PlayClipAtPoint(flapSound, transform.position, 1f);
        }
    }

    private void Update()
    {
        if (!playerControlEnabled) return;

        if (Input.GetKeyDown(KeyCode.S)) {
            ScreenCapture.CaptureScreenshot($"/Users/garyleung/Desktop/ss-player-{count++}.png");
        }

        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            rb.linearVelocity = Vector2.up * flapStrength;
            AudioSource.PlayClipAtPoint(flapSound, transform.position, 1f);
        }

        if (transform.position.x < -6.96f)
        {
            rb.AddForce(Vector2.right * 2f, ForceMode2D.Force);
        }
        else
        {
            transform.position = new Vector3(-6.96f, transform.position.y, transform.position.z);
        }

        if (inspirationalMessage.activeSelf)
        {
            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Kinematic;
            playerControlEnabled = false;
            if (!enableControlsCoroutineStarted)
                StartCoroutine(EnableControlsAfterMessage());
        }
    }

    private IEnumerator EnableControlsAfterMessage()
    {
        enableControlsCoroutineStarted = true;
        yield return new WaitUntil(() => !inspirationalMessage.activeSelf);
        EnablePlayerControl();
        enableControlsCoroutineStarted = false;
    }
}
