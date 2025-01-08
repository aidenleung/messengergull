using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Saw : MonoBehaviour
{
    [SerializeField] private GameObject sawVisual;
    [SerializeField] private GameObject sawWarning;
    [SerializeField] private float switchPosDelay = 2f;
    [SerializeField] private float maxVolume = 1f; // Maximum volume for the saw
    [SerializeField] private float minDistance = 1f; // Minimum distance for maximum volume
    [SerializeField] private float maxDistance = 30f; // Maximum distance for zero volume

    private GameManager gameManager;
    private Vector3 targetPos = Vector3.zero;
    private LineRenderer lineRenderer;
    private AudioSource audioSource;

    private void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        lineRenderer = GetComponent<LineRenderer>();
        transform.position = new Vector3(transform.position.x, Random.Range(-6.5f, 6.5f), 0f);
        sawWarning.transform.parent = null;
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(MoveSaw());
    }

    private void Update()
    {
        if (targetPos != Vector3.zero)
        {
            targetPos = new Vector3(transform.position.x, targetPos.y, targetPos.z);
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * 5f);
        }
        sawWarning.transform.position += Vector3.left * gameManager.scrollSpeed * Time.deltaTime;
        transform.position += Vector3.left * gameManager.scrollSpeed * Time.deltaTime;

        float rotationSpeedMultiplier = 100f;
        sawVisual.transform.Rotate(new Vector3(0f, 0f, gameManager.scrollSpeed * rotationSpeedMultiplier) * Time.deltaTime);
        sawWarning.transform.rotation = sawVisual.transform.rotation;

        lineRenderer.SetPosition(0, sawVisual.transform.position);
        lineRenderer.SetPosition(1, sawWarning.transform.position);

        if (transform.position.x < -25f) // If the saw goes off the screen
        {
            Destroy(sawWarning);
            Destroy(gameObject);
        }

        UpdateSawVolume();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerFly>() != null)
        {
            gameManager.obstaclesDodged++;
            gameManager.totalScore += 10;
        }
    }

    private IEnumerator MoveSaw()
    {
        while (true)
        {
            yield return new WaitForSeconds(switchPosDelay);
            Vector3 randomPos = new Vector3(transform.position.x, Random.Range(-6.5f, 6.5f), 0f);

            while (Vector3.Distance(sawWarning.transform.position, randomPos) > 0.1f)
            {
                randomPos = new Vector3(transform.position.x, randomPos.y, randomPos.z);
                sawWarning.transform.position = Vector3.Lerp(sawWarning.transform.position, randomPos, Time.deltaTime * 5f);
                yield return null;
            }
            sawWarning.transform.position = randomPos;

            targetPos = randomPos;
        }
    }

    private void UpdateSawVolume()
    {
        // Find the player position
        PlayerFly player = FindAnyObjectByType<PlayerFly>();
        if (player != null)
        {
            float distance = Vector2.Distance(transform.position, player.transform.position);

            // Normalize distance to volume range
            float normalizedDistance = Mathf.Clamp01((maxDistance - distance) / (maxDistance - minDistance));

            // Set the volume based on the normalized distance
            audioSource.volume = Mathf.Lerp(0f, maxVolume, normalizedDistance);
        }
    }
}
