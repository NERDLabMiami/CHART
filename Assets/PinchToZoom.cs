using UnityEngine;

public class PinchToZoom : MonoBehaviour
{
    public float minZoom = 0.5f;      // Minimum zoom level
    public float maxZoom = 2.0f;      // Maximum zoom level
    private RectTransform imageRectTransform;  // Reference to the RectTransform of the image

    private float initialTouchDistance;        // Distance between fingers for pinch-to-zoom
    private Vector3 initialScale;              // Initial scale before pinch-to-zoom

    private void Awake()
    {
        // Get the RectTransform component
        imageRectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        HandlePinchZoom();
    }

    // Handle pinch-to-zoom functionality
    private void HandlePinchZoom()
    {
        if (Input.touchCount == 2)  // Check if two fingers are touching the screen
        {
            // Get the two touches
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            // Calculate the current distance between the two touches
            float currentTouchDistance = Vector2.Distance(touchZero.position, touchOne.position);

            // If this is the first pinch (initial touch), set the initial distance and scale
            if (initialTouchDistance == 0)
            {
                initialTouchDistance = currentTouchDistance;  // Save the initial distance
                initialScale = imageRectTransform.localScale;  // Save the initial scale
            }
            else
            {
                // Calculate the scaling factor by comparing the current distance to the initial distance
                float scaleFactor = currentTouchDistance / initialTouchDistance;

                // Apply the scaling factor to the initial scale
                Vector3 newScale = initialScale * scaleFactor;

                // Clamp the new scale to the minZoom and maxZoom limits
                newScale.x = Mathf.Clamp(newScale.x, minZoom, maxZoom);
                newScale.y = Mathf.Clamp(newScale.y, minZoom, maxZoom);
                newScale.z = 1;  // Keep the z-axis scale at 1 to avoid distortions

                // Apply the new scale to the image
                imageRectTransform.localScale = newScale;
            }
        }
        else
        {
            // Reset the initial touch distance when no pinch is happening
            initialTouchDistance = 0;
        }
    }
}
