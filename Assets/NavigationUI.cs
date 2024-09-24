using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class NavigationUI : MonoBehaviour
{
    public GameObject backButton;                     // Reference to the back button GameObject (child of Navigation)
  
    public CanvasGroup firstCanvasGroup;              // Reference to the first CanvasGroup (initial screen)
    public GameObject entireCanvas;                   // Reference to the entire canvas, excluding the navigation elements
    public VideoPlayer player;
    private CanvasGroup canvasBeforeVideo;            // Tracks the CanvasGroup that led to the 360 video
    private bool isInVideoMode = false;               // Flag to track if the user is in 360 video mode
    private CanvasGroup currentCanvasGroup;           // Stores reference to the current CanvasGroup
    private Stack<CanvasGroup> canvasHistory = new Stack<CanvasGroup>();  // Stack to store the navigation history

    private void Start()
    {
        // Start with the first CanvasGroup and set it as the current canvas
        if (firstCanvasGroup != null)
        {
            currentCanvasGroup = firstCanvasGroup;
            ShowCanvasGroup(currentCanvasGroup);
        }

        // Hide the back button initially since the first canvas has no previous canvas
        backButton.SetActive(false);
    }

    // Call this method to navigate to a specific CanvasGroup
    public void ShowNextCanvas(CanvasGroup nextCanvasGroup)
    {
        if (currentCanvasGroup != null)
        {
            // Push the current canvas onto the stack before switching to the next one
            canvasHistory.Push(currentCanvasGroup);
            HideCanvasGroup(currentCanvasGroup);  // Hide the current canvas
        }

        // Show the next CanvasGroup
        ShowCanvasGroup(nextCanvasGroup);
        currentCanvasGroup = nextCanvasGroup;

        // Show the back button unless we're on the first canvas
        backButton.SetActive(canvasHistory.Count > 0);
    }

    // Special method for handling the 360 video case
    public void Show360Video()
    {
        // Store the canvas before showing the video
        canvasBeforeVideo = currentCanvasGroup;

        // Hide the entire canvas, except for navigation elements
        HideEntireCanvas();

        // The back button should remain visible for the user to exit the video
        backButton.SetActive(true);

        // Set the video mode flag to true
        isInVideoMode = true;
    }

    // Method to handle the back button
    public void OnBackButtonPress()
    {
        if (isInVideoMode)
        {
            // If we are in video mode, return from the video
            ReturnFrom360Video();
        }
        else
        {
            // Otherwise, return to the previous canvas
            ShowPreviousCanvas();
        }
    }

    // Call this method to return from the 360 video
    public void ReturnFrom360Video()
    {
        // Reactivate the entire canvas that was hidden
        ShowEntireCanvas();

        // Stop the video
        if (player != null)
        {
            player.Stop();  // Stop the video when returning from the 360 video
        }

        // Show the canvas that led to the video
        ShowCanvasGroup(canvasBeforeVideo);
        currentCanvasGroup = canvasBeforeVideo;
        canvasBeforeVideo = null;  // Clear the reference after returning

        // Set the video mode flag to false
        isInVideoMode = false;

        // Show the back button only if not returning to the first canvas
        backButton.SetActive(canvasHistory.Count > 0);
    }

    // Call this method to go back to the previous CanvasGroup
    public void ShowPreviousCanvas()
    {
        if (canvasHistory.Count > 0)
        {
            // Hide the current CanvasGroup
            HideCanvasGroup(currentCanvasGroup);

            // Pop the previous CanvasGroup from the stack and show it
            CanvasGroup previousCanvasGroup = canvasHistory.Pop();
            ShowCanvasGroup(previousCanvasGroup);
            currentCanvasGroup = previousCanvasGroup;

            // Hide the back button if we're back to the first canvas
            backButton.SetActive(canvasHistory.Count > 0);
        }
    }

    // Helper method to show a CanvasGroup
    private void ShowCanvasGroup(CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.gameObject.SetActive(true);  // Ensure the CanvasGroup is active
    }

    // Helper method to hide a CanvasGroup
    private void HideCanvasGroup(CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.gameObject.SetActive(false);  // Disable the CanvasGroup
    }

    // Helper method to hide the entire canvas (except for the navigation and back button)
    private void HideEntireCanvas()
    {
        entireCanvas.SetActive(false);  // Hide the entire canvas
    }

    // Helper method to show the entire canvas (if needed)
    private void ShowEntireCanvas()
    {
        entireCanvas.SetActive(true);  // Show the entire canvas
    }
}
