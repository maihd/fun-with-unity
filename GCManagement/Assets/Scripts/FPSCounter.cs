using UnityEngine;
using TMPro;

public class FPSCounter : MonoBehaviour
{
    private TMP_Text fpsText;
    private int frameCount = 0;
    private float timeElapsed = 0f;

    void Awake()
    {
        // Cache the TextMeshPro component attached to this GameObject
        fpsText = GetComponent<TMP_Text>();
    }

    void Update()
    {
        frameCount++;
        timeElapsed += Time.unscaledDeltaTime;

        // Update the display once every second
        if (timeElapsed >= 1.0f)
        {
            int fps = Mathf.RoundToInt(frameCount / timeElapsed);
            fpsText.text = $"FPS: {fps}";

            // Reset trackers for the next second
            frameCount = 0;
            timeElapsed = 0f;
        }
    }
}