using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

public class PinchToZoom : MonoBehaviour
{
    public float minZoom = 0.5f;        // Minimum zoom level
    public float maxZoom = 2.0f;        // Maximum zoom level
    public float zoomSpeed = 0.1f;      // Zoom speed with the mouse scroll

    private RectTransform imageRectTransform;  // Reference to the RectTransform of the image
    private Vector3 initialScale;         // Initial scale of the image

    private Vector2 dragStartPos;         // Starting position for drag
    private Vector3 imageStartPos;        // Image's position at the start of the drag
    private bool isDragging = false;      // Dragging state
    private Vector2 initialTouchDistance; // Initial distance between touch points

    private Camera mainCamera;

    private void Awake()
    {
        // Initialize EnhancedTouch (New Input System)
        EnhancedTouchSupport.Enable();

        // Get the RectTransform component of the image
        imageRectTransform = GetComponent<RectTransform>();
        mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        // Enable EnhancedTouch when script is enabled
        EnhancedTouchSupport.Enable();
    }

    private void OnDisable()
    {
        // Disable EnhancedTouch when script is disabled
        EnhancedTouchSupport.Disable();
    }

    private void Update()
    {
#if UNITY_EDITOR
        HandleMouseZoom();
        HandleMouseDrag();
#else
        HandlePinchZoom();
        HandlePanDrag();
#endif
    }

    // Handle pinch-to-zoom using mouse scroll in editor
    private void HandleMouseZoom()
    {
        if (Mouse.current != null)
        {
            // Get mouse scroll delta
            float scrollDelta = Mouse.current.scroll.y.ReadValue();

            if (scrollDelta != 0)
            {
                // Calculate new scale
                Vector3 newScale = imageRectTransform.localScale;
                newScale += Vector3.one * scrollDelta * zoomSpeed;

                // Clamp the zoom level
                newScale.x = Mathf.Clamp(newScale.x, minZoom, maxZoom);
                newScale.y = Mathf.Clamp(newScale.y, minZoom, maxZoom);
                newScale.z = 1; // Keep the z scale fixed

                // Apply the new scale
                imageRectTransform.localScale = newScale;

                // Ensure the image stays within bounds
                ClampToScreenBounds();
            }
        }
    }

    // Handle dragging using mouse in editor
    private void HandleMouseDrag()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            isDragging = true;
            dragStartPos = Mouse.current.position.ReadValue();
            imageStartPos = imageRectTransform.localPosition;
        }

        if (isDragging && Mouse.current.leftButton.isPressed)
        {
            // Calculate drag delta
            Vector2 dragDelta = Mouse.current.position.ReadValue() - dragStartPos;

            // Update image position
            imageRectTransform.localPosition = imageStartPos + new Vector3(dragDelta.x, dragDelta.y, 0);

            // Clamp the image within the screen bounds
            ClampToScreenBounds();
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            isDragging = false;
        }
    }

    // Handle pinch-to-zoom using the New Input System
    private void HandlePinchZoom()
    {
        if (UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches.Count == 2)
        {
            var touch0 = UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches[0];
            var touch1 = UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches[1];

            float currentTouchDistance = Vector2.Distance(touch0.screenPosition, touch1.screenPosition);

            // If it's the first pinch, set the initial touch distance and scale
            if (initialTouchDistance == Vector2.zero)
            {
                initialTouchDistance = new Vector2(currentTouchDistance, currentTouchDistance);
                initialScale = imageRectTransform.localScale;
            }
            else
            {
                // Calculate the scaling factor
                float scaleFactor = currentTouchDistance / initialTouchDistance.x;

                // Apply scaling and clamp to min/max zoom limits
                Vector3 newScale = initialScale * scaleFactor;
                newScale.x = Mathf.Clamp(newScale.x, minZoom, maxZoom);
                newScale.y = Mathf.Clamp(newScale.y, minZoom, maxZoom);
                newScale.z = 1;

                imageRectTransform.localScale = newScale;

                // Ensure the image stays within bounds
                ClampToScreenBounds();
            }
        }
        else
        {
            // Reset the initial touch distance when no pinch is happening
            initialTouchDistance = Vector2.zero;
        }
    }

    // Handle dragging the image using the New Input System
    private void HandlePanDrag()
    {
        if (UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches.Count == 1 && !isDragging)
        {
            var touch = UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches[0];
            if (touch.phase == UnityEngine.InputSystem.TouchPhase.Began)
            {
                isDragging = true;
                dragStartPos = touch.screenPosition;
                imageStartPos = imageRectTransform.localPosition;
            }
        }

        if (UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches.Count == 1 && isDragging)
        {
            var touch = UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches[0];
            if (touch.phase == UnityEngine.InputSystem.TouchPhase.Moved)
            {
                // Calculate drag delta
                Vector2 dragDelta = touch.screenPosition - dragStartPos;

                // Move the image according to the drag delta
                imageRectTransform.localPosition = imageStartPos + new Vector3(dragDelta.x, dragDelta.y, 0);

                // Ensure the image stays within screen bounds
                ClampToScreenBounds();
            }
        }

        if (UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches.Count == 0)
        {
            // Reset dragging state when no touches are active
            isDragging = false;
        }
    }

    // Ensure the image stays within the screen bounds during dragging
    private void ClampToScreenBounds()
    {
        Vector3 position = imageRectTransform.localPosition;
        Vector3 scale = imageRectTransform.localScale;

        float halfWidth = imageRectTransform.rect.width * scale.x / 2;
        float halfHeight = imageRectTransform.rect.height * scale.y / 2;

        RectTransform canvasRectTransform = imageRectTransform.GetComponentInParent<Canvas>().GetComponent<RectTransform>();

        // Calculate bounds for the canvas
        float canvasWidth = canvasRectTransform.rect.width;
        float canvasHeight = canvasRectTransform.rect.height;

        // Clamp the image's position to ensure it doesn't move beyond the canvas boundaries
        position.x = Mathf.Clamp(position.x, canvasWidth / 2 - halfWidth, halfWidth - canvasWidth / 2);
        position.y = Mathf.Clamp(position.y, canvasHeight / 2 - halfHeight, halfHeight - canvasHeight / 2);

        // Apply the clamped position
        imageRectTransform.localPosition = position;
    }
}
