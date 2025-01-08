using UnityEngine;
using UnityEngine.UIElements;

public class Pipe : MonoBehaviour
{
    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();

        transform.position = new Vector3(transform.position.x, Random.Range(-3f, 4.5f), 0f);
    }

    private void Update()
    {
        transform.position += Vector3.left * gameManager.scrollSpeed * Time.deltaTime;

        if (transform.position.x < -25f) // If the pipe goes off the screen
        {
            gameManager.totalScore += 10;
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerFly>() != null)
        {
            gameManager.obstaclesDodged++;
            gameManager.totalScore += 10;
        }
    }
}
