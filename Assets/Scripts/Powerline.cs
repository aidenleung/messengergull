using UnityEngine;

public class Powerline : MonoBehaviour
{
    [SerializeField] private int length;
    [SerializeField] private GameObject powerlineStart;
    [SerializeField] private GameObject powerlineContinuation;

    private float xoffset;
    private GameManager gameManager;

    private static Powerline activePowerline;

    private void Start()
    {
        // Check if another Powerline is active and destroy this one if it isn't the first
        if (activePowerline != null && activePowerline != this)
        {
            Destroy(gameObject);
            return;
        }

        // Set this as the active Powerline
        activePowerline = this;

        gameManager = FindAnyObjectByType<GameManager>();
        transform.position = new Vector3(transform.position.x, -8.5f, 0f);
        length = Random.Range(5, 20);
        GeneratePowerline();
    }

    private void Update()
    {
        transform.position += Vector3.left * gameManager.scrollSpeed * Time.deltaTime;

        // Destroy the Powerline if it goes out of bounds
        if (transform.position.x < -25f - xoffset)
        {
            gameManager.totalScore += 5 * length;
            Destroy(gameObject);

            // Reset the activePowerline reference when this Powerline is destroyed
            if (activePowerline == this)
            {
                activePowerline = null;
            }
        }
    }

    private void GeneratePowerline()
    {
        // Instantiate the powerline start
        GameObject instantiated = Instantiate(powerlineStart, transform.position, Quaternion.identity);
        instantiated.transform.parent = transform;
        xoffset += 2.7f;

        // Instantiate the continuation sections
        for (int i = 0; i < length - 1; i++)
        {
            instantiated = Instantiate(powerlineContinuation, new Vector3(transform.position.x + xoffset, transform.position.y, transform.position.z), Quaternion.identity);
            instantiated.transform.parent = transform;
            xoffset += 4.5f;
        }
    }
}
