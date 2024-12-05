using System.Collections;
using UnityEngine;

public class Slide : MonoBehaviour
{
    public AudioClip clip; // Audio clip associated with this slide
    public FocalPoint[] focalPoints; // Array of focal points
    public float durationMultiplier = 1f; // Adjusts the relative length of this slide's display time

    private int currentFocalPointIndex = 0;
    private bool isTransitioning = false;
    public AudioSource audioSource; // Direct reference to the AudioSource (assign manually in editor)
    public bool slideCompleted = false;

    void Start()
    {
        // Log to check if AudioSource is assigned
        if (audioSource != null)
        {
            Debug.Log($"AudioSource is assigned to Slide {gameObject.name}");
        }
        else
        {
            Debug.LogError($"AudioSource is null for Slide {gameObject.name}. Please assign it in the editor.");
        }
    }

    public void StartSlide()
    {
        // Check if the AudioClip is null
        if (clip == null)
        {
            Debug.LogError($"Slide {gameObject.name} does not have an AudioClip assigned. Make sure the clip is set in the editor.");
            slideCompleted = true; // If no clip, mark the slide as complete
            return;
        }

        // Check if the AudioSource is null
        if (audioSource == null)
        {
            Debug.LogError($"AudioSource is null for Slide {gameObject.name}. Cannot play audio.");
            slideCompleted = true; // If there's no audio source, mark the slide as complete
            return;
        }

        // Play the assigned audio clip
        Debug.Log($"Playing audio for Slide {gameObject.name}, Clip: {clip.name}");
        audioSource.Stop();
        audioSource.clip = clip;
        audioSource.Play();

        // Start moving through the focal points
        StartCoroutine(MoveToFocalPoints());
    }

    private IEnumerator MoveToFocalPoints()
    {
        Debug.Log($"Moving through focal points for Slide {gameObject.name}");

        // Iterate through each focal point
        for (currentFocalPointIndex = 0; currentFocalPointIndex < focalPoints.Length; currentFocalPointIndex++)
        {
            FocalPoint focalPoint = focalPoints[currentFocalPointIndex];
            yield return MoveToFocalPoint(focalPoint);
        }

        // Wait for the audio clip to finish
        yield return new WaitUntil(() => !audioSource.isPlaying);

        slideCompleted = true; // Mark the slide as completed
    }

    private IEnumerator MoveToFocalPoint(FocalPoint focalPoint)
    {
        isTransitioning = true;

        // Get the current RectTransform of the slide
        RectTransform slideRect = GetComponent<RectTransform>();
        if (slideRect == null)
        {
            Debug.LogError($"RectTransform not found on the Slide: {gameObject.name}");
            yield break;
        }

        Vector2 initialPosition = slideRect.anchoredPosition;
        Vector2 targetPosition = focalPoint.GetPosition();

        // Invert the coordinates if necessary (flipping X and Y as required)
        targetPosition.x = -targetPosition.x;
        targetPosition.y = -targetPosition.y;

        // Get the initial and target scale based on zoomLevel
        Vector3 initialScale = slideRect.localScale;
        Vector3 targetScale = Vector3.one * focalPoint.zoomLevel;

        float elapsedTime = 0f;
        float focalPointDuration = (clip.length / focalPoints.Length) * durationMultiplier;

        while (elapsedTime < focalPointDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / focalPointDuration;

            // Move and scale the slide towards the focal point
            slideRect.anchoredPosition = Vector2.Lerp(initialPosition, targetPosition, progress);
            slideRect.localScale = Vector3.Lerp(initialScale, targetScale, progress);

            yield return null;
        }

        // Ensure the slide reaches the exact focal point position and scale
        slideRect.anchoredPosition = targetPosition;
        slideRect.localScale = targetScale;

        // Pause briefly at the focal point
        yield return new WaitForSeconds(0.5f); // Adjust pause duration as needed

        isTransitioning = false;
    }
}
