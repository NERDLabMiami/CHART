using UnityEngine;
using TMPro;
using UnityEngine.Video;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.UI;

public class Controls : MonoBehaviour
{

    public TextMeshProUGUI title;
    public VideoPlayer video;
    public GameObject navigationUI;
    [SerializeField] private RectTransform controlPanel; // Control panel to animate
    [SerializeField] private float tweenDuration = 0.5f; // Time for the animation
    [SerializeField] private float targetYPosition = 0f; // On-screen target position

    private Vector2 offScreenPosition; // Initial off-screen position
    private bool isPanelVisible = false; // Track panel visibility

    private CHART playerInput; // Reference to PlayerInput component


    private void Awake()
    {
        playerInput = new CHART(); // Initialize the input system

        // Subscribe to the Tap action
        playerInput.UI.Tap.performed += ctx => StartCoroutine(HandleTapNextFrame());
        
            // Set the off-screen position and hide the panel initially
            offScreenPosition = new Vector2(controlPanel.anchoredPosition.x, -controlPanel.rect.height);
            controlPanel.anchoredPosition = offScreenPosition;
        }

    private void OnEnable()
    {
        playerInput.Enable(); // Enable input when the object is enabled
    }

    private void OnDisable()
    {
        playerInput.Disable(); // Disable input when the object is disabled
    }
    public void PlayChapter(Chapter chapter)
    {
        title.text = chapter.title.text;  // Update the UI with the chapter title
        video.clip = chapter.videoClip;   // Load the associated video clip
        video.Play();  // Start playing the video
    }

    public void StopVideo()
    {
        video.Stop();  // Stop the video playback.
    }

    public void PauseVideo()
    {
        if (video.isPlaying)
        {
            video.Pause();
        }
        else
        {
            video.Play();
        }
    }

    // Handle tap processing in the next frame
    private IEnumerator HandleTapNextFrame()
    {
        yield return null;  // Wait for the next frame to ensure UI state is updated

        Vector2 tapPosition = Pointer.current.position.ReadValue();  // Get the tap position

        // 1. Check if the tap hit any UI element (buttons, other UI components)
        if (IsTapOnUIElement(tapPosition))
        {
            Debug.Log("Tapped on a UI element.");
            yield break;  // Don't change panel visibility if any UI element was tapped
        }

        // 2. If the panel is visible and the tap is outside, hide it
        if (isPanelVisible && !IsTapInsideControlPanel(tapPosition))
        {
            Debug.Log("Hiding the panel.");
            HidePanel();
        }
        else if (!isPanelVisible)  // 3. If the panel is not visible, show it
        {
            Debug.Log("Showing the panel.");
            ShowPanel();
        }
    }

    private bool IsTapOnUIElement(Vector2 tapPosition)
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = tapPosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        // Filter out the RawImage (or other elements) that should not block taps
        foreach (RaycastResult result in results)
        {
            if (result.gameObject.GetComponent<RawImage>() != null)
            {
                Debug.Log("Tap detected on RawImage, ignoring.");
                continue;  // Ignore taps on RawImage
            }

            Debug.Log($"Tap detected on UI element: {result.gameObject.name}");
            return true;  // If any other UI element is hit, return true
        }

        return false;  // No significant UI elements were hit
    }

    private bool IsTapInsideControlPanel(Vector2 tapPosition)
    {
        // Check if the tap is inside the control panel's area
        return RectTransformUtility.RectangleContainsScreenPoint(controlPanel, tapPosition);
    }

    private void ShowPanel()
    {
        isPanelVisible = true;
        navigationUI.SetActive(true);
        // Tween the panel from off-screen to on-screen position
        LeanTween.moveY(controlPanel, targetYPosition, tweenDuration).setEase(LeanTweenType.easeOutCubic);
    }

    private void HidePanel()
    {
        isPanelVisible = false;
        navigationUI.SetActive(false);

        // Tween the panel back to off-screen and deactivate it
        LeanTween.moveY(controlPanel, offScreenPosition.y, tweenDuration).setEase(LeanTweenType.easeInCubic)
            .setOnComplete(() =>
            {

                Debug.Log("Panel fully hidden");
            });
    }
}
