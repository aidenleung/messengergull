using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] obstacles;
    [SerializeField] private float spawnDelay = 1f;
    [SerializeField] private GameManager gameManager;

    private void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        StartSpawning();
    }

    public void DestroyObstacles()
    {
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        foreach (GameObject obstacle in obstacles)
        {
            Destroy(obstacle);
        }
    }

    public void StartSpawning()
    {
        StartCoroutine(SpawnObstacle());
    }

    private IEnumerator SpawnObstacle()
    {
        yield return new WaitUntil(() => gameManager.targetObstaclesDodged > 0);
        for (int i = 0; i < gameManager.targetObstaclesDodged; i++)
        {
            yield return new WaitForSeconds(spawnDelay);
            GameObject obstacle = Instantiate(obstacles[Random.Range(0, obstacles.Length)], transform.position, Quaternion.identity);
            if (obstacle.GetComponent<Powerline>() != null)
            {
                i--;
            }
        }
    }

}
