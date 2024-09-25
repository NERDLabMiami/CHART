using UnityEngine;
using UnityEngine.InputSystem;

public class PinchToZoom : MonoBehaviour
{
    public InputActionAsset inputActions;   // Reference to the Input Action Asset
    public float minZoom = 0.5f;            // Minimum zoom level
    public float maxZoom = 2.0f;            // Maximum zoom level
    public float zoomSpeed = 0.1f;          // Zoom speed with the mouse scroll

    private InputAction zoomAction;         // Input action for zoom
    private InputAction panAction;          // Input action for drag
    private InputAction mouseScrollAction;  // Input action for mouse scroll

    private RectTransform imageRectTransform;
    private Vector3 initialScale;
    private Vector2 dragStartPos;
    private Vector3 imageStartPos;
    private bool isDragging = false;

    private Camera mainCamera;

    private void Awake()
    {
        // Get the RectTransform component of the image
        imageRectTransform = GetComponent<RectTransform>();
        mainCamera = Camera.main;

        // Setup input actions
        zoomAction = inputActions.FindAction("PinchZoom");
        panAction = inputActions.FindAction("Drag");
        mouseScrollAction = inputActions.FindAction("MouseScroll");
    }

    private void OnEnable()
    {
        // Enable input actions
        zoomAction.Enable();
        panAction.Enable();
        mouseScrollAction.Enable();
    }

    private void OnDisable()
    {
        // Disable input actions
        zoomAction.Disable();
        panAction.Disable();
        mouseScrollAction.Disable();
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

    // Handle pinch-to-zoom using input actions (for touch devices)
    private void HandlePinchZoom()
    {
        // Check if zoom action is being performed
        if (zoomAction.IsInProgress())
        {
            Vector2 pinchValue = zoomAction.ReadValue<Vector2>();
            float pinchDelta = pinchValue.y;

            // Calculate new scale
            Vector3 newScale = imageRectTransform.localScale;
            newScale += Vector3.one * pinchDelta * zoomSpeed;

            // Clamp the zoom level
            newScale.x = Mathf.Clamp(newScale.x, minZoom, maxZoom);
            newScale.y = Mathf.Clamp(newScale.y, minZoom, maxZoom);
            newScale.z = 1;

            // Apply the new scale
            imageRectTransform.localScale = newScale;

            // Ensure the image stays within bounds
            ClampToScreenBounds();
        }
    }

    // Handle dragging the image using input actions (for touch devices)
    private void HandlePanDrag()
    {
        if (panAction.IsInProgress() && !isDragging)
        {
            Vector2 dragInput = panAction.ReadValue<Vector2>();

            // Start dragging
            dragStartPos = dragInput;
            imageStartPos = imageRectTransform.localPosition;
            isDragging = true;
        }

        if (isDragging)
        {
            Vector2 dragInput = panAction.ReadValue<Vector2>();

            // Calculate drag delta
            Vector2 dragDelta = dragInput - dragStartPos;

            // Move the image according to the drag delta
            imageRectTransform.localPosition = imageStartPos + new Vector3(dragDelta.x, dragDelta.y, 0);

            // Ensure the image stays within screen bounds
            ClampToScreenBounds();
        }

        if (panAction.WasReleasedThisFrame())
        {
            // Reset dragging state when touch ends
            isDragging = false;
        }
    }

    // Handle zooming with the mouse scroll (editor mode)
    private void HandleMouseZoom()
    {
        if (mouseScrollAction.IsInProgress())
        {
            float scrollDelta = mouseScrollAction.ReadValue<Vector2>().y;

            if (scrollDelta != 0)
            {
                // Calculate new scale
                Vector3 newScale = imageRectTransform.localScale;
                newScale += Vector3.one * scrollDelta * zoomSpeed;

                // Clamp the zoom level
                newScale.x = Mathf.Clamp(newScale.x, minZoom, maxZoom);
                newScale.y = Mathf.Clamp(newScale.y, minZoom, maxZoom);
                newScale.z = 1;

                // Apply the new scale
                imageRectTransform.localScale = newScale;

                // Ensure the image stays within bounds
                ClampToScreenBounds();
            }
        }
    }

    // Handle dragging using the mouse (editor mode)
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
