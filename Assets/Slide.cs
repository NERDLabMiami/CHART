using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slide : MonoBehaviour
{
    public AudioClip clip;
    public float waitTime = 1f; // Wait time before transitioning to the next focal point
    public float transitionDuration = 1f; // Duration of transition between focal points
    public List<FocalPoint> focalPoints;

    private int currentFocalPointIndex = 0; // Index of the current focal point
    private Vector3 initialPosition; // Initial position of the parent image
    private Vector3 targetPosition; // Target position for the transition
    private float transitionTimer = 0f; // Timer for transition duration
    private float waitTimer = 0f; // Timer for wait time
    private bool transitioning = false; // Flag to indicate if transitioning
    public bool slideCompleted = false; // Flag to indicate if sliding is complete
    private Slideshow parentSlideshow;

    void Start()
    {
        initialPosition = transform.localPosition;
    }

    public void StartTransition(Slideshow slideshow)
    {
        parentSlideshow = slideshow;

        if (focalPoints.Count == 0 || currentFocalPointIndex >= focalPoints.Count)
        {
            if (clip)
            {
                Debug.Log("Has Clip: " + clip.length);
                Invoke("SlideComplete", clip.length);
                DelayDeactivation(clip.length * 2);
            }
            else
            {
                SlideComplete();
            }
            return;
        }

        if (focalPoints[currentFocalPointIndex])
        {
            if (focalPoints[currentFocalPointIndex].timeToFocus > waitTime)
            {
                waitTime = focalPoints[currentFocalPointIndex].timeToFocus;
            }
        }

        targetPosition = -focalPoints[currentFocalPointIndex].transform.localPosition;
        transitioning = true;
        transitionTimer = 0f;
    }

    void Update()
    {
        if (transitioning)
        {
            transitionTimer += Time.deltaTime;
            float progress = transitionTimer / transitionDuration;

            transform.localPosition = Vector3.Lerp(initialPosition, targetPosition, progress);

            if (progress >= 1f)
            {
                waitTimer += Time.deltaTime;

                if (waitTimer >= waitTime)
                {
                    transitioning = false;
                    waitTimer = 0f;

                    currentFocalPointIndex++;

                    if (currentFocalPointIndex >= focalPoints.Count)
                    {
                        SlideComplete();
                        return;
                    }

                    targetPosition = -focalPoints[currentFocalPointIndex].transform.localPosition;
                    initialPosition = transform.localPosition;
                    transitioning = true;
                    transitionTimer = 0f;
                }
            }
        }
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }

    public void DelayDeactivation(float time)
    {
        Invoke("Deactivate", time);
    }

    void SlideComplete()
    {
        slideCompleted = true;
        Debug.Log("Slide completed");
    }
}
