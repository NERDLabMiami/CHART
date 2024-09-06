using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slide : MonoBehaviour
{
    public AudioClip clip; // Audio clip associated with the slide
    public List<FocalPoint> focalPoints; // List of focal points to move to
    public float transitionDuration = 1f; // Duration for the movement between focal points

    private int currentFocalPointIndex = 0; // Track current focal point
    public bool slideCompleted = false; // Track slide completion
    private Slideshow parentSlideshow; // Reference to the parent slideshow
    private AudioSource cameraAudioSource; // AudioSource attached to the camera

    private Vector3 currentScale; // Track the current scale of the image

    void Start()
    {
        // Set the initial zoom level to the current scale
        currentScale = transform.localScale = Vector3.one;

        // Find the AudioSource attached to the camera
        cameraAudioSource = Camera.main.GetComponent<AudioSource>();

        // Play audio if there is a clip and the AudioSource is available
        if (clip != null && cameraAudioSource != null)
        {
            cameraAudioSource.clip = clip;
            cameraAudioSource.Play();
        }

        // Begin the transition to the first focal point if available
        if (focalPoints.Count > 0)
        {
            StartCoroutine(MoveToFocalPoint(focalPoints[currentFocalPointIndex]));
        }
    }

    public void StartTransition(Slideshow slideshow)
    {
        parentSlideshow = slideshow;

        // If no focal points or we're done with all focal points, complete the slide
        if (focalPoints.Count == 0 || currentFocalPointIndex >= focalPoints.Count)
        {
            SlideComplete();
            return;
        }

        StartCoroutine(MoveToFocalPoint(focalPoints[currentFocalPointIndex]));
    }

    IEnumerator MoveToFocalPoint(FocalPoint focalPoint)
    {
        RectTransform slideRectTransform = GetComponent<RectTransform>();

        // Get the current position of the slide and the target position (relative to the focal point)
        Vector2 initialPosition = slideRectTransform.anchoredPosition;
        Vector2 targetPosition = GetTargetPosition(focalPoint);
        Vector3 targetScale = Vector3.one * focalPoint.zoomLevel;

        float elapsedTime = 0f;

        // Smooth movement and zoom to the focal point over the transition duration
        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / transitionDuration;

            // Move the image toward the target position
            slideRectTransform.anchoredPosition = Vector2.Lerp(initialPosition, targetPosition, progress);

            // Gradually scale the image to reach the desired zoom level
            slideRectTransform.localScale = Vector3.Lerp(currentScale, targetScale, progress);

            yield return null;
        }

        // Ensure final alignment of slide's center with the focal point and set the final zoom level
        slideRectTransform.anchoredPosition = targetPosition;
        slideRectTransform.localScale = targetScale;

        // Set the current scale to the final scale for smooth transitions to the next focal point
        currentScale = targetScale;

        // Move to the next focal point if available
        currentFocalPointIndex++;
        if (currentFocalPointIndex < focalPoints.Count)
        {
            StartTransition(parentSlideshow);
        }
        else
        {
            SlideComplete();
        }
    }

    // Calculate the target position to move the center of the slide to the center of the focal point
    private Vector2 GetTargetPosition(FocalPoint focalPoint)
    {
        RectTransform focalRectTransform = focalPoint.GetComponent<RectTransform>();
        RectTransform slideRectTransform = GetComponent<RectTransform>();

        // The focal point's position is relative to the parent slide
        return -focalRectTransform.anchoredPosition;
    }

    // Mark the slide as complete and inform the parent slideshow
    public void SlideComplete()
    {
        slideCompleted = true;

        // Inform the parent slideshow
        if (parentSlideshow != null)
        {
            parentSlideshow.OnSlideComplete();
        }
    }
}
