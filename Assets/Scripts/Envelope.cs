using System.Collections;
using UnityEngine;
using UnityEngine.UI;

using Random = UnityEngine.Random;

public class Envelope : MonoBehaviour
{
    [SerializeField] private string[] messages;
    private Text messageText;

    int count = 0;

    private void Start()
    {
        // Get the TextMeshProUGUI component instead of Text
        messageText = GameObject.FindGameObjectWithTag("Message").transform.GetChild(0).transform.GetChild(0).GetComponent<Text>();
    }

        void Update()
    {

        if (Input.GetKeyDown(KeyCode.S)) {
            ScreenCapture.CaptureScreenshot($"/Users/garyleung/Desktop/ss-message-{count++}.png");
        }
    }

    public void ShowMessage()
    {
        StartCoroutine(ShowMessageCoroutine());
    }

    private IEnumerator ShowMessageCoroutine()
    {
        if (messageText == null) yield break;
        messageText.text = messages[Random.Range(0, messages.Length)];

        messageText.transform.parent.gameObject.SetActive(true);
        yield return new WaitForSeconds(5.2f);
        messageText.transform.parent.gameObject.SetActive(false);
    }
}
