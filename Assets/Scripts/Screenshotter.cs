using UnityEngine;

public class Screenshotter : MonoBehaviour
{
    int count = 0;

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.S)) {
            ScreenCapture.CaptureScreenshot($"/Users/garyleung/Desktop/ss-menu-{count++}.png");
        }
    }
}
