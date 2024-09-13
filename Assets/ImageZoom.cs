using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ImageZoom : MonoBehaviour, IDragHandler, IPointerDownHandler
{
    public float zoomSpeed = 0.05f;            // Speed of the zoom
    public float minZoom = 0.5f;               // Minimum zoom level
    public float maxZoom = 2.0f;               // Maximum zoom level
    public float zoomOutDuration = 1.0f;       // Duration of the initial zoom-out animation
    public float zoomInAmount = 1.5f;          // Amount to zoom back in after zooming out

    private RectTransform imageRectTransform;  // Reference to the RectTransform of the image
    private Image imageComponent;              // Reference to the Image component
    private bool isZoomingOut = true;          // Flag to indicate if the image is zooming out at the start
    private bool isZoomingIn = false;          // Flag to indicate if the image is zooming back in
    private float targetScale;                 // Target scale for the initial zoom-out
    private bool canDrag = false;              // Flag to allow dragging after zoom completes
    private Vector2 dragStartPos;              // Dragging start position (in local space)
    private Vector3 imageStartPos;             // Initial image position when dragging starts (in local space)

    private void Awake()
    {
        // Get the Image and RectTransform components
        imageComponent = GetComponent<Image>();
        imageRectTransform = imageComponent.rectTransform;

        // Ensure the pivot is at the center of the RectTransform
        imageRectTransform.pivot = new Vector2(0.5f, 0.5f);  // Set the pivot to the center
    }

    private void Start()
    {
        // Set the image to fit the screen size initially
        StartZoomOut();
    }

    private void Update()
    {
        // Handle the initial zoom-out animation
        if (isZoomingOut)
        {
            float step = zoomOutDuration * Time.deltaTime;
            imageRectTransform.localScale = Vector3.Lerp(imageRectTransform.localScale, new Vector3(targetScale, targetScale, 1), step);

            // Once zoom-out is complete, start zooming back in
            if (Mathf.Abs(imageRectTransform.localScale.x - targetScale) < 0.01f)
            {
                isZoomingOut = false;
                StartZoomIn();
            }
        }
        // Handle zooming back in
        else if (isZoomingIn)
        {
            float step = zoomOutDuration * Time.deltaTime;
            float zoomInTargetScale = targetScale * zoomInAmount;
            imageRectTransform.localScale = Vector3.Lerp(imageRectTransform.localScale, new Vector3(zoomInTargetScale, zoomInTargetScale, 1), step);

            // Once zoom-in is complete, enable dragging
            if (Mathf.Abs(imageRectTransform.localScale.x - zoomInTargetScale) < 0.01f)
            {
                isZoomingIn = false;
                canDrag = true;
            }
        }
    }

    // Method to start the zoom-out animation
    private void StartZoomOut()
    {
        // Calculate the target scale to fit the image to the screen
        float screenRatio = (float)Screen.width / Screen.height;
        float imageRatio = imageRectTransform.rect.width / imageRectTransform.rect.height;

        if (imageRatio > screenRatio)
        {
            // If the image is wider than the screen, fit by width
            targetScale = Screen.width / imageRectTransform.rect.width;
        }
        else
        {
            // Otherwise, fit by height
            targetScale = Screen.height / imageRectTransform.rect.height;
        }

        // Start the zoom-out process
        isZoomingOut = true;

        // Ensure the position stays centered when zooming out
        imageRectTransform.localPosition = Vector3.zero;
    }

    // Method to start the zoom-in animation
    private void StartZoomIn()
    {
        isZoomingIn = true;
    }

    // Ensure the image stays within the screen boundaries during dragging
    private void ClampToScreenBounds()
    {
        Vector3 position = imageRectTransform.localPosition;
        Vector3 scale = imageRectTransform.localScale;

        float halfWidth = imageRectTransform.rect.width * scale.x / 2;
        float halfHeight = imageRectTransform.rect.height * scale.y / 2;

        // Calculate the screen limits (to ensure the image never leaves the screen)
        float minX = Screen.width / 2 - halfWidth;
        float maxX = halfWidth - Screen.width / 2;
        float minY = Screen.height / 2 - halfHeight;
        float maxY = halfHeight - Screen.height / 2;

        // Clamp the position so the image stays within the screen boundaries
        position.x = Mathf.Clamp(position.x, minX, maxX);
        position.y = Mathf.Clamp(position.y, minY, maxY);

        imageRectTransform.localPosition = position;
    }

    // Handle pointer down to start dragging
    public void OnPointerDown(PointerEventData eventData)
    {
        if (!canDrag) return;

        // Capture the start position of the drag
        dragStartPos = eventData.position;
        imageStartPos = imageRectTransform.localPosition; // Capture the image's current position
    }

    // Handle dragging the image
    public void OnDrag(PointerEventData eventData)
    {
        if (!canDrag) return;

        // Calculate the drag delta
        Vector2 dragDelta = eventData.position - dragStartPos;

        // Update the image's position
        imageRectTransform.localPosition = imageStartPos + new Vector3(dragDelta.x, dragDelta.y, 0);

        // Clamp the image within the screen boundaries after dragging
        ClampToScreenBounds();
    }
}
