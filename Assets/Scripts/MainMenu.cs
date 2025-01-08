using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private AudioSource buttonClick;
    [SerializeField] private GameObject transitionIn;

    private void Start()
    {
        transitionIn.SetActive(false);
        startButton.onClick.AddListener(() =>
        {
            StartCoroutine(StartGame());
        });
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.S)) {
            StartCoroutine(StartGame());
        }
    }

    private IEnumerator StartGame()
    {
        transitionIn.SetActive(true);
        buttonClick.Play();
        yield return new WaitForSeconds(0.7f);
        SceneManager.LoadScene(1);
    }
}