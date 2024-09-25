using UnityEngine;
using UnityEngine.UI;

public class ImageZoomOut : MonoBehaviour
{
    public float zoomSpeed = 0.5f;      // Speed of the zoom out
    public float zoomOutDuration = 2.0f; // Duration of the zoom-out animation (in seconds)

    private RectTransform imageRectTransform;  // Reference to the RectTransform of the image
    private RectTransform canvasRectTransform; // Reference to the Canvas's RectTransform
    private Vector3 initialScale;              // Initial scale of the image
    private Vector3 targetScale;               // Target scale based on the image-to-screen ratio
    private bool isZoomingOut = false;         // Flag to control the zoom-out animation
    private float zoomOutTimeElapsed = 0.0f;   // Track how much time has passed during zoom-out

    private void Awake()
    {
        // Get the RectTransform component of the image and the parent canvas
        imageRectTransform = GetComponent<RectTransform>();
        canvasRectTransform = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        // When the image is activated, start the zoom-out
        StartZoomOut();
    }

    private void Update()
    {
        if (isZoomingOut)
        {
            Debug.Log("Zooming Out");
            // Calculate the progress of the zoom-out based on the elapsed time and total duration
            zoomOutTimeElapsed += Time.deltaTime;
            float progress = zoomOutTimeElapsed / zoomOutDuration;

            // Smoothly interpolate the scale from initial to target
            imageRectTransform.localScale = Vector3.Lerp(initialScale, targetScale, progress);

            // Stop zooming once the target scale is reached
            if (progress >= 1.0f)
            {
                isZoomingOut = false;
            }
        }
    }

    // Method to start the zoom-out process
    public void StartZoomOut()
    {
        // Reset the timer
        zoomOutTimeElapsed = 0.0f;

        // Start from the current scale
        initialScale = imageRectTransform.localScale;

        // Calculate the target scale to make the image fill the screen
        CalculateTargetZoomLevel();

        // Start the zoom-out process
        isZoomingOut = true;
    }

    // Calculate the target zoom level so the image fills the screen
    private void CalculateTargetZoomLevel()
    {
        // Get the dimensions of the image and the canvas
        float imageWidth = imageRectTransform.rect.width;
        float imageHeight = imageRectTransform.rect.height;

        float canvasWidth = canvasRectTransform.rect.width;
        float canvasHeight = canvasRectTransform.rect.height;

        // Calculate aspect ratios
        float imageAspectRatio = imageWidth / imageHeight;
        float canvasAspectRatio = canvasWidth / canvasHeight;

        // Determine the target scale to make the image fill the canvas
        if (imageAspectRatio > canvasAspectRatio)
        {
            // If the image is wider than the canvas, fit by width
            targetScale = new Vector3(canvasWidth / imageWidth, canvasWidth / imageWidth, 1.0f);
        }
        else
        {
            // If the image is taller than the canvas, fit by height
            targetScale = new Vector3(canvasHeight / imageHeight, canvasHeight / imageHeight, 1.0f);
        }
    }
}
