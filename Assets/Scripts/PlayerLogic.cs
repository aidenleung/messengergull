using System.Collections;
using UnityEngine;

public class PlayerLogic : MonoBehaviour
{
    [SerializeField] private GameObject heartExplosion;
    [SerializeField] private Transform envelopeSpawnPoint;
    [SerializeField] private GameObject envelopePrefab;
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private AudioClip explosionSound;

    private ScreenShake screenShakeScript;
    private GameObject envelope;

    private void Awake()
    {
        screenShakeScript = FindAnyObjectByType<ScreenShake>();
        SpawnEnvelope();
    }

    private void Update()
    {
        if (Mathf.Abs(transform.position.y) > 11f && transform.position.x > -14f)
        {
            KillPlayer();
        }
    }

    private void SpawnEnvelope()
    {
        envelope = Instantiate(envelopePrefab, envelopeSpawnPoint.position, Quaternion.identity);
        envelope.transform.parent = transform;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.parent != null)
        {
            if (collision.transform.parent.GetComponent<Powerline>() != null)
            {
                GetComponent<Rigidbody2D>().AddForce(Vector2.up * 50f, ForceMode2D.Impulse);
            }
            else
            {
                KillPlayer();
            }
        }
        else
        {
            KillPlayer();
        }
    }

    private void KillPlayer()
    {
        AudioSource.PlayClipAtPoint(explosionSound, transform.position, 1f);
        heartExplosion.transform.parent = null;
        heartExplosion.SetActive(true);
        screenShakeScript.StartShake();
        gameOverScreen.SetActive(true);
        Destroy(gameObject);
    }

    public void DropEnvelope()
    {
        envelope.AddComponent<Rigidbody2D>();
        envelope.transform.parent = null;
        envelope.GetComponent<Envelope>().ShowMessage();
        StartCoroutine(DestroyEnvelope(envelope));
        Invoke("SpawnEnvelope", 0.7f);
    }

    private IEnumerator DestroyEnvelope(GameObject envelope)
    {
        yield return new WaitForSeconds(8f);
        Destroy(envelope);
    }
}
